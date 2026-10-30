using LocalExplore.Application.Common.Models;
using LocalExplore.Application.DTOs;
using LocalExplore.Application.Interfaces;
using LocalExplore.Domain.Entities;
using LocalExplore.Domain.Enums;
using LocalExplore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LocalExplore.Infrastructure.Services;

public class TourService : ITourService
{
    private readonly ApplicationDbContext _context;

    public TourService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResponse<TourSummaryDto>> GetToursAsync(TourFilterRequest filter)
    {
        var query = _context.Tours
            .Include(t => t.Category)
            .Include(t => t.Location)
            .Include(t => t.Host)
                .ThenInclude(h => h.User)
            .Include(t => t.Images)
            .Include(t => t.Schedules)
            .Where(t => t.Status == TourStatus.Published)
            .AsNoTracking();

        // 1. Keyword search
        if (!string.IsNullOrWhiteSpace(filter.Keyword))
        {
            var kw = filter.Keyword.Trim().ToLower();
            query = query.Where(t => t.Title.ToLower().Contains(kw) ||
                                     t.ShortDescription.ToLower().Contains(kw) ||
                                     t.Category.Name.ToLower().Contains(kw) ||
                                     t.Location.Name.ToLower().Contains(kw) ||
                                     t.Location.ProvinceName.ToLower().Contains(kw));
        }

        // 2. Category filter
        if (filter.CategoryId.HasValue)
        {
            query = query.Where(t => t.CategoryId == filter.CategoryId.Value);
        }

        // 3. Location filter
        if (filter.LocationId.HasValue)
        {
            query = query.Where(t => t.LocationId == filter.LocationId.Value);
        }

        // 4. Price range filter
        if (filter.MinPrice.HasValue)
        {
            query = query.Where(t => t.BasePrice >= filter.MinPrice.Value);
        }
        if (filter.MaxPrice.HasValue)
        {
            query = query.Where(t => t.BasePrice <= filter.MaxPrice.Value);
        }

        // 5. Min rating
        if (filter.MinRating.HasValue)
        {
            query = query.Where(t => t.AverageRating >= filter.MinRating.Value);
        }

        // 6. Departure Date
        if (filter.DepartureDate.HasValue)
        {
            var date = filter.DepartureDate.Value;
            query = query.Where(t => t.Schedules.Any(s => s.DepartureDate == date && s.Status == ScheduleStatus.Available));
        }

        // 7. Sorting
        query = filter.SortBy switch
        {
            "price_asc" => query.OrderBy(t => t.BasePrice),
            "price_desc" => query.OrderByDescending(t => t.BasePrice),
            "rating" => query.OrderByDescending(t => t.AverageRating),
            _ => query.OrderByDescending(t => t.CreatedAt)
        };

        var totalCount = await query.CountAsync();
        var pageIndex = Math.Max(1, filter.PageIndex);
        var pageSize = Math.Max(1, Math.Min(50, filter.PageSize));

        var items = await query
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .Select(t => new TourSummaryDto
            {
                Id = t.Id,
                Title = t.Title,
                Slug = t.Slug,
                ShortDescription = t.ShortDescription,
                CoverImageUrl = t.Images.Where(i => i.IsCover).Select(i => i.ImageUrl).FirstOrDefault()
                                ?? t.Images.Select(i => i.ImageUrl).FirstOrDefault()
                                ?? "https://images.unsplash.com/photo-1506744038136-46273834b3fb?w=800",
                CategoryName = t.Category.Name,
                LocationName = t.Location.Name,
                ProvinceName = t.Location.ProvinceName,
                BasePrice = t.BasePrice,
                DurationHours = t.DurationHours,
                AverageRating = t.AverageRating,
                ReviewCount = t.ReviewCount,
                HostName = t.Host.User.FullName,
                HostAvatarUrl = t.Host.User.AvatarUrl,
                Latitude = t.Latitude,
                Longitude = t.Longitude,
                Status = t.Status
            })
            .ToListAsync();

        return new PagedResponse<TourSummaryDto>(items, totalCount, pageIndex, pageSize);
    }

