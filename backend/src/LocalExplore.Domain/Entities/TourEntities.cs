using LocalExplore.Domain.Common;
using LocalExplore.Domain.Enums;

namespace LocalExplore.Domain.Entities;

public class Tour : AuditableEntity
{
    public int HostId { get; set; }
    public virtual HostProfile Host { get; set; } = null!;

    public int CategoryId { get; set; }
    public virtual Category Category { get; set; } = null!;

    public int LocationId { get; set; }
    public virtual Location Location { get; set; } = null!;

    public int CancellationPolicyId { get; set; }
    public virtual CancellationPolicy CancellationPolicy { get; set; } = null!;

    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
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

    public TourStatus Status { get; set; } = TourStatus.Draft;
    public string? RejectionReason { get; set; }

    public decimal AverageRating { get; set; } = 0.00m;
    public int ReviewCount { get; set; } = 0;
    public int ViewCount { get; set; } = 0;

    // Navigation properties
    public virtual ICollection<TourImage> Images { get; set; } = new List<TourImage>();
    public virtual ICollection<TourItinerary> Itineraries { get; set; } = new List<TourItinerary>();
    public virtual ICollection<TourSchedule> Schedules { get; set; } = new List<TourSchedule>();
    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
    public virtual ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
}

public class TourImage : BaseEntity
{
    public int TourId { get; set; }
    public virtual Tour Tour { get; set; } = null!;

    public string ImageUrl { get; set; } = string.Empty;
    public string? Caption { get; set; }
    public bool IsCover { get; set; } = false;
    public int DisplayOrder { get; set; } = 0;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class TourItinerary : BaseEntity
{
    public int TourId { get; set; }
    public virtual Tour Tour { get; set; } = null!;

    public int StepOrder { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int DurationMinutes { get; set; }
    public string? LocationName { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class TourSchedule : AuditableEntity
{
    public int TourId { get; set; }
    public virtual Tour Tour { get; set; } = null!;

    public DateOnly DepartureDate { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public decimal Price { get; set; }
    public int MaxCapacity { get; set; }
    public int BookedCapacity { get; set; } = 0;

    public ScheduleStatus Status { get; set; } = ScheduleStatus.Available;

    // Concurrency control token
    public byte[]? RowVersion { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
