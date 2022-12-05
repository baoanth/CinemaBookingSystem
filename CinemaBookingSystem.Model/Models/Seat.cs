using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaBookingSystem.Model.Models
{
    [Table("Seats")]
    public class Seat
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SeatID { get; set; }
        [Required]
        [MaxLength(5)]
        public string Row { get; set; }
        [Required]
        public int Column { get; set; }
        [Required]
        public int TheatreID { get; set; }
        [ForeignKey("TheatreID")]
        public virtual Theatre Theatre { get; set; }

    }
}
