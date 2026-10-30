using System.Data;
using System.Security.Cryptography;
using LocalExplore.Application.Common.Models;
using LocalExplore.Application.DTOs;
using LocalExplore.Application.Interfaces;
using LocalExplore.Domain.Entities;
using LocalExplore.Domain.Enums;
using LocalExplore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LocalExplore.Infrastructure.Services;

public class BookingService : IBookingService
{
    private readonly ApplicationDbContext _context;

    public BookingService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<BookingDetailDto>> CreateBookingAsync(int customerId, CreateBookingRequest request)
    {
        if (request.Participants == null || !request.Participants.Any())
        {
            return ApiResponse<BookingDetailDto>.FailureResult("Vui lòng cung cấp danh sách người tham gia.");
        }

        var totalGuests = request.Participants.Count;

        // Bọc trong Database Transaction với IsolationLevel Serializable để chống Overbooking tuyệt đối
        await using var transaction = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable);

        try
        {
            var schedule = await _context.TourSchedules
                .Include(s => s.Tour)
                    .ThenInclude(t => t.Location)
                .Include(s => s.Tour)
                    .ThenInclude(t => t.Images)
                .FirstOrDefaultAsync(s => s.Id == request.ScheduleId);

            if (schedule == null)
            {
                return ApiResponse<BookingDetailDto>.FailureResult("Lịch khởi hành không tồn tại.");
            }

            if (schedule.Status != ScheduleStatus.Available)
            {
                return ApiResponse<BookingDetailDto>.FailureResult("Lịch khởi hành này hiện không còn nhận đặt chỗ.");
            }

            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            if (schedule.DepartureDate < today)
            {
                return ApiResponse<BookingDetailDto>.FailureResult("Không thể đặt lịch khởi hành trong quá khứ.");
            }

            // Kiểm tra số chỗ còn lại
            var remainingCapacity = schedule.MaxCapacity - schedule.BookedCapacity;
            if (totalGuests > remainingCapacity)
            {
                return ApiResponse<BookingDetailDto>.FailureResult($"Lịch khởi hành chỉ còn trống {remainingCapacity} chỗ. Bạn đang yêu cầu {totalGuests} chỗ.");
            }

            // Tạm trừ số chỗ trống
            schedule.BookedCapacity += totalGuests;
            if (schedule.BookedCapacity >= schedule.MaxCapacity)
            {
                schedule.Status = ScheduleStatus.FullyBooked;
            }

            // Tính toán tổng tiền
            var unitPrice = schedule.Price;
            var totalAmount = unitPrice * totalGuests;
            var discountAmount = 0.00m;
            var finalAmount = totalAmount - discountAmount;

            var bookingCode = $"LE-{DateTime.UtcNow:yyMMdd}-{RandomNumberGenerator.GetInt32(1000, 9999)}";
            var qrToken = $"LE-QR-{Guid.NewGuid():N}";

            var booking = new Booking
            {
                BookingCode = bookingCode,
                CustomerId = customerId,
                ScheduleId = schedule.Id,
                ContactName = request.ContactName.Trim(),
                ContactEmail = request.ContactEmail.Trim(),
                ContactPhone = request.ContactPhone.Trim(),
                TotalParticipants = totalGuests,
                TotalAmount = totalAmount,
                DiscountAmount = discountAmount,
                FinalAmount = finalAmount,
                Status = BookingStatus.PendingPayment,
                PaymentStatus = PaymentStatus.Unpaid,
                Notes = request.Notes,
                QrCodeToken = qrToken,
                ExpirePaymentAt = DateTime.UtcNow.AddMinutes(15),
                CreatedAt = DateTime.UtcNow
            };

            foreach (var p in request.Participants)
            {
                booking.Participants.Add(new BookingParticipant
                {
                    FullName = p.FullName.Trim(),
                    Gender = p.Gender,
                    ParticipantType = p.ParticipantType,
                    BirthDate = p.BirthDate,
                    SpecialRequest = p.SpecialRequest
                });
            }

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            var detailDto = MapToBookingDetailDto(booking, schedule.Tour, schedule);
            return ApiResponse<BookingDetailDto>.SuccessResult(detailDto, "Giữ chỗ thành công! Vui lòng hoàn tất thanh toán trong vòng 15 phút.");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return ApiResponse<BookingDetailDto>.FailureResult($"Lỗi khi đặt chỗ: {ex.Message}");
        }
    }

    public async Task<ApiResponse<List<BookingDetailDto>>> GetMyBookingsAsync(int customerId)
    {
        var bookings = await _context.Bookings
            .Include(b => b.Schedule)
                .ThenInclude(s => s.Tour)
                    .ThenInclude(t => t.Location)
            .Include(b => b.Schedule)
                .ThenInclude(s => s.Tour)
                    .ThenInclude(t => t.Images)
            .Include(b => b.Participants)
            .Include(b => b.CheckIn)
            .Include(b => b.Review)
            .Where(b => b.CustomerId == customerId)
            .OrderByDescending(b => b.CreatedAt)
            .ToListAsync();

        var list = bookings.Select(b => MapToBookingDetailDto(b, b.Schedule.Tour, b.Schedule)).ToList();
        return ApiResponse<List<BookingDetailDto>>.SuccessResult(list);
    }

    public async Task<ApiResponse<BookingDetailDto>> GetBookingByCodeAsync(string bookingCode, int requestingUserId, bool isAdmin)
    {
        var booking = await _context.Bookings
            .Include(b => b.Customer)
            .Include(b => b.Schedule)
                .ThenInclude(s => s.Tour)
                    .ThenInclude(t => t.Location)
            .Include(b => b.Schedule)
                .ThenInclude(s => s.Tour)
                    .ThenInclude(t => t.Images)
            .Include(b => b.Schedule)
                .ThenInclude(s => s.Tour)
                    .ThenInclude(t => t.Host)
            .Include(b => b.Participants)
            .Include(b => b.CheckIn)
            .Include(b => b.Review)
            .FirstOrDefaultAsync(b => b.BookingCode == bookingCode);

        if (booking == null)
        {
            return ApiResponse<BookingDetailDto>.FailureResult("Không tìm thấy đơn đặt tour.");
        }

        // Quyền truy cập: Admin, Customer sở hữu đơn, hoặc Host sở hữu tour
        var isOwner = booking.CustomerId == requestingUserId;
        var isTourHost = booking.Schedule.Tour.Host.UserId == requestingUserId;

        if (!isAdmin && !isOwner && !isTourHost)
        {
            return ApiResponse<BookingDetailDto>.FailureResult("Bạn không có quyền xem thông tin vé này.");
        }

        var detail = MapToBookingDetailDto(booking, booking.Schedule.Tour, booking.Schedule);
        return ApiResponse<BookingDetailDto>.SuccessResult(detail);
    }

    public async Task<ApiResponse<bool>> CancelBookingAsync(int customerId, int bookingId)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable);

        try
        {
            var booking = await _context.Bookings
                .Include(b => b.Schedule)
                    .ThenInclude(s => s.Tour)
                        .ThenInclude(t => t.CancellationPolicy)
                .FirstOrDefaultAsync(b => b.Id == bookingId && b.CustomerId == customerId);

            if (booking == null)
            {
                return ApiResponse<bool>.FailureResult("Không tìm thấy đơn đặt tour.");
            }

            if (booking.Status == BookingStatus.Cancelled || booking.Status == BookingStatus.Completed)
            {
                return ApiResponse<bool>.FailureResult($"Không thể hủy đơn ở trạng thái {booking.Status}.");
            }

            var schedule = booking.Schedule;
            var departureDateTime = schedule.DepartureDate.ToDateTime(schedule.StartTime);
            var hoursUntilDeparture = (departureDateTime - DateTime.UtcNow).TotalHours;

            var policy = schedule.Tour.CancellationPolicy;
            decimal refundPercentage = 0;

            if (hoursUntilDeparture > 72)
            {
                refundPercentage = policy.RefundPercentageBefore72h;
            }
            else if (hoursUntilDeparture >= 24)
            {
                refundPercentage = policy.RefundPercentageBetween24And72h;
            }
            else
            {
                refundPercentage = policy.RefundPercentageUnder24h;
            }

            // Cập nhật trạng thái
            booking.Status = BookingStatus.Cancelled;
            booking.PaymentStatus = refundPercentage > 0 ? PaymentStatus.Refunded : booking.PaymentStatus;
            booking.UpdatedAt = DateTime.UtcNow;

            // Hoàn trả số chỗ cho schedule nếu hủy hợp lệ
            if (hoursUntilDeparture >= 24)
            {
                schedule.BookedCapacity = Math.Max(0, schedule.BookedCapacity - booking.TotalParticipants);
                schedule.Status = ScheduleStatus.Available;
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            var refundAmount = (booking.FinalAmount * refundPercentage) / 100m;
            return ApiResponse<bool>.SuccessResult(true, $"Hủy tour thành công! Theo chính sách, bạn được hoàn {refundPercentage}% (khoảng {refundAmount:N0} VNĐ).");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return ApiResponse<bool>.FailureResult($"Lỗi khi hủy tour: {ex.Message}");
        }
    }

    public async Task<ApiResponse<List<BookingDetailDto>>> GetHostBookingsAsync(int hostUserId)
    {
        var host = await _context.HostProfiles.FirstOrDefaultAsync(h => h.UserId == hostUserId);
        if (host == null)
        {
            return ApiResponse<List<BookingDetailDto>>.FailureResult("Không tìm thấy hồ sơ Host.");
        }

        var bookings = await _context.Bookings
            .Include(b => b.Customer)
            .Include(b => b.Schedule)
                .ThenInclude(s => s.Tour)
                    .ThenInclude(t => t.Location)
            .Include(b => b.Schedule)
                .ThenInclude(s => s.Tour)
                    .ThenInclude(t => t.Images)
            .Include(b => b.Participants)
            .Include(b => b.CheckIn)
            .Where(b => b.Schedule.Tour.HostId == host.Id)
            .OrderByDescending(b => b.CreatedAt)
            .ToListAsync();

        var list = bookings.Select(b => MapToBookingDetailDto(b, b.Schedule.Tour, b.Schedule)).ToList();
        return ApiResponse<List<BookingDetailDto>>.SuccessResult(list);
    }

    public async Task<ApiResponse<bool>> SimulatePaymentSuccessAsync(int bookingId, string method)
    {
        var booking = await _context.Bookings.FindAsync(bookingId);
        if (booking == null) return ApiResponse<bool>.FailureResult("Không tìm thấy đơn đặt tour.");

        if (booking.PaymentStatus == PaymentStatus.Paid)
        {
            return ApiResponse<bool>.FailureResult("Đơn đặt này đã được thanh toán trước đó.");
        }

        booking.PaymentStatus = PaymentStatus.Paid;
        booking.Status = BookingStatus.Confirmed;
        booking.UpdatedAt = DateTime.UtcNow;

        var payment = new Payment
        {
            BookingId = booking.Id,
            TransactionCode = $"TRANS-{DateTime.UtcNow:yyyyMMddHHmmss}-{RandomNumberGenerator.GetInt32(1000, 9999)}",
            PaymentMethod = method,
            Amount = booking.FinalAmount,
            Status = "Success",
            GatewayTransactionNo = $"MOCK-GW-{RandomNumberGenerator.GetInt32(100000, 999999)}",
            PaidAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow
        };

        _context.Payments.Add(payment);
        await _context.SaveChangesAsync();

        return ApiResponse<bool>.SuccessResult(true, "Thanh toán giả lập thành công! Vé điện tử và mã QR đã được kích hoạt.");
    }

    private static BookingDetailDto MapToBookingDetailDto(Booking b, Tour tour, TourSchedule s)
    {
        return new BookingDetailDto
        {
            Id = b.Id,
            BookingCode = b.BookingCode,
            CustomerId = b.CustomerId,
            CustomerName = b.ContactName,
            TourId = tour.Id,
            TourTitle = tour.Title,
            TourSlug = tour.Slug,
            TourCoverImage = tour.Images.Where(i => i.IsCover).Select(i => i.ImageUrl).FirstOrDefault()
                             ?? tour.Images.Select(i => i.ImageUrl).FirstOrDefault()
                             ?? "https://images.unsplash.com/photo-1506744038136-46273834b3fb?w=800",
            TourAddress = tour.Address,
            TourLatitude = tour.Latitude,
            TourLongitude = tour.Longitude,
            ScheduleId = s.Id,
            DepartureDate = s.DepartureDate,
            StartTime = s.StartTime,
            EndTime = s.EndTime,
            ContactName = b.ContactName,
            ContactEmail = b.ContactEmail,
            ContactPhone = b.ContactPhone,
            TotalParticipants = b.TotalParticipants,
            TotalAmount = b.TotalAmount,
            DiscountAmount = b.DiscountAmount,
            FinalAmount = b.FinalAmount,
            Status = b.Status,
            PaymentStatus = b.PaymentStatus,
            Notes = b.Notes,
            QrCodeToken = b.QrCodeToken,
            ExpirePaymentAt = b.ExpirePaymentAt,
            CreatedAt = b.CreatedAt,
            IsCheckedIn = b.CheckIn != null,
            CheckedInAt = b.CheckIn?.CheckedInAt,
            HasReviewed = b.Review != null,
            Participants = b.Participants.Select(p => new BookingParticipantDto
            {
                FullName = p.FullName,
                Gender = p.Gender,
                ParticipantType = p.ParticipantType,
                BirthDate = p.BirthDate,
                SpecialRequest = p.SpecialRequest
            }).ToList()
        };
    }
}