    public async Task<ApiResponse<TourDetailDto>> GetTourBySlugAsync(string slug)
    {
        var tour = await _context.Tours
            .Include(t => t.Category)
            .Include(t => t.Location)
            .Include(t => t.Host)
                .ThenInclude(h => h.User)
            .Include(t => t.CancellationPolicy)
            .Include(t => t.Images.OrderBy(i => i.DisplayOrder))
            .Include(t => t.Itineraries.OrderBy(i => i.StepOrder))
            .Include(t => t.Schedules.Where(s => s.Status == ScheduleStatus.Available && s.DepartureDate >= DateOnly.FromDateTime(DateTime.UtcNow)).OrderBy(s => s.DepartureDate))
            .Include(t => t.Reviews.Where(r => !r.IsHidden).OrderByDescending(r => r.CreatedAt).Take(10))
                .ThenInclude(r => r.Customer)
            .FirstOrDefaultAsync(t => t.Slug == slug);

        if (tour == null)
        {
            return ApiResponse<TourDetailDto>.FailureResult("Không tìm thấy tour trải nghiệm.");
        }

        // Increase view count
        tour.ViewCount += 1;
        await _context.SaveChangesAsync();

        var dto = MapToTourDetailDto(tour);
        return ApiResponse<TourDetailDto>.SuccessResult(dto);
    }

    public async Task<ApiResponse<TourDetailDto>> GetTourByIdAsync(int id)
    {
        var tour = await _context.Tours
            .Include(t => t.Category)
            .Include(t => t.Location)
            .Include(t => t.Host)
                .ThenInclude(h => h.User)
            .Include(t => t.CancellationPolicy)
            .Include(t => t.Images.OrderBy(i => i.DisplayOrder))
            .Include(t => t.Itineraries.OrderBy(i => i.StepOrder))
            .Include(t => t.Schedules.OrderBy(s => s.DepartureDate))
            .Include(t => t.Reviews.Where(r => !r.IsHidden).OrderByDescending(r => r.CreatedAt))
                .ThenInclude(r => r.Customer)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (tour == null)
        {
            return ApiResponse<TourDetailDto>.FailureResult("Không tìm thấy tour.");
        }

        var dto = MapToTourDetailDto(tour);
        return ApiResponse<TourDetailDto>.SuccessResult(dto);
    }

    public async Task<ApiResponse<int>> CreateTourAsync(int hostUserId, CreateTourRequest request)
    {
        var host = await _context.HostProfiles.FirstOrDefaultAsync(h => h.UserId == hostUserId);
        if (host == null || !host.IsApproved)
        {
            return ApiResponse<int>.FailureResult("Tài khoản của bạn chưa được duyệt làm Local Host.");
        }

        var slug = GenerateSlug(request.Title);
        var existingSlug = await _context.Tours.AnyAsync(t => t.Slug == slug);
        if (existingSlug)
        {
            slug = $"{slug}-{DateTime.UtcNow.Ticks % 10000}";
        }

        var tour = new Tour
        {
            HostId = host.Id,
            CategoryId = request.CategoryId,
            LocationId = request.LocationId,
            CancellationPolicyId = request.CancellationPolicyId,
            Title = request.Title.Trim(),
            Slug = slug,
            ShortDescription = request.ShortDescription.Trim(),
            DetailedDescription = request.DetailedDescription.Trim(),
            Address = request.Address.Trim(),
            Latitude = request.Latitude,
            Longitude = request.Longitude,
            DurationHours = request.DurationHours,
            SupportedLanguages = request.SupportedLanguages,
            MinAge = request.MinAge,
            BasePrice = request.BasePrice,
            IncludedServices = request.IncludedServices,
            ExcludedServices = request.ExcludedServices,
            ImportantNotes = request.ImportantNotes,
            Status = TourStatus.PendingApproval, // Send directly for Admin approval
            CreatedAt = DateTime.UtcNow
        };

        if (request.Images != null && request.Images.Any())
        {
            foreach (var img in request.Images)
            {
                tour.Images.Add(new TourImage
                {
                    ImageUrl = img.ImageUrl,
                    Caption = img.Caption,
                    IsCover = img.IsCover,
                    DisplayOrder = img.DisplayOrder,
                    CreatedAt = DateTime.UtcNow
                });
            }
        }

        if (request.Itineraries != null && request.Itineraries.Any())
        {
            foreach (var itin in request.Itineraries)
            {
                tour.Itineraries.Add(new TourItinerary
                {
                    StepOrder = itin.StepOrder,
                    Title = itin.Title,
                    Description = itin.Description,
                    DurationMinutes = itin.DurationMinutes,
                    LocationName = itin.LocationName,
                    CreatedAt = DateTime.UtcNow
                });
            }
        }

        _context.Tours.Add(tour);
        host.TotalToursCount += 1;
        await _context.SaveChangesAsync();

        return ApiResponse<int>.SuccessResult(tour.Id, "Tạo tour thành công! Tour đã được gửi đến Admin để kiểm duyệt.");
    }

