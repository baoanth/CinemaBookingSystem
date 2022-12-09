using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaBookingSystem.Model.Models
{
    [Table("BookingDetails")]
    public class BookingDetail
    {
        [Key]
        [Column(Order = 1)]
        public int BookingID { get; set; }
        [Key]
        [Column(Order = 2)]
        public int PositionID { get; set; }

        [ForeignKey("BookingID")]
        public virtual Booking Booking { get; set; }
        [ForeignKey("PositionID")]
        public virtual ScreeningPosition ScreeningPosition { get; set; }
    }
}
