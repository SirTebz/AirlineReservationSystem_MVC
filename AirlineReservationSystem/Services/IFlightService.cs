using AirlineReservationSystem.Models;

namespace AirlineReservationSystem.Services
{
    public interface IFlightService
    {
        Task<List<Flight>> SearchFlightsAsync(string origin, string destination, DateTime departureDate);
        Task<Flight?> GetFlightByIdAsync(int flightId);
        Task<List<Flight>> GetAllFlightsAsync();
        Task<List<Flight>> GetActiveFlightsAsync();
        Task<bool> CreateFlightAsync(Flight flight);
        Task<bool> UpdateFlightAsync(Flight flight);
        Task<bool> DeleteFlightAsync(int flightId);
        Task<List<Seat>> GetAvailableSeatsAsync(int flightId);
        Task<bool> IsSeatAvailableAsync(int seatId);
    }
}
