using AirlineReservationSystem.Models;
using AirlineReservationSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AirlineReservationSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IFlightService _flightService;
        private readonly IBookingService _bookingService;
        private readonly ILogger<AdminController> _logger;

        public AdminController(
            IFlightService flightService,
            IBookingService bookingService,
            ILogger<AdminController> logger)
        {
            _flightService = flightService;
            _bookingService = bookingService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        // Flight Management
        public async Task<IActionResult> Flights()
        {
            var flights = await _flightService.GetAllFlightsAsync();
            return View(flights);
        }

        [HttpGet]
        public IActionResult CreateFlight()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateFlight(Flight flight)
        {
            if (ModelState.IsValid)
            {
                var result = await _flightService.CreateFlightAsync(flight);
                
                if (result)
                {
                    TempData["SuccessMessage"] = "Flight created successfully.";
                    return RedirectToAction(nameof(Flights));
                }
                else
                {
                    ModelState.AddModelError("", "Error creating flight.");
                }
            }

            return View(flight);
        }

        [HttpGet]
        public async Task<IActionResult> EditFlight(int id)
        {
            var flight = await _flightService.GetFlightByIdAsync(id);
            
            if (flight == null)
            {
                TempData["ErrorMessage"] = "Flight not found.";
                return RedirectToAction(nameof(Flights));
            }

            return View(flight);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditFlight(Flight flight)
        {
            if (ModelState.IsValid)
            {
                var result = await _flightService.UpdateFlightAsync(flight);
                
                if (result)
                {
                    TempData["SuccessMessage"] = "Flight updated successfully.";
                    return RedirectToAction(nameof(Flights));
                }
                else
                {
                    ModelState.AddModelError("", "Error updating flight.");
                }
            }

            return View(flight);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteFlight(int id)
        {
            var result = await _flightService.DeleteFlightAsync(id);
            
            if (result)
            {
                TempData["SuccessMessage"] = "Flight deleted successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Error deleting flight.";
            }

            return RedirectToAction(nameof(Flights));
        }

        public async Task<IActionResult> FlightDetails(int id)
        {
            var flight = await _flightService.GetFlightByIdAsync(id);
            
            if (flight == null)
            {
                TempData["ErrorMessage"] = "Flight not found.";
                return RedirectToAction(nameof(Flights));
            }

            return View(flight);
        }

        // Booking Management
        public async Task<IActionResult> Bookings()
        {
            var bookings = await _bookingService.GetAllBookingsAsync();
            return View(bookings);
        }

        public async Task<IActionResult> BookingDetails(int id)
        {
            var booking = await _bookingService.GetBookingByIdAsync(id);
            
            if (booking == null)
            {
                TempData["ErrorMessage"] = "Booking not found.";
                return RedirectToAction(nameof(Bookings));
            }

            return View(booking);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateBookingStatus(int id, string status)
        {
            var result = await _bookingService.UpdateBookingStatusAsync(id, status);
            
            if (result)
            {
                TempData["SuccessMessage"] = "Booking status updated successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Error updating booking status.";
            }

            return RedirectToAction(nameof(BookingDetails), new { id });
        }

        // Dashboard with statistics
        public async Task<IActionResult> Dashboard()
        {
            var allFlights = await _flightService.GetAllFlightsAsync();
            var allBookings = await _bookingService.GetAllBookingsAsync();

            ViewBag.TotalFlights = allFlights.Count;
            ViewBag.ActiveFlights = allFlights.Count(f => f.IsActive);
            ViewBag.TotalBookings = allBookings.Count;
            ViewBag.ConfirmedBookings = allBookings.Count(b => b.Status == "Confirmed");
            ViewBag.CancelledBookings = allBookings.Count(b => b.Status == "Cancelled");
            ViewBag.TotalRevenue = allBookings
                .Where(b => b.Status == "Confirmed")
                .Sum(b => b.TotalPrice);

            return View();
        }
    }
}
