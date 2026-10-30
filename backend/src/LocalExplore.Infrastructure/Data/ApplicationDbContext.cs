using LocalExplore.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LocalExplore.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();
    public DbSet<HostProfile> HostProfiles => Set<HostProfile>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Location> Locations => Set<Location>();
    public DbSet<CancellationPolicy> CancellationPolicies => Set<CancellationPolicy>();
    public DbSet<Tour> Tours => Set<Tour>();
    public DbSet<TourImage> TourImages => Set<TourImage>();
    public DbSet<TourItinerary> TourItineraries => Set<TourItinerary>();
    public DbSet<TourSchedule> TourSchedules => Set<TourSchedule>();
    public DbSet<Booking> Bookings => Set<Booking>();
    public DbSet<BookingParticipant> BookingParticipants => Set<BookingParticipant>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<CheckIn> CheckIns => Set<CheckIn>();
    public DbSet<Review> Reviews => Set<Review>();
    public DbSet<Favorite> Favorites => Set<Favorite>();
    public DbSet<Notification> Notifications => Set<Notification>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // 1. Users
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users");
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Email).IsUnique();
            entity.Property(e => e.Email).HasMaxLength(256).IsRequired();
            entity.Property(e => e.FullName).HasMaxLength(150).IsRequired();
            entity.Property(e => e.PasswordHash).HasMaxLength(500).IsRequired();
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
            entity.Property(e => e.AvatarUrl).HasMaxLength(500);
        });

        // 2. Roles
        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("Roles");
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Name).IsUnique();
            entity.Property(e => e.Name).HasMaxLength(50).IsRequired();
            entity.Property(e => e.NormalizedName).HasMaxLength(50).IsRequired();
        });

        // 3. UserRoles
        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.ToTable("UserRoles");
            entity.HasKey(e => new { e.UserId, e.RoleId });

            entity.HasOne(e => e.User)
                  .WithMany(u => u.UserRoles)
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Role)
                  .WithMany(r => r.UserRoles)
                  .HasForeignKey(e => e.RoleId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // 4. HostProfiles
        modelBuilder.Entity<HostProfile>(entity =>
        {
            entity.ToTable("HostProfiles");
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.UserId).IsUnique();

            entity.HasOne(e => e.User)
                  .WithOne(u => u.HostProfile)
                  .HasForeignKey<HostProfile>(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.ApprovedByAdmin)
                  .WithMany()
                  .HasForeignKey(e => e.ApprovedByAdminId)
                  .OnDelete(DeleteBehavior.NoAction);

            entity.Property(e => e.RatingAverage).HasPrecision(3, 2);
        });

        // 5. Categories
        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("Categories");
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Name).IsUnique();
            entity.HasIndex(e => e.Slug).IsUnique();
        });

        // 6. Locations
        modelBuilder.Entity<Location>(entity =>
        {
            entity.ToTable("Locations");
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Slug).IsUnique();
            entity.Property(e => e.Latitude).HasPrecision(9, 6);
            entity.Property(e => e.Longitude).HasPrecision(9, 6);
        });

        // 7. CancellationPolicies
        modelBuilder.Entity<CancellationPolicy>(entity =>
        {
            entity.ToTable("CancellationPolicies");
            entity.HasKey(e => e.Id);
        });

        // 8. Tours
        modelBuilder.Entity<Tour>(entity =>
        {
            entity.ToTable("Tours");
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Slug).IsUnique();

            entity.Property(e => e.Latitude).HasPrecision(9, 6);
            entity.Property(e => e.Longitude).HasPrecision(9, 6);
            entity.Property(e => e.DurationHours).HasPrecision(4, 1);
            entity.Property(e => e.BasePrice).HasPrecision(18, 2);
            entity.Property(e => e.AverageRating).HasPrecision(3, 2);
            entity.Property(e => e.Status).HasConversion<string>();

            entity.HasOne(e => e.Host)
                  .WithMany(h => h.Tours)
                  .HasForeignKey(e => e.HostId)
                  .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(e => e.Category)
                  .WithMany(c => c.Tours)
                  .HasForeignKey(e => e.CategoryId)
                  .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(e => e.Location)
                  .WithMany(l => l.Tours)
                  .HasForeignKey(e => e.LocationId)
                  .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(e => e.CancellationPolicy)
                  .WithMany(p => p.Tours)
                  .HasForeignKey(e => e.CancellationPolicyId)
                  .OnDelete(DeleteBehavior.NoAction);
        });

        // 9. TourImages
        modelBuilder.Entity<TourImage>(entity =>
        {
            entity.ToTable("TourImages");
            entity.HasKey(e => e.Id);

            entity.HasOne(e => e.Tour)
                  .WithMany(t => t.Images)
                  .HasForeignKey(e => e.TourId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // 10. TourItineraries
        modelBuilder.Entity<TourItinerary>(entity =>
        {
            entity.ToTable("TourItineraries");
            entity.HasKey(e => e.Id);

            entity.HasOne(e => e.Tour)
                  .WithMany(t => t.Itineraries)
                  .HasForeignKey(e => e.TourId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // 11. TourSchedules
        modelBuilder.Entity<TourSchedule>(entity =>
        {
            entity.ToTable("TourSchedules");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Price).HasPrecision(18, 2);
            entity.Property(e => e.Status).HasConversion<string>();
            entity.Property(e => e.RowVersion).IsRowVersion();

            entity.HasOne(e => e.Tour)
                  .WithMany(t => t.Schedules)
                  .HasForeignKey(e => e.TourId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // 12. Bookings
        modelBuilder.Entity<Booking>(entity =>
        {
            entity.ToTable("Bookings");
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.BookingCode).IsUnique();
            entity.HasIndex(e => e.QrCodeToken).IsUnique();

            entity.Property(e => e.TotalAmount).HasPrecision(18, 2);
            entity.Property(e => e.DiscountAmount).HasPrecision(18, 2);
            entity.Property(e => e.FinalAmount).HasPrecision(18, 2);
            entity.Property(e => e.Status).HasConversion<string>();
            entity.Property(e => e.PaymentStatus).HasConversion<string>();

            entity.HasOne(e => e.Customer)
                  .WithMany(u => u.Bookings)
                  .HasForeignKey(e => e.CustomerId)
                  .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(e => e.Schedule)
                  .WithMany(s => s.Bookings)
                  .HasForeignKey(e => e.ScheduleId)
                  .OnDelete(DeleteBehavior.NoAction);
        });

        // 13. BookingParticipants
        modelBuilder.Entity<BookingParticipant>(entity =>
        {
            entity.ToTable("BookingParticipants");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ParticipantType).HasConversion<string>();

            entity.HasOne(e => e.Booking)
                  .WithMany(b => b.Participants)
                  .HasForeignKey(e => e.BookingId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // 14. Payments
        modelBuilder.Entity<Payment>(entity =>
        {
            entity.ToTable("Payments");
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.TransactionCode).IsUnique();
            entity.Property(e => e.Amount).HasPrecision(18, 2);

            entity.HasOne(e => e.Booking)
                  .WithMany(b => b.Payments)
                  .HasForeignKey(e => e.BookingId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // 15. CheckIns
        modelBuilder.Entity<CheckIn>(entity =>
        {
            entity.ToTable("CheckIns");
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.BookingId).IsUnique();

            entity.HasOne(e => e.Booking)
                  .WithOne(b => b.CheckIn)
                  .HasForeignKey<CheckIn>(e => e.BookingId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.CheckedInByHost)
                  .WithMany()
                  .HasForeignKey(e => e.CheckedInByHostId)
                  .OnDelete(DeleteBehavior.NoAction);
        });

        // 16. Reviews
        modelBuilder.Entity<Review>(entity =>
        {
            entity.ToTable("Reviews");
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.BookingId).IsUnique();

            entity.HasOne(e => e.Booking)
                  .WithOne(b => b.Review)
                  .HasForeignKey<Review>(e => e.BookingId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Customer)
                  .WithMany(u => u.Reviews)
                  .HasForeignKey(e => e.CustomerId)
                  .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(e => e.Tour)
                  .WithMany(t => t.Reviews)
                  .HasForeignKey(e => e.TourId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // 17. Favorites
        modelBuilder.Entity<Favorite>(entity =>
        {
            entity.ToTable("Favorites");
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.UserId, e.TourId }).IsUnique();

            entity.HasOne(e => e.User)
                  .WithMany(u => u.Favorites)
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Tour)
                  .WithMany(t => t.Favorites)
                  .HasForeignKey(e => e.TourId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // 18. Notifications
        modelBuilder.Entity<Notification>(entity =>
        {
            entity.ToTable("Notifications");
            entity.HasKey(e => e.Id);

            entity.HasOne(e => e.User)
                  .WithMany(u => u.Notifications)
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