    public async Task<ApiResponse<bool>> UpdateTourAsync(int hostUserId, int tourId, CreateTourRequest request)
    {
        var host = await _context.HostProfiles.FirstOrDefaultAsync(h => h.UserId == hostUserId);
        if (host == null)
        {
            return ApiResponse<bool>.FailureResult("Không tìm thấy thông tin Host.");
        }

        var tour = await _context.Tours
            .Include(t => t.Images)
            .Include(t => t.Itineraries)
            .FirstOrDefaultAsync(t => t.Id == tourId && t.HostId == host.Id);

        if (tour == null)
        {
            return ApiResponse<bool>.FailureResult("Bạn không có quyền chỉnh sửa tour này.");
        }

        tour.CategoryId = request.CategoryId;
        tour.LocationId = request.LocationId;
        tour.CancellationPolicyId = request.CancellationPolicyId;
        tour.Title = request.Title.Trim();
        tour.ShortDescription = request.ShortDescription.Trim();
        tour.DetailedDescription = request.DetailedDescription.Trim();
        tour.Address = request.Address.Trim();
        tour.Latitude = request.Latitude;
        tour.Longitude = request.Longitude;
        tour.DurationHours = request.DurationHours;
        tour.SupportedLanguages = request.SupportedLanguages;
        tour.MinAge = request.MinAge;
        tour.BasePrice = request.BasePrice;
        tour.IncludedServices = request.IncludedServices;
        tour.ExcludedServices = request.ExcludedServices;
        tour.ImportantNotes = request.ImportantNotes;
        tour.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return ApiResponse<bool>.SuccessResult(true, "Cập nhật tour thành công.");
    }

    public async Task<ApiResponse<int>> AddScheduleAsync(int hostUserId, int tourId, CreateScheduleRequest request)
    {
        var host = await _context.HostProfiles.FirstOrDefaultAsync(h => h.UserId == hostUserId);
        var tour = await _context.Tours.FirstOrDefaultAsync(t => t.Id == tourId && t.HostId == host!.Id);
        if (tour == null)
        {
            return ApiResponse<int>.FailureResult("Không tìm thấy tour hoặc bạn không sở hữu tour này.");
        }

        var schedule = new TourSchedule
        {
            TourId = tourId,
            DepartureDate = request.DepartureDate,
            StartTime = request.StartTime,
            EndTime = request.EndTime,
            Price = request.Price > 0 ? request.Price : tour.BasePrice,
            MaxCapacity = request.MaxCapacity,
            BookedCapacity = 0,
            Status = ScheduleStatus.Available,
            CreatedAt = DateTime.UtcNow
        };

        _context.TourSchedules.Add(schedule);
        await _context.SaveChangesAsync();

        return ApiResponse<int>.SuccessResult(schedule.Id, "Thêm lịch khởi hành thành công!");
    }

    public async Task<ApiResponse<List<TourScheduleDto>>> GetTourSchedulesAsync(int tourId)
    {
        var schedules = await _context.TourSchedules
            .Where(s => s.TourId == tourId && s.Status == ScheduleStatus.Available && s.DepartureDate >= DateOnly.FromDateTime(DateTime.UtcNow))
            .OrderBy(s => s.DepartureDate)
            .ThenBy(s => s.StartTime)
            .Select(s => new TourScheduleDto
            {
                Id = s.Id,
                TourId = s.TourId,
                DepartureDate = s.DepartureDate,
                StartTime = s.StartTime,
                EndTime = s.EndTime,
                Price = s.Price,
                MaxCapacity = s.MaxCapacity,
                BookedCapacity = s.BookedCapacity,
                Status = s.Status
            })
            .ToListAsync();

        return ApiResponse<List<TourScheduleDto>>.SuccessResult(schedules);
    }

    public async Task<ApiResponse<bool>> ApproveTourAsync(int tourId)
    {
        var tour = await _context.Tours.FindAsync(tourId);
        if (tour == null) return ApiResponse<bool>.FailureResult("Không tìm thấy tour.");

        tour.Status = TourStatus.Published;
        tour.RejectionReason = null;
        tour.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return ApiResponse<bool>.SuccessResult(true, "Đã phê duyệt và xuất bản tour lên sàn!");
    }

    public async Task<ApiResponse<bool>> RejectTourAsync(int tourId, string reason)
    {
        var tour = await _context.Tours.FindAsync(tourId);
        if (tour == null) return ApiResponse<bool>.FailureResult("Không tìm thấy tour.");

        tour.Status = TourStatus.Rejected;
        tour.RejectionReason = reason;
        tour.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return ApiResponse<bool>.SuccessResult(true, "Đã từ chối duyệt tour.");
    }

