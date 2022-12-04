using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaBookingSystem.Model.Models
{
    [Table("Movies")]
    public class Movie
    {
        [Key]
        public int MovieID { get; set; }
        [Required]
        public int MovieName { get; set; }
        [Required]
        public int Director { get; set; }
        [Required]
        public int Cast { get; set; }
        [Required]
        public DateOnly ReleaseDate { get; set; }
        [Required]
        public string Genres { get; set; }
        [Required]
        public int RunningTime { get; set; }
        public string? Rated { get; set; }
        [Required]
        [MaxLength]
        public string Description { get; set; }
        public IEnumerable<Screening> Screenings { get; set; }
    }
}
