namespace LocalExplore.Domain.Enums;

public enum RoleType
{
    Customer = 1,
    Host = 2,
    Admin = 3
}

public enum TourStatus
{
    Draft,
    PendingApproval,
    Published,
    Rejected,
    Hidden,
    Archived
}

public enum ScheduleStatus
{
    Available,
    FullyBooked,
    Cancelled,
    Completed
}

public enum BookingStatus
{
    PendingPayment,
    Paid,
    Confirmed,
    Cancelled,
    Completed,
    Refunded,
    Rejected
}

public enum PaymentStatus
{
    Unpaid,
    Pending,
    Paid,
    Refunded,
    Failed
}

public enum ParticipantType
{
    Adult,
    Child
}
