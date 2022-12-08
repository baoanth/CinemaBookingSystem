using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaBookingSystem.Model.Models
{
    [Table("TicketBookings")]
    public class TicketBooking
    {
        [Key]
        [Column(Order = 1)]
        public int BookingID { get; set; }
        [Key]
        [Column(Order = 2)]
        public int TicketID { get; set; }

        [ForeignKey("BookingID")]
        public virtual Booking Booking { get; set; }
        [ForeignKey("TicketID")]
        public virtual Ticket Ticket { get; set; }
    }
}
