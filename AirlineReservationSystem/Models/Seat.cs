using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirlineReservationSystem.Models
{
    public class Seat
    {
        [Key]
        public int SeatId { get; set; }

        [Required]
        public int FlightId { get; set; }

        [Required]
        [StringLength(10)]
        public string SeatNumber { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string SeatClass { get; set; } = "Economy"; // Economy, Business, First

        public bool IsAvailable { get; set; } = true;

        public bool IsWindowSeat { get; set; }

        public bool IsAisleSeat { get; set; }

        // Navigation property
        [ForeignKey("FlightId")]
        public virtual Flight? Flight { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
