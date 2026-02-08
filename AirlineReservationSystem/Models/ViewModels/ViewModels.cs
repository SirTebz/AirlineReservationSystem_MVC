using System.ComponentModel.DataAnnotations;

namespace AirlineReservationSystem.Models.ViewModels
{
    public class FlightSearchViewModel
    {
        [Required(ErrorMessage = "Origin is required")]
        public string Origin { get; set; } = string.Empty;

        [Required(ErrorMessage = "Destination is required")]
        public string Destination { get; set; } = string.Empty;

        [Required(ErrorMessage = "Departure date is required")]
        [DataType(DataType.Date)]
        public DateTime DepartureDate { get; set; }

        public int? Passengers { get; set; } = 1;

        public List<Flight>? SearchResults { get; set; }
    }

    public class BookingViewModel
    {
        public int FlightId { get; set; }
        public int SeatId { get; set; }

        [Required(ErrorMessage = "Passenger name is required")]
        [StringLength(100)]
        public string PassengerName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string PassengerEmail { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid phone number")]
        public string PassengerPhone { get; set; } = string.Empty;

        public Flight? Flight { get; set; }
        public Seat? Seat { get; set; }
        public decimal TotalPrice { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = string.Empty;

        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [Display(Name = "Passport Number")]
        public string? PassportNumber { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