    public async Task<ApiResponse<List<TourSummaryDto>>> GetHostToursAsync(int hostUserId)
    {
        var host = await _context.HostProfiles.Include(h => h.User).FirstOrDefaultAsync(h => h.UserId == hostUserId);
        if (host == null) return ApiResponse<List<TourSummaryDto>>.FailureResult("Không tìm thấy hồ sơ Host.");

        var tours = await _context.Tours
            .Include(t => t.Category)
            .Include(t => t.Location)
            .Include(t => t.Images)
            .Where(t => t.HostId == host.Id)
            .OrderByDescending(t => t.CreatedAt)
            .Select(t => new TourSummaryDto
            {
                Id = t.Id,
                Title = t.Title,
                Slug = t.Slug,
                ShortDescription = t.ShortDescription,
                CoverImageUrl = t.Images.Where(i => i.IsCover).Select(i => i.ImageUrl).FirstOrDefault()
                                ?? t.Images.Select(i => i.ImageUrl).FirstOrDefault()
                                ?? "https://images.unsplash.com/photo-1506744038136-46273834b3fb?w=800",
                CategoryName = t.Category.Name,
                LocationName = t.Location.Name,
                ProvinceName = t.Location.ProvinceName,
                BasePrice = t.BasePrice,
                DurationHours = t.DurationHours,
                AverageRating = t.AverageRating,
                ReviewCount = t.ReviewCount,
                HostName = t.Host.User.FullName,
                Latitude = t.Latitude,
                Longitude = t.Longitude,
                Status = t.Status
            })
            .ToListAsync();

        return ApiResponse<List<TourSummaryDto>>.SuccessResult(tours);
    }

    private static TourDetailDto MapToTourDetailDto(Tour tour)
    {
        return new TourDetailDto
        {
            Id = tour.Id,
            Title = tour.Title,
            Slug = tour.Slug,
            ShortDescription = tour.ShortDescription,
            DetailedDescription = tour.DetailedDescription,
            CoverImageUrl = tour.Images.Where(i => i.IsCover).Select(i => i.ImageUrl).FirstOrDefault()
                            ?? tour.Images.Select(i => i.ImageUrl).FirstOrDefault()
                            ?? "https://images.unsplash.com/photo-1506744038136-46273834b3fb?w=800",
            CategoryName = tour.Category.Name,
            LocationName = tour.Location.Name,
            ProvinceName = tour.Location.ProvinceName,
            Address = tour.Address,
            Latitude = tour.Latitude,
            Longitude = tour.Longitude,
            DurationHours = tour.DurationHours,
            SupportedLanguages = tour.SupportedLanguages,
            MinAge = tour.MinAge,
            BasePrice = tour.BasePrice,
            IncludedServices = tour.IncludedServices,
            ExcludedServices = tour.ExcludedServices,
            ImportantNotes = tour.ImportantNotes,
            AverageRating = tour.AverageRating,
            ReviewCount = tour.ReviewCount,
            HostName = tour.Host.User.FullName,
            HostAvatarUrl = tour.Host.User.AvatarUrl,
            Status = tour.Status,
            CancellationPolicyName = tour.CancellationPolicy.Name,
            CancellationPolicyDescription = tour.CancellationPolicy.Description,
            Images = tour.Images.Select(i => new TourImageDto
            {
                Id = i.Id,
                ImageUrl = i.ImageUrl,
                Caption = i.Caption,
                IsCover = i.IsCover,
                DisplayOrder = i.DisplayOrder
            }).ToList(),
            Itineraries = tour.Itineraries.Select(itin => new TourItineraryDto
            {
                StepOrder = itin.StepOrder,
                Title = itin.Title,
                Description = itin.Description,
                DurationMinutes = itin.DurationMinutes,
                LocationName = itin.LocationName
            }).ToList(),
            AvailableSchedules = tour.Schedules.Select(s => new TourScheduleDto
            {
                Id = s.Id,
                TourId = s.TourId,
                DepartureDate = s.DepartureDate,
                StartTime = s.StartTime,
                EndTime = s.EndTime,
                Price = s.Price,
                MaxCapacity = s.MaxCapacity,
                BookedCapacity = s.BookedCapacity,
                Status = s.Status
            }).ToList(),
            RecentReviews = tour.Reviews.Select(r => new ReviewDto
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
            }).ToList()
        };
    }

    private static string GenerateSlug(string title)
    {
        var normalized = title.ToLowerInvariant().Trim();
        var chars = normalized.Select(c => char.IsLetterOrDigit(c) ? c : '-').ToArray();
        var slug = new string(chars);
        while (slug.Contains("--")) slug = slug.Replace("--", "-");
        return slug.Trim('-');
    }
}
