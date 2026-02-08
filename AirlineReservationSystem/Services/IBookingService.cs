using AirlineReservationSystem.Models;

namespace AirlineReservationSystem.Services
{
    public interface IBookingService
    {
        Task<Booking?> CreateBookingAsync(int flightId, int seatId, string userId, 
            string passengerName, string passengerEmail, string passengerPhone);
        Task<Booking?> GetBookingByIdAsync(int bookingId);
        Task<Booking?> GetBookingByReferenceAsync(string bookingReference);
        Task<List<Booking>> GetUserBookingsAsync(string userId);
        Task<List<Booking>> GetAllBookingsAsync();
        Task<bool> CancelBookingAsync(int bookingId);
        Task<bool> UpdateBookingStatusAsync(int bookingId, string status);
        string GenerateBookingReference();
    }
}
