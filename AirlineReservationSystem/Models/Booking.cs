using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirlineReservationSystem.Models
{
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        public int FlightId { get; set; }

        [Required]
        public int SeatId { get; set; }

        [Required]
        public DateTime BookingDate { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Confirmed"; // Confirmed, Cancelled, Pending

        [Required]
        [StringLength(20)]
        public string BookingReference { get; set; } = string.Empty;

        [Required]
        public decimal TotalPrice { get; set; }

        [StringLength(100)]
        public string PassengerName { get; set; } = string.Empty;

        [StringLength(50)]
        public string PassengerEmail { get; set; } = string.Empty;

        [StringLength(20)]
        public string PassengerPhone { get; set; } = string.Empty;

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual ApplicationUser? User { get; set; }

        [ForeignKey("FlightId")]
        public virtual Flight? Flight { get; set; }

        [ForeignKey("SeatId")]
        public virtual Seat? Seat { get; set; }
    }
}
