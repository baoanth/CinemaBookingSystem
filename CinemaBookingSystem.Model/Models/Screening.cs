using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaBookingSystem.Model.Models
{
    [Table("Screening")]
    public class Screening
    {   
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ScreeningID { get; set; }
        [Required]
        public DateOnly ShowDate { get; set; }
        [Required]
        public DateTime ShowTime { get; set; }
        [Required]
        public bool ShowStatus { get; set; }
        [Required]
        public bool IsFull { get; set; }
        [Required]
        public int EmptySeats { get; set; }
        [Required]
        public int TheatreID { get; set; }
        [Required]
        public int MovieID { get; set; }
        [ForeignKey("TheatreID")]
        public virtual Theatre Theatre { get; set; }
        [ForeignKey("MovieID")]
        public virtual Movie Movie { get; set; }
        public IEnumerable<Ticket> Tickets { get; set; }
    }
}
