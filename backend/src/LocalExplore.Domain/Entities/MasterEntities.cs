using LocalExplore.Domain.Common;

namespace LocalExplore.Domain.Entities;

public class HostProfile : AuditableEntity
{
    public int UserId { get; set; }
    public virtual User User { get; set; } = null!;

    public string? Bio { get; set; }
    public string IdentityCardNumber { get; set; } = string.Empty;
    public DateOnly? IssueDate { get; set; }
    public string? IssuePlace { get; set; }
    public string? FrontCardUrl { get; set; }
    public string? BackCardUrl { get; set; }

    public string BankName { get; set; } = string.Empty;
    public string BankAccountNumber { get; set; } = string.Empty;
    public string BankAccountHolder { get; set; } = string.Empty;

    public int ExperienceYears { get; set; } = 1;
    public bool IsApproved { get; set; } = false;
    public DateTime? ApprovedAt { get; set; }
    public int? ApprovedByAdminId { get; set; }
    public virtual User? ApprovedByAdmin { get; set; }
    public string? RejectionReason { get; set; }

    public decimal RatingAverage { get; set; } = 5.00m;
    public int TotalToursCount { get; set; } = 0;

    public virtual ICollection<Tour> Tours { get; set; } = new List<Tour>();
}

public class Category : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? IconUrl { get; set; }
    public string? ImageUrl { get; set; }
    public int DisplayOrder { get; set; } = 0;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public virtual ICollection<Tour> Tours { get; set; } = new List<Tour>();
}

public class Location : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string ProvinceName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public bool IsPopular { get; set; } = false;
    public int DisplayOrder { get; set; } = 0;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public virtual ICollection<Tour> Tours { get; set; } = new List<Tour>();
}

public class CancellationPolicy : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int RefundPercentageBefore72h { get; set; } = 100;
    public int RefundPercentageBetween24And72h { get; set; } = 50;
    public int RefundPercentageUnder24h { get; set; } = 0;
    public bool IsDefault { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public virtual ICollection<Tour> Tours { get; set; } = new List<Tour>();
}
