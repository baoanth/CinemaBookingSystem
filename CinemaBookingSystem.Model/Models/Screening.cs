using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaBookingSystem.Model.Models
{
    [Table("Screenings")]
    public class Screening
    {   
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ScreeningId { get; set; }
        [Required]
        public DateTime ShowTime { get; set; }
        [Required]
        public bool ShowStatus { get; set; }
        [Required]
        public int TheatreId { get; set; }
        [Required]
        public int MovieId { get; set; }
        [ForeignKey("TheatreId")]
        public virtual Theatre Theatre { get; set; }
        [ForeignKey("MovieId")]
        public virtual Movie Movie { get; set; }
        public IEnumerable<ScreeningPosition> ScreeningPositions { get; set; }
    }
}
