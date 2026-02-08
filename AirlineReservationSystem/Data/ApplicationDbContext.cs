using AirlineReservationSystem.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AirlineReservationSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Flight> Flights { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure Flight
            builder.Entity<Flight>()
                .HasMany(f => f.Seats)
                .WithOne(s => s.Flight)
                .HasForeignKey(s => s.FlightId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Flight>()
                .HasMany(f => f.Bookings)
                .WithOne(b => b.Flight)
                .HasForeignKey(b => b.FlightId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure Seat
            builder.Entity<Seat>()
                .HasMany(s => s.Bookings)
                .WithOne(b => b.Seat)
                .HasForeignKey(b => b.SeatId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure Booking
            builder.Entity<Booking>()
                .HasOne(b => b.User)
                .WithMany(u => u.Bookings)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            builder.Entity<Flight>()
                .HasIndex(f => f.FlightNumber);

            builder.Entity<Flight>()
                .HasIndex(f => new { f.Origin, f.Destination, f.DepartureTime });

            builder.Entity<Booking>()
                .HasIndex(b => b.BookingReference)
                .IsUnique();

            builder.Entity<Seat>()
                .HasIndex(s => new { s.FlightId, s.SeatNumber })
                .IsUnique();
        }
    }
}
