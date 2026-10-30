using LocalExplore.Domain.Enums;

namespace LocalExplore.Application.DTOs;

public class CreateBookingRequest
{
    public int ScheduleId { get; set; }
    public string ContactName { get; set; } = string.Empty;
    public string ContactEmail { get; set; } = string.Empty;
    public string ContactPhone { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public List<BookingParticipantDto> Participants { get; set; } = new();
}

public class BookingParticipantDto
{
    public string FullName { get; set; } = string.Empty;
    public string Gender { get; set; } = "Nam";
    public ParticipantType ParticipantType { get; set; } = ParticipantType.Adult;
    public DateOnly? BirthDate { get; set; }
    public string? SpecialRequest { get; set; }
}

public class BookingDetailDto
{
    public int Id { get; set; }
    public string BookingCode { get; set; } = string.Empty;
    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;

    public int TourId { get; set; }
    public string TourTitle { get; set; } = string.Empty;
    public string TourSlug { get; set; } = string.Empty;
    public string TourCoverImage { get; set; } = string.Empty;
    public string TourAddress { get; set; } = string.Empty;
    public decimal TourLatitude { get; set; }
    public decimal TourLongitude { get; set; }

    public int ScheduleId { get; set; }
    public DateOnly DepartureDate { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }

    public string ContactName { get; set; } = string.Empty;
    public string ContactEmail { get; set; } = string.Empty;
    public string ContactPhone { get; set; } = string.Empty;

    public int TotalParticipants { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal FinalAmount { get; set; }

    public BookingStatus Status { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
    public string? Notes { get; set; }
    public string QrCodeToken { get; set; } = string.Empty;
    public DateTime ExpirePaymentAt { get; set; }
    public DateTime CreatedAt { get; set; }

    public bool IsCheckedIn { get; set; }
    public DateTime? CheckedInAt { get; set; }
    public bool HasReviewed { get; set; }

    public List<BookingParticipantDto> Participants { get; set; } = new();
}

public class CheckInRequest
{
    public string QrCodeToken { get; set; } = string.Empty;
    public string? DeviceInfo { get; set; }
    public string? Notes { get; set; }
}

public class CheckInResultDto
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? BookingCode { get; set; }
    public string? TourTitle { get; set; }
    public string? CustomerName { get; set; }
    public int TotalParticipants { get; set; }
    public DateTime? CheckedInAt { get; set; }
}

public class CreateReviewRequest
{
    public int BookingId { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; } = string.Empty;
    public List<string>? ImageUrls { get; set; }
}

public class ReviewDto
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string? CustomerAvatarUrl { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; } = string.Empty;
    public List<string>? ImageUrls { get; set; }
    public string? HostReply { get; set; }
    public DateTime? HostRepliedAt { get; set; }
    public DateTime CreatedAt { get; set; }
}
