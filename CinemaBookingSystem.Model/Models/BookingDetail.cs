using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaBookingSystem.Model.Models
{
    [Table("BookingDetails")]
    public class BookingDetail
    {
        [Key]
        [Column(Order = 1)]
        public int BookingId { get; set; }
        [Key]
        [Column(Order = 2)]
        public int PositionId { get; set; }

        [ForeignKey("BookingId")]
        public virtual Booking Booking { get; set; }
        [ForeignKey("PositionId")]
        public virtual ScreeningPosition ScreeningPosition { get; set; }
    }
}
