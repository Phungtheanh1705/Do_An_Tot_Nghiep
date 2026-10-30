using LocalExplore.Application.Common.Models;
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

public class BookingServiceTests
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
    public async Task CreateBooking_WhenRequestedCapacityExceedsRemaining_ShouldFailWithErrorMessage()
    {
        // Arrange
        var db = CreateInMemoryDbContext(Guid.NewGuid().ToString());
        var location = new Location { Id = 1, Name = "Sapa", ProvinceName = "Lào Cai", Slug = "sapa" };
        db.Locations.Add(location);

        var tour = new Tour
        {
            Id = 1,
            Title = "Tour Test Trải nghiệm",
            Slug = "tour-test",
            BasePrice = 300000,
            HostId = 1,
            CategoryId = 1,
            LocationId = 1,
            Location = location,
            CancellationPolicyId = 1,
            Status = TourStatus.Published
        };
        db.Tours.Add(tour);

        // Schedule with max capacity 10, booked 8 => remaining 2
        var schedule = new TourSchedule
        {
            Id = 101,
            TourId = 1,
            Tour = tour,
            DepartureDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(7)),
            StartTime = new TimeOnly(8, 0),
            EndTime = new TimeOnly(12, 0),
            Price = 300000,
            MaxCapacity = 10,
            BookedCapacity = 8,
            Status = ScheduleStatus.Available,
            CreatedAt = DateTime.UtcNow
        };
        db.TourSchedules.Add(schedule);
        await db.SaveChangesAsync();

        var bookingService = new BookingService(db);
        var request = new CreateBookingRequest
        {
            ScheduleId = 101,
            ContactName = "Nguyễn Văn A",
            ContactPhone = "0912345678",
            ContactEmail = "a@test.com",
            Participants = new List<BookingParticipantDto>
            {
                new() { FullName = "Khách 1" },
                new() { FullName = "Khách 2" },
                new() { FullName = "Khách 3" } // Requesting 3 spots while only 2 are left!
            }
        };

        // Act
        var result = await bookingService.CreateBookingAsync(customerId: 99, request);

        // Assert
        result.Success.Should().BeFalse();
        result.Message.Should().Contain("chỉ còn trống 2 chỗ");
    }

    [Fact]
    public async Task CreateBooking_WhenCapacitySufficient_ShouldCreateBookingWithPendingPayment()
    {
        // Arrange
        var db = CreateInMemoryDbContext(Guid.NewGuid().ToString());
        var customer = new User
        {
            Id = 99,
            Email = "customer@test.com",
            PasswordHash = "hash",
            FullName = "Khách Hàng Test",
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
        db.Users.Add(customer);

        var location = new Location { Id = 1, Name = "Hà Nội", ProvinceName = "Hà Nội", Slug = "ha-noi" };
        db.Locations.Add(location);

        var tour = new Tour
        {
            Id = 1,
            Title = "Tour Test Đủ Chỗ",
            Slug = "tour-du-cho",
            BasePrice = 500000,
            HostId = 1,
            CategoryId = 1,
            LocationId = 1,
            Location = location,
            CancellationPolicyId = 1,
            Status = TourStatus.Published
        };
        db.Tours.Add(tour);

        var schedule = new TourSchedule
        {
            Id = 102,
            TourId = 1,
            Tour = tour,
            DepartureDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(5)),
            StartTime = new TimeOnly(9, 0),
            EndTime = new TimeOnly(16, 0),
            Price = 500000,
            MaxCapacity = 10,
            BookedCapacity = 4,
            Status = ScheduleStatus.Available,
            CreatedAt = DateTime.UtcNow
        };
        db.TourSchedules.Add(schedule);
        await db.SaveChangesAsync();

        var bookingService = new BookingService(db);
        var request = new CreateBookingRequest
        {
            ScheduleId = 102,
            ContactName = "Nguyễn Văn A",
            ContactPhone = "0987654321",
            ContactEmail = "a@test.com",
            Participants = new List<BookingParticipantDto>
            {
                new() { FullName = "Người Lớn 1" },
                new() { FullName = "Người Lớn 2" }
            }
        };

        // Act
        var result = await bookingService.CreateBookingAsync(customerId: 99, request);

        // Assert
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data.BookingCode.Should().StartWith("LE-");
        result.Data.TotalParticipants.Should().Be(2);
        result.Data.FinalAmount.Should().Be(1000000); // 500k * 2
        result.Data.PaymentStatus.Should().Be(PaymentStatus.Unpaid);

        // Verify database updated booked capacity
        var updatedSchedule = await db.TourSchedules.FindAsync(102);
        updatedSchedule!.BookedCapacity.Should().Be(6); // 4 + 2
    }

    [Theory]
    [InlineData(100, 100)] // 100 hours before departure (>72h) => 100% refund
    [InlineData(48, 50)]   // 48 hours before departure (24-72h) => 50% refund
    [InlineData(12, 0)]    // 12 hours before departure (<24h) => 0% refund
    public async Task CancelBooking_ShouldCalculateRefundAccuratelyBasedOnPolicy(int hoursUntilDeparture, int expectedRefundPercentage)
    {
        // Arrange
        var db = CreateInMemoryDbContext(Guid.NewGuid().ToString());
        var policy = new CancellationPolicy
        {
            Id = 1,
            Name = "Chính sách Tiêu chuẩn",
            Description = "Hoàn 100% trước 72h, 50% từ 24-72h, 0% dưới 24h",
            RefundPercentageBefore72h = 100,
            RefundPercentageBetween24And72h = 50,
            RefundPercentageUnder24h = 0,
            IsDefault = true
        };
        db.CancellationPolicies.Add(policy);

        var tour = new Tour
        {
            Id = 1,
            Title = "Tour Hủy Trải Nghiệm",
            Slug = "tour-huy",
            BasePrice = 1000000,
            HostId = 1,
            CategoryId = 1,
            LocationId = 1,
            CancellationPolicyId = 1,
            CancellationPolicy = policy,
            Status = TourStatus.Published
        };
        db.Tours.Add(tour);

        var departureDateTime = DateTime.UtcNow.AddHours(hoursUntilDeparture);
        var schedule = new TourSchedule
        {
            Id = 201,
            TourId = 1,
            DepartureDate = DateOnly.FromDateTime(departureDateTime),
            StartTime = TimeOnly.FromDateTime(departureDateTime),
            EndTime = TimeOnly.FromDateTime(departureDateTime).AddHours(4),
            Price = 1000000,
            MaxCapacity = 10,
            BookedCapacity = 2,
            Status = ScheduleStatus.Available,
            Tour = tour
        };
        db.TourSchedules.Add(schedule);

        var booking = new Booking
        {
            Id = 301,
            BookingCode = "LE-CANCEL-TEST",
            CustomerId = 99,
            ScheduleId = 201,
            ContactName = "Khách Hàng Test",
            TotalParticipants = 2,
            FinalAmount = 2000000,
            Status = BookingStatus.Confirmed,
            PaymentStatus = PaymentStatus.Paid,
            QrCodeToken = "TOKEN-CANCEL-TEST",
            CreatedAt = DateTime.UtcNow.AddDays(-2),
            Schedule = schedule
        };
        db.Bookings.Add(booking);
        await db.SaveChangesAsync();

        var bookingService = new BookingService(db);

        // Act
        var result = await bookingService.CancelBookingAsync(customerId: 99, bookingId: 301);

        // Assert
        result.Success.Should().BeTrue();
        result.Message.Should().Contain($"{expectedRefundPercentage}%");

        var updatedBooking = await db.Bookings.FindAsync(301);
        updatedBooking!.Status.Should().Be(BookingStatus.Cancelled);

        // Verify capacity is freed up if >= 24h, else retained by host
        var updatedSchedule = await db.TourSchedules.FindAsync(201);
        if (hoursUntilDeparture >= 24)
        {
            updatedSchedule!.BookedCapacity.Should().Be(0); // 2 - 2
        }
        else
        {
            updatedSchedule!.BookedCapacity.Should().Be(2); // Non-refundable retains capacity
        }
    }
}
