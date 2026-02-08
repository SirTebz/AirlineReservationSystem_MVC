using AirlineReservationSystem.Models;
using AirlineReservationSystem.Models.ViewModels;
using AirlineReservationSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AirlineReservationSystem.Controllers
{
    [Authorize]
    public class BookingsController : Controller
    {
        private readonly IBookingService _bookingService;
        private readonly IFlightService _flightService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<BookingsController> _logger;

        public BookingsController(IBookingService bookingService, IFlightService flightService, UserManager<ApplicationUser> userManager, ILogger<BookingsController> logger)
        {
            _bookingService = bookingService;
            _flightService = flightService;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Challenge();
            }

            var bookings = await _bookingService.GetUserBookingsAsync(user.Id);
            return View(bookings);
        }

        [HttpGet]
        public async Task<IActionResult> Create(int flightId, int seatId)
        {
            var flight = await _flightService.GetFlightByIdAsync(flightId);
            if (flight == null)
            {
                TempData["ErrorMessage"] = "Flight not found.";
                return RedirectToAction("Index", "Flights");
            }

            var seat = flight.Seats.FirstOrDefault(s => s.SeatId == seatId);
            if (seat == null || !seat.IsAvailable)
            {
                TempData["ErrorMessage"] = "Selected seat is not available.";
                return RedirectToAction("Details", "Flights", new { id = flightId });
            }

            var user = await _userManager.GetUserAsync(User);
            var model = new BookingViewModel
            {
                FlightId = flightId,
                SeatId = seatId,
                Flight = flight,
                Seat = seat,
                TotalPrice = flight.Price,
                PassengerName = $"{user?.FirstName} {user?.LastName}",
                PassengerEmail = user?.Email ?? string.Empty,
                PassengerPhone = user?.PhoneNumber ?? string.Empty
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookingViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var flight = await _flightService.GetFlightByIdAsync(model.FlightId);
                var seat = flight?.Seats.FirstOrDefault(s => s.SeatId == model.SeatId);
                model.Flight = flight;
                model.Seat = seat;
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Challenge();
            }

            var booking = await _bookingService.CreateBookingAsync(
                model.FlightId,
                model.SeatId,
                user.Id,
                model.PassengerName,
                model.PassengerEmail,
                model.PassengerPhone);

            if (booking == null)
            {
                TempData["ErrorMessage"] = "Unable to complete booking. The selected seat may no longer be available.";
                return RedirectToAction("Details", "Flights", new { id = model.FlightId });
            }

            TempData["SuccessMessage"] = $"Booking confirmed! Your reference number is: {booking.BookingReference}";
            return RedirectToAction(nameof(Confirmation), new { id = booking.BookingId });
        }

        public async Task<IActionResult> Details(int id)
        {
            var booking = await _bookingService.GetBookingByIdAsync(id);

            if (booking == null)
            {
                TempData["ErrorMessage"] = "Booking not found.";
                return RedirectToAction(nameof(Index));
            }

            var user = await _userManager.GetUserAsync(User);
            if (booking.UserId != user?.Id && !User.IsInRole("Admin"))
            {
                TempData["ErrorMessage"] = "Unauthorized access.";
                return RedirectToAction(nameof(Index));
            }

            return View(booking);
        }

        public async Task<IActionResult> Confirmation(int id)
        {
            var booking = await _bookingService.GetBookingByIdAsync(id);

            if (booking == null)
            {
                TempData["ErrorMessage"] = "Booking not found.";
                return RedirectToAction(nameof(Index));
            }

            var user = await _userManager.GetUserAsync(User);
            if (booking.UserId != user?.Id && !User.IsInRole("Admin"))
            {
                TempData["ErrorMessage"] = "Unauthorized access.";
                return RedirectToAction(nameof(Index));
            }

            return View(booking);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            var booking = await _bookingService.GetBookingByIdAsync(id);

            if (booking == null)
            {
                TempData["ErrorMessage"] = "Booking not found.";
                return RedirectToAction(nameof(Index));
            }

            var user = await _userManager.GetUserAsync(User);
            if (booking.UserId != user?.Id && !User.IsInRole("Admin"))
            {
                TempData["ErrorMessage"] = "Unauthorized access.";
                return RedirectToAction(nameof(Index));
            }

            var result = await _bookingService.CancelBookingAsync(id);

            if (result)
            {
                TempData["SuccessMessage"] = "Booking cancelled successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Unable to cancel booking.";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> SearchByReference(string? reference)
        {
            if (string.IsNullOrWhiteSpace(reference))
            {
                return View();
            }

            var booking = await _bookingService.GetBookingByReferenceAsync(reference);

            if (booking == null)
            {
                TempData["ErrorMessage"] = "No booking found with this reference number.";
                return View();
            }

            return RedirectToAction(nameof(Details), new { id = booking.BookingId });
        }
    }
}
