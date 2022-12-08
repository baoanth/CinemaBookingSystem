using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaBookingSystem.Model.Models
{
    [Table("Tickets")]
    public class Ticket
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TicketID { get; set; }
        [Required]
        public int TicketPrice { get; set; }
        [Required]
        public bool IsBooked { get; set; }
        [Required]
        public int SeatID { get; set; }
        [Required]
        public int ScreeningID { get; set; }
        [ForeignKey("SeatID")]
        public virtual Seat Seat { get; set; }
        [ForeignKey("ScreeningID")]
        public virtual Screening Screening { get; set; }

    }
}
