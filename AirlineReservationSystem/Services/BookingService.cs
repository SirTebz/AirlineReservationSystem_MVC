using AirlineReservationSystem.Data;
using AirlineReservationSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace AirlineReservationSystem.Services
{
    public class BookingService : IBookingService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<BookingService> _logger;
        
        public BookingService(ApplicationDbContext context, ILogger<BookingService> logger)
        {
            _context = context;
            _logger = logger;
        }
        
        public async Task<Booking?> CreateBookingAsync(int flightId, int seatId, string userId, 
            string passengerName, string passengerEmail, string passengerPhone)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Check if seat is available
                var seat = await _context.Seats
                    .FirstOrDefaultAsync(s => s.SeatId == seatId && s.FlightId == flightId);
                
                if (seat == null || !seat.IsAvailable)
                {
                    _logger.LogWarning("Seat {SeatId} is not available", seatId);
                    return null;
                }
                
                // Get flight details
                var flight = await _context.Flights.FindAsync(flightId);
                if (flight == null)
                {
                    _logger.LogWarning("Flight {FlightId} not found", flightId);
                    return null;
                }
                
                // Mark seat as unavailable
                seat.IsAvailable = false;
                
                // Create booking
                var booking = new Booking
                {
                    UserId = userId,
                    FlightId = flightId,
                    SeatId = seatId,
                    BookingDate = DateTime.Now,
                    Status = "Confirmed",
                    BookingReference = GenerateBookingReference(),
                    TotalPrice = flight.Price,
                    PassengerName = passengerName,
                    PassengerEmail = passengerEmail,
                    PassengerPhone = passengerPhone
                };
                
                _context.Bookings.Add(booking);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                
                // Load navigation properties
                await _context.Entry(booking)
                    .Reference(b => b.Flight)
                    .LoadAsync();
                await _context.Entry(booking)
                    .Reference(b => b.Seat)
                    .LoadAsync();
                
                return booking;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error creating booking");
                return null;
            }
        }
        
        public async Task<Booking?> GetBookingByIdAsync(int bookingId)
        {
            try
            {
                return await _context.Bookings
                    .Include(b => b.Flight)
                    .Include(b => b.Seat)
                    .Include(b => b.User)
                    .FirstOrDefaultAsync(b => b.BookingId == bookingId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting booking by ID");
                return null;
            }
        }
        
        public async Task<Booking?> GetBookingByReferenceAsync(string bookingReference)
        {
            try
            {
                return await _context.Bookings
                    .Include(b => b.Flight)
                    .Include(b => b.Seat)
                    .Include(b => b.User)
                    .FirstOrDefaultAsync(b => b.BookingReference == bookingReference);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting booking by reference");
                return null;
            }
        }
        
        public async Task<List<Booking>> GetUserBookingsAsync(string userId)
        {
            try
            {
                return await _context.Bookings
                    .Include(b => b.Flight)
                    .Include(b => b.Seat)
                    .Where(b => b.UserId == userId)
                    .OrderByDescending(b => b.BookingDate)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user bookings");
                return new List<Booking>();
            }
        }
        
        public async Task<List<Booking>> GetAllBookingsAsync()
        {
            try
            {
                return await _context.Bookings
                    .Include(b => b.Flight)
                    .Include(b => b.Seat)
                    .Include(b => b.User)
                    .OrderByDescending(b => b.BookingDate)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all bookings");
                return new List<Booking>();
            }
        }
        
        public async Task<bool> CancelBookingAsync(int bookingId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var booking = await _context.Bookings
                    .Include(b => b.Seat)
                    .FirstOrDefaultAsync(b => b.BookingId == bookingId);
                
                if (booking == null)
                    return false;
                
                // Update booking status
                booking.Status = "Cancelled";
                
                // Make seat available again
                if (booking.Seat != null)
                {
                    booking.Seat.IsAvailable = true;
                }
                
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error cancelling booking");
                return false;
            }
        }
        
        public async Task<bool> UpdateBookingStatusAsync(int bookingId, string status)
        {
            try
            {
                var booking = await _context.Bookings.FindAsync(bookingId);
                if (booking == null)
                    return false;
                
                booking.Status = status;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating booking status");
                return false;
            }
        }
        
        public string GenerateBookingReference()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 8)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
