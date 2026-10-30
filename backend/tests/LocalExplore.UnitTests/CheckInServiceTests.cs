using LocalExplore.Application.DTOs;
using LocalExplore.Domain.Entities;
using LocalExplore.Domain.Enums;
using LocalExplore.Infrastructure.Data;
using LocalExplore.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Xunit;
using FluentAssertions;

namespace LocalExplore.UnitTests;

public class CheckInServiceTests
{
    private ApplicationDbContext CreateInMemoryDbContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        return new ApplicationDbContext(options);
    }

    [Fact]
    public async Task VerifyCheckIn_WhenFirstScannedByHost_ShouldSucceedAndMarkStatusCompleted()
    {
        // Arrange
        var db = CreateInMemoryDbContext(Guid.NewGuid().ToString());
        const int hostUserId = 10;
        const int hostProfileId = 5;

        var hostUser = new User
        {
            Id = hostUserId,
            FullName = "Vàng A Thào",
            Email = "host.sapa@test.com",
            PasswordHash = "hash"
        };
        db.Users.Add(hostUser);

        var host = new HostProfile
        {
            Id = hostProfileId,
            UserId = hostUserId,
            User = hostUser,
            IdentityCardNumber = "001234567890",
            BankName = "Vietcombank",
            BankAccountNumber = "9988776655",
            BankAccountHolder = "VANG A THAO",
            IsApproved = true
        };
        db.HostProfiles.Add(host);

        var customer = new User
        {
            Id = 88,
            FullName = "Phạm Thùy Linh",
            Email = "linh@test.com",
            PasswordHash = "hash"
        };
        db.Users.Add(customer);

        var tour = new Tour
        {
            Id = 1,
            Title = "Tour Sa Pa",
            Slug = "tour-sa-pa",
            HostId = hostProfileId,
            CategoryId = 1,
            LocationId = 1,
            CancellationPolicyId = 1,
            Status = TourStatus.Published,
            Host = host
        };
        db.Tours.Add(tour);

        var schedule = new TourSchedule
        {
            Id = 50,
            TourId = 1,
            DepartureDate = DateOnly.FromDateTime(DateTime.UtcNow),
            StartTime = new TimeOnly(8, 0),
            EndTime = new TimeOnly(12, 0),
            Price = 450000,
            Tour = tour
        };
        db.TourSchedules.Add(schedule);

        var booking = new Booking
        {
            Id = 77,
            BookingCode = "LE-CHECKIN-FIRST",
            CustomerId = 88,
            Customer = customer,
            ContactName = "Phạm Thùy Linh",
            ContactPhone = "0988112233",
            ContactEmail = "linh@test.com",
            ScheduleId = 50,
            Schedule = schedule,
            TotalParticipants = 2,
            FinalAmount = 900000,
            Status = BookingStatus.Confirmed,
            PaymentStatus = PaymentStatus.Paid,
            QrCodeToken = "VALID-QR-TOKEN-12345",
            CreatedAt = DateTime.UtcNow
        };
        db.Bookings.Add(booking);
        await db.SaveChangesAsync();

        var checkInService = new CheckInService(db);
        var request = new CheckInRequest
        {
            QrCodeToken = "VALID-QR-TOKEN-12345",
            Notes = "Cổng bản Cát Cát"
        };

        // Act
        var result = await checkInService.VerifyAndCheckInAsync(hostUserId, request);

        // Assert
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data.CustomerName.Should().Be("Phạm Thùy Linh");
        result.Data.TotalParticipants.Should().Be(2);

        // Check DB state
        var updatedBooking = await db.Bookings.FindAsync(77);
        updatedBooking!.Status.Should().Be(BookingStatus.Completed);

        var checkInRecord = await db.CheckIns.FirstOrDefaultAsync(c => c.BookingId == 77);
        checkInRecord.Should().NotBeNull();
        checkInRecord!.CheckedInByHostId.Should().Be(hostUserId);
    }

    [Fact]
    public async Task VerifyCheckIn_WhenScannedSecondTime_ShouldFailWithSingleUseViolationError()
    {
        // Arrange
        var db = CreateInMemoryDbContext(Guid.NewGuid().ToString());
        const int hostUserId = 10;
        const int hostProfileId = 5;

        var hostUser = new User
        {
            Id = hostUserId,
            FullName = "Vàng A Thào",
            Email = "host.sapa@test.com",
            PasswordHash = "hash"
        };
        db.Users.Add(hostUser);

        var host = new HostProfile
        {
            Id = hostProfileId,
            UserId = hostUserId,
            User = hostUser,
            IdentityCardNumber = "001234567890",
            BankName = "Vietcombank",
            BankAccountNumber = "9988776655",
            BankAccountHolder = "VANG A THAO",
            IsApproved = true
        };
        db.HostProfiles.Add(host);

        var customer = new User
        {
            Id = 88,
            FullName = "Khách Test",
            Email = "test@test.com",
            PasswordHash = "hash"
        };
        db.Users.Add(customer);

        var tour = new Tour
        {
            Id = 1,
            Title = "Tour Sa Pa",
            Slug = "tour-sa-pa",
            HostId = hostProfileId,
            CategoryId = 1,
            LocationId = 1,
            CancellationPolicyId = 1,
            Status = TourStatus.Published,
            Host = host
        };
        db.Tours.Add(tour);

        var schedule = new TourSchedule
        {
            Id = 50,
            TourId = 1,
            DepartureDate = DateOnly.FromDateTime(DateTime.UtcNow),
            StartTime = new TimeOnly(8, 0),
            EndTime = new TimeOnly(12, 0),
            Price = 450000,
            Tour = tour
        };
        db.TourSchedules.Add(schedule);

        var existingCheckIn = new CheckIn
        {
            Id = 999,
            BookingId = 88,
            CheckedInByHostId = hostUserId,
            CheckedInAt = DateTime.UtcNow.AddMinutes(-30)
        };

        var booking = new Booking
        {
            Id = 88,
            BookingCode = "LE-CHECKIN-SECOND",
            CustomerId = 88,
            Customer = customer,
            ContactName = "Khách Test",
            ContactPhone = "0988112233",
            ContactEmail = "test@test.com",
            ScheduleId = 50,
            Schedule = schedule,
            TotalParticipants = 1,
            Status = BookingStatus.Completed,
            PaymentStatus = PaymentStatus.Paid,
            QrCodeToken = "ALREADY-USED-TOKEN",
            CheckIn = existingCheckIn,
            CreatedAt = DateTime.UtcNow
        };
        existingCheckIn.Booking = booking;

        db.Bookings.Add(booking);
        db.CheckIns.Add(existingCheckIn);
        await db.SaveChangesAsync();

        var checkInService = new CheckInService(db);
        var request = new CheckInRequest
        {
            QrCodeToken = "ALREADY-USED-TOKEN"
        };

        // Act
        var result = await checkInService.VerifyAndCheckInAsync(hostUserId, request);

        // Assert
        result.Success.Should().BeFalse();
        result.Message.Should().Contain("MÃ VÉ ĐÃ ĐƯỢC CHECK-IN");
    }

    [Fact]
    public async Task VerifyCheckIn_WhenInvalidToken_ShouldReturnFailure()
    {
        // Arrange
        var db = CreateInMemoryDbContext(Guid.NewGuid().ToString());
        var hostUser = new User
        {
            Id = 10,
            FullName = "Vàng A Thào",
            Email = "host.sapa@test.com",
            PasswordHash = "hash"
        };
        db.Users.Add(hostUser);

        var host = new HostProfile
        {
            Id = 1,
            UserId = 10,
            User = hostUser,
            IdentityCardNumber = "001234567890",
            BankName = "Vietcombank",
            BankAccountNumber = "9988776655",
            BankAccountHolder = "VANG A THAO",
            IsApproved = true
        };
        db.HostProfiles.Add(host);
        await db.SaveChangesAsync();

        var checkInService = new CheckInService(db);
        var request = new CheckInRequest
        {
            QrCodeToken = "NON-EXISTENT-TOKEN"
        };

        // Act
        var result = await checkInService.VerifyAndCheckInAsync(10, request);

        // Assert
        result.Success.Should().BeFalse();
        result.Message.Should().Contain("Không tìm thấy thông tin vé từ mã QR này");
    }
}
