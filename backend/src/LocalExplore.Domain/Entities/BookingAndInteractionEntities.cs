using LocalExplore.Domain.Common;
using LocalExplore.Domain.Enums;

namespace LocalExplore.Domain.Entities;

public class Booking : AuditableEntity
{
    public string BookingCode { get; set; } = string.Empty;

    public int CustomerId { get; set; }
    public virtual User Customer { get; set; } = null!;

    public int ScheduleId { get; set; }
    public virtual TourSchedule Schedule { get; set; } = null!;

    public string ContactName { get; set; } = string.Empty;
    public string ContactEmail { get; set; } = string.Empty;
    public string ContactPhone { get; set; } = string.Empty;

    public int TotalParticipants { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal DiscountAmount { get; set; } = 0.00m;
    public decimal FinalAmount { get; set; }

    public BookingStatus Status { get; set; } = BookingStatus.PendingPayment;
    public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Unpaid;
    public string? Notes { get; set; }
    public string QrCodeToken { get; set; } = string.Empty;
    public DateTime ExpirePaymentAt { get; set; }

    // Navigation properties
    public virtual ICollection<BookingParticipant> Participants { get; set; } = new List<BookingParticipant>();
    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
    public virtual CheckIn? CheckIn { get; set; }
    public virtual Review? Review { get; set; }
}

public class BookingParticipant : BaseEntity
{
    public int BookingId { get; set; }
    public virtual Booking Booking { get; set; } = null!;

    public string FullName { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public ParticipantType ParticipantType { get; set; } = ParticipantType.Adult;
    public DateOnly? BirthDate { get; set; }
    public string? SpecialRequest { get; set; }
}

public class Payment : BaseEntity
{
    public int BookingId { get; set; }
    public virtual Booking Booking { get; set; } = null!;

    public string TransactionCode { get; set; } = string.Empty;
    public string PaymentMethod { get; set; } = string.Empty; // Sandbox_VNPAY, Sandbox_MOMO, Direct
    public decimal Amount { get; set; }
    public string Status { get; set; } = "Pending"; // Pending, Success, Failed, Cancelled
    public string? GatewayTransactionNo { get; set; }
    public string? GatewayResponse { get; set; }
    public DateTime? PaidAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class CheckIn : BaseEntity
{
    public int BookingId { get; set; }
    public virtual Booking Booking { get; set; } = null!;

    public int CheckedInByHostId { get; set; }
    public virtual User CheckedInByHost { get; set; } = null!;

    public DateTime CheckedInAt { get; set; } = DateTime.UtcNow;
    public string? DeviceInfo { get; set; }
    public string? Notes { get; set; }
}

public class Review : AuditableEntity
{
    public int BookingId { get; set; }
    public virtual Booking Booking { get; set; } = null!;

    public int CustomerId { get; set; }
    public virtual User Customer { get; set; } = null!;

    public int TourId { get; set; }
    public virtual Tour Tour { get; set; } = null!;

    public int Rating { get; set; } // 1 - 5
    public string Comment { get; set; } = string.Empty;
    public string? ImageUrls { get; set; } // JSON array

    public string? HostReply { get; set; }
    public DateTime? HostRepliedAt { get; set; }
    public bool IsHidden { get; set; } = false;
}

public class Favorite : BaseEntity
{
    public int UserId { get; set; }
    public virtual User User { get; set; } = null!;

    public int TourId { get; set; }
    public virtual Tour Tour { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class Notification : BaseEntity
{
    public int UserId { get; set; }
    public virtual User User { get; set; } = null!;

    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string NotificationType { get; set; } = string.Empty;
    public string? TargetUrl { get; set; }
    public bool IsRead { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
