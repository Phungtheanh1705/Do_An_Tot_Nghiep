using LocalExplore.Application.Common.Models;
using LocalExplore.Application.DTOs;
using LocalExplore.Application.Interfaces;
using LocalExplore.Domain.Entities;
using LocalExplore.Domain.Enums;
using LocalExplore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LocalExplore.Infrastructure.Services;

public class CheckInService : ICheckInService
{
    private readonly ApplicationDbContext _context;

    public CheckInService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<CheckInResultDto>> VerifyAndCheckInAsync(int hostUserId, CheckInRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.QrCodeToken))
        {
            return ApiResponse<CheckInResultDto>.FailureResult("Mã QR không hợp lệ hoặc để trống.");
        }

        var booking = await _context.Bookings
            .Include(b => b.Customer)
            .Include(b => b.Schedule)
                .ThenInclude(s => s.Tour)
                    .ThenInclude(t => t.Host)
            .Include(b => b.CheckIn)
            .FirstOrDefaultAsync(b => b.QrCodeToken == request.QrCodeToken.Trim());

        if (booking == null)
        {
            return ApiResponse<CheckInResultDto>.FailureResult("Không tìm thấy thông tin vé từ mã QR này.");
        }

        // 1. Kiểm tra quyền của Host
        var tourHostUserId = booking.Schedule.Tour.Host.UserId;
        var isAdmin = await _context.UserRoles.AnyAsync(ur => ur.UserId == hostUserId && ur.Role.NormalizedName == "ADMIN");

        if (tourHostUserId != hostUserId && !isAdmin)
        {
            return ApiResponse<CheckInResultDto>.FailureResult("Bạn không phải là Host sở hữu tour của vé này.");
        }

        // 2. Kiểm tra trạng thái thanh toán
        if (booking.Status != BookingStatus.Confirmed && booking.Status != BookingStatus.Paid && booking.Status != BookingStatus.Completed)
        {
            return ApiResponse<CheckInResultDto>.FailureResult($"Vé chưa đủ điều kiện tham gia (Trạng thái đơn: {booking.Status}).");
        }

        // 3. Kiểm tra đã quét chưa (Chống quét lặp)
        if (booking.CheckIn != null)
        {
            return ApiResponse<CheckInResultDto>.FailureResult(
                $"MÃ VÉ ĐÃ ĐƯỢC CHECK-IN LÚC {booking.CheckIn.CheckedInAt:HH:mm:ss dd/MM/yyyy}! Không thể tái sử dụng.",
                new List<string> { "Vé đã check-in trước đó." });
        }

        // 4. Ghi nhận check-in
        var checkIn = new CheckIn
        {
            BookingId = booking.Id,
            CheckedInByHostId = hostUserId,
            CheckedInAt = DateTime.UtcNow,
            DeviceInfo = request.DeviceInfo,
            Notes = request.Notes
        };

        _context.CheckIns.Add(checkIn);
        booking.Status = BookingStatus.Completed; // Chuyển sang Completed để khách có thể Review sau khi trải nghiệm
        await _context.SaveChangesAsync();

        var result = new CheckInResultDto
        {
            Success = true,
            Message = "Xác thực vé và Check-in thành công!",
            BookingCode = booking.BookingCode,
            TourTitle = booking.Schedule.Tour.Title,
            CustomerName = booking.ContactName,
            TotalParticipants = booking.TotalParticipants,
            CheckedInAt = checkIn.CheckedInAt
        };

        return ApiResponse<CheckInResultDto>.SuccessResult(result, "Check-in thành công! Chúc quý khách chuyến đi vui vẻ.");
    }
}

public class ReviewService : IReviewService
{
    private readonly ApplicationDbContext _context;

