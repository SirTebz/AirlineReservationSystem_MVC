using AirlineReservationSystem.Data;
using AirlineReservationSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace AirlineReservationSystem.Services
{
    public class FlightService : IFlightService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<FlightService> _logger;
        
        public FlightService(ApplicationDbContext context, ILogger<FlightService> logger)
        {
            _context = context;
            _logger = logger;
        }
        
        public async Task<List<Flight>> SearchFlightsAsync(string origin, string destination, DateTime departureDate)
        {
            try
            {
                var startDate = departureDate.Date;
                var endDate = startDate.AddDays(1);
                
                var flights = await _context.Flights
                    .Include(f => f.Seats)
                    .Where(f => f.Origin.ToLower().Contains(origin.ToLower()) &&
                                f.Destination.ToLower().Contains(destination.ToLower()) &&
                                f.DepartureTime >= startDate &&
                                f.DepartureTime < endDate &&
                                f.IsActive)
                    .OrderBy(f => f.DepartureTime)
                    .ToListAsync();
                
                return flights;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching flights");
                return new List<Flight>();
            }
        }
        
        public async Task<Flight?> GetFlightByIdAsync(int flightId)
        {
            try
            {
                return await _context.Flights
                    .Include(f => f.Seats)
                    .Include(f => f.Bookings)
                    .FirstOrDefaultAsync(f => f.FlightId == flightId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting flight by ID");
                return null;
            }
        }
        
        public async Task<List<Flight>> GetAllFlightsAsync()
        {
            try
            {
                return await _context.Flights
                    .Include(f => f.Seats)
                    .OrderBy(f => f.DepartureTime)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all flights");
                return new List<Flight>();
            }
        }
        
        public async Task<List<Flight>> GetActiveFlightsAsync()
        {
            try
            {
                return await _context.Flights
                    .Include(f => f.Seats)
                    .Where(f => f.IsActive && f.DepartureTime > DateTime.Now)
                    .OrderBy(f => f.DepartureTime)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting active flights");
                return new List<Flight>();
            }
        }
        
        public async Task<bool> CreateFlightAsync(Flight flight)
        {
            try
            {
                _context.Flights.Add(flight);
                await _context.SaveChangesAsync();
                
                // Create seats for the flight
                await CreateSeatsForFlightAsync(flight);
                
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating flight");
                return false;
            }
        }
        
        public async Task<bool> UpdateFlightAsync(Flight flight)
        {
            try
            {
                _context.Flights.Update(flight);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating flight");
                return false;
            }
        }
        
        public async Task<bool> DeleteFlightAsync(int flightId)
        {
            try
            {
                var flight = await _context.Flights.FindAsync(flightId);
                if (flight == null)
                    return false;
                
                // Soft delete
                flight.IsActive = false;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting flight");
                return false;
            }
        }
        
        public async Task<List<Seat>> GetAvailableSeatsAsync(int flightId)
        {
            try
            {
                return await _context.Seats
                    .Where(s => s.FlightId == flightId && s.IsAvailable)
                    .OrderBy(s => s.SeatClass)
                    .ThenBy(s => s.SeatNumber)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting available seats");
                return new List<Seat>();
            }
        }
        
        public async Task<bool> IsSeatAvailableAsync(int seatId)
        {
            try
            {
                var seat = await _context.Seats.FindAsync(seatId);
                return seat?.IsAvailable ?? false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking seat availability");
                return false;
            }
        }
        
        private async Task CreateSeatsForFlightAsync(Flight flight)
        {
            var seats = new List<Seat>();
            
            // First Class (rows 1-3, seats A-F)
            for (int row = 1; row <= 3; row++)
            {
                foreach (char col in new[] { 'A', 'B', 'C', 'D', 'E', 'F' })
                {
                    seats.Add(new Seat
                    {
                        FlightId = flight.FlightId,
                        SeatNumber = $"{row}{col}",
                        SeatClass = "First",
                        IsAvailable = true,
                        IsWindowSeat = col == 'A' || col == 'F',
                        IsAisleSeat = col == 'C' || col == 'D'
                    });
                }
            }
            
            // Business Class (rows 4-10, seats A-F)
            for (int row = 4; row <= 10; row++)
            {
                foreach (char col in new[] { 'A', 'B', 'C', 'D', 'E', 'F' })
                {
                    seats.Add(new Seat
                    {
                        FlightId = flight.FlightId,
                        SeatNumber = $"{row}{col}",
                        SeatClass = "Business",
                        IsAvailable = true,
                        IsWindowSeat = col == 'A' || col == 'F',
                        IsAisleSeat = col == 'C' || col == 'D'
                    });
                }
            }
            
            // Economy Class
            int economyRows = (flight.TotalSeats - seats.Count) / 6;
            for (int row = 11; row <= 10 + economyRows; row++)
            {
                foreach (char col in new[] { 'A', 'B', 'C', 'D', 'E', 'F' })
                {
                    seats.Add(new Seat
                    {
                        FlightId = flight.FlightId,
                        SeatNumber = $"{row}{col}",
                        SeatClass = "Economy",
                        IsAvailable = true,
                        IsWindowSeat = col == 'A' || col == 'F',
                        IsAisleSeat = col == 'C' || col == 'D'
                    });
                }
            }
            
            _context.Seats.AddRange(seats.Take(flight.TotalSeats));
            await _context.SaveChangesAsync();
        }
    }
}
