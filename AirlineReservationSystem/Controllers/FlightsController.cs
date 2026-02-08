using AirlineReservationSystem.Models.ViewModels;
using AirlineReservationSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace AirlineReservationSystem.Controllers
{
    public class FlightsController : Controller
    {
        private readonly IFlightService _flightService;
        private readonly ILogger<FlightsController> _logger;

        public FlightsController(IFlightService flightService, ILogger<FlightsController> logger)
        {
            _flightService = flightService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var model = new FlightSearchViewModel
            {
                DepartureDate = DateTime.Today.AddDays(1)
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Search(FlightSearchViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.SearchResults = await _flightService.SearchFlightsAsync(
                    model.Origin,
                    model.Destination,
                    model.DepartureDate);

                if (model.SearchResults.Count == 0)
                {
                    TempData["InfoMessage"] = "No flights found for your search criteria. Please try different dates or destinations.";
                }
                else
                {
                    TempData["SuccessMessage"] = $"Found {model.SearchResults.Count} flight(s) matching your search.";
                }
            }

            return View("Index", model);
        }

        public async Task<IActionResult> Details(int id)
        {
            var flight = await _flightService.GetFlightByIdAsync(id);

            if (flight == null)
            {
                TempData["ErrorMessage"] = "Flight not found.";
                return RedirectToAction(nameof(Index));
            }

            var availableSeats = await _flightService.GetAvailableSeatsAsync(id);
            ViewBag.AvailableSeats = availableSeats;

            return View(flight);
        }

        public async Task<IActionResult> Browse()
        {
            var flights = await _flightService.GetActiveFlightsAsync();
            return View(flights);
        }

        [HttpGet]
        public async Task<IActionResult> GetAvailableSeats(int flightId)
        {
            var seats = await _flightService.GetAvailableSeatsAsync(flightId);
            return Json(seats);
        }
    }
}