    public ReviewService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<ReviewDto>> CreateReviewAsync(int customerId, CreateReviewRequest request)
    {
        if (request.Rating < 1 || request.Rating > 5)
        {
            return ApiResponse<ReviewDto>.FailureResult("Số sao đánh giá phải từ 1 đến 5 sao.");
        }

        var booking = await _context.Bookings
            .Include(b => b.Review)
            .Include(b => b.Customer)
            .FirstOrDefaultAsync(b => b.Id == request.BookingId && b.CustomerId == customerId);

        if (booking == null)
        {
            return ApiResponse<ReviewDto>.FailureResult("Không tìm thấy đơn đặt tour tương ứng.");
        }

        if (booking.Status != BookingStatus.Completed)
        {
            return ApiResponse<ReviewDto>.FailureResult("Bạn chỉ có thể đánh giá chuyến đi sau khi tour đã hoàn thành.");
        }

        if (booking.Review != null)
        {
            return ApiResponse<ReviewDto>.FailureResult("Bạn đã đánh giá chuyến đi này rồi. Mỗi chuyến đi chỉ được đánh giá 1 lần.");
        }

        var tour = await _context.Tours.FindAsync(booking.ScheduleId > 0 ? (await _context.TourSchedules.Where(s => s.Id == booking.ScheduleId).Select(s => s.TourId).FirstOrDefaultAsync()) : 0);
        if (tour == null)
        {
            return ApiResponse<ReviewDto>.FailureResult("Không tìm thấy tour.");
        }

        var review = new Review
        {
            BookingId = booking.Id,
            CustomerId = customerId,
            TourId = tour.Id,
            Rating = request.Rating,
            Comment = request.Comment.Trim(),
            ImageUrls = request.ImageUrls != null ? System.Text.Json.JsonSerializer.Serialize(request.ImageUrls) : null,
            CreatedAt = DateTime.UtcNow
        };

        _context.Reviews.Add(review);
        await _context.SaveChangesAsync();

        // Tự động tính toán lại Điểm trung bình và số lượng review cho Tour
        var allReviews = await _context.Reviews
            .Where(r => r.TourId == tour.Id && !r.IsHidden)
            .ToListAsync();

        tour.ReviewCount = allReviews.Count;
        tour.AverageRating = Math.Round((decimal)allReviews.Average(r => r.Rating), 2);
        await _context.SaveChangesAsync();

        var dto = new ReviewDto
        {
            Id = review.Id,
            CustomerId = customerId,
            CustomerName = booking.Customer.FullName,
            CustomerAvatarUrl = booking.Customer.AvatarUrl,
            Rating = review.Rating,
            Comment = review.Comment,
            ImageUrls = request.ImageUrls,
            CreatedAt = review.CreatedAt
        };

        return ApiResponse<ReviewDto>.SuccessResult(dto, "Cảm ơn bạn đã chia sẻ đánh giá trải nghiệm!");
    }

    public async Task<ApiResponse<bool>> ReplyReviewAsync(int hostUserId, int reviewId, string reply)
    {
        var host = await _context.HostProfiles.FirstOrDefaultAsync(h => h.UserId == hostUserId);
        if (host == null) return ApiResponse<bool>.FailureResult("Không tìm thấy hồ sơ Host.");

        var review = await _context.Reviews
            .Include(r => r.Tour)
            .FirstOrDefaultAsync(r => r.Id == reviewId && r.Tour.HostId == host.Id);

        if (review == null)
        {
            return ApiResponse<bool>.FailureResult("Không tìm thấy đánh giá hoặc bạn không sở hữu tour này.");
        }

        review.HostReply = reply.Trim();
        review.HostRepliedAt = DateTime.UtcNow;
        review.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return ApiResponse<bool>.SuccessResult(true, "Phản hồi đánh giá thành công.");
    }

    public async Task<ApiResponse<List<ReviewDto>>> GetTourReviewsAsync(int tourId)
    {
        var reviews = await _context.Reviews
            .Include(r => r.Customer)
            .Where(r => r.TourId == tourId && !r.IsHidden)
            .OrderByDescending(r => r.CreatedAt)
            .Select(r => new ReviewDto
            {
                Id = r.Id,
                CustomerId = r.CustomerId,
                CustomerName = r.Customer.FullName,
                CustomerAvatarUrl = r.Customer.AvatarUrl,
                Rating = r.Rating,
                Comment = r.Comment,
                HostReply = r.HostReply,
                HostRepliedAt = r.HostRepliedAt,
                CreatedAt = r.CreatedAt
            })
            .ToListAsync();

        return ApiResponse<List<ReviewDto>>.SuccessResult(reviews);
    }
}
