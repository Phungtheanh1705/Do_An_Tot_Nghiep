using LocalExplore.Domain.Enums;

namespace LocalExplore.Application.DTOs;

public class TourSummaryDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string ShortDescription { get; set; } = string.Empty;
    public string CoverImageUrl { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
    public string LocationName { get; set; } = string.Empty;
    public string ProvinceName { get; set; } = string.Empty;
    public decimal BasePrice { get; set; }
    public decimal DurationHours { get; set; }
    public decimal AverageRating { get; set; }
    public int ReviewCount { get; set; }
    public string HostName { get; set; } = string.Empty;
    public string? HostAvatarUrl { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public TourStatus Status { get; set; }
}

public class TourDetailDto : TourSummaryDto
{
    public string DetailedDescription { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string SupportedLanguages { get; set; } = string.Empty;
    public int MinAge { get; set; }
    public string? IncludedServices { get; set; }
    public string? ExcludedServices { get; set; }
    public string? ImportantNotes { get; set; }
    public string CancellationPolicyName { get; set; } = string.Empty;
    public string CancellationPolicyDescription { get; set; } = string.Empty;

    public List<TourImageDto> Images { get; set; } = new();
    public List<TourItineraryDto> Itineraries { get; set; } = new();
    public List<TourScheduleDto> AvailableSchedules { get; set; } = new();
    public List<ReviewDto> RecentReviews { get; set; } = new();
}

public class TourImageDto
{
    public int Id { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string? Caption { get; set; }
    public bool IsCover { get; set; }
    public int DisplayOrder { get; set; }
}

public class TourItineraryDto
{
    public int StepOrder { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int DurationMinutes { get; set; }
    public string? LocationName { get; set; }
}

public class TourScheduleDto
{
    public int Id { get; set; }
    public int TourId { get; set; }
    public DateOnly DepartureDate { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public decimal Price { get; set; }
    public int MaxCapacity { get; set; }
    public int BookedCapacity { get; set; }
    public int RemainingCapacity => MaxCapacity - BookedCapacity;
    public ScheduleStatus Status { get; set; }
}

public class TourFilterRequest
{
    public string? Keyword { get; set; }
    public int? CategoryId { get; set; }
    public int? LocationId { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public DateOnly? DepartureDate { get; set; }
    public decimal? MinRating { get; set; }
    public string? SortBy { get; set; } // 'price_asc', 'price_desc', 'rating', 'newest'
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class CreateTourRequest
{
    public int CategoryId { get; set; }
    public int LocationId { get; set; }
    public int CancellationPolicyId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string ShortDescription { get; set; } = string.Empty;
    public string DetailedDescription { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public decimal DurationHours { get; set; }
    public string SupportedLanguages { get; set; } = "Tiếng Việt";
    public int MinAge { get; set; } = 5;
    public decimal BasePrice { get; set; }
    public string? IncludedServices { get; set; }
    public string? ExcludedServices { get; set; }
    public string? ImportantNotes { get; set; }

    public List<TourImageDto> Images { get; set; } = new();
    public List<TourItineraryDto> Itineraries { get; set; } = new();
}

public class CreateScheduleRequest
{
    public DateOnly DepartureDate { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public decimal Price { get; set; }
    public int MaxCapacity { get; set; }
}
