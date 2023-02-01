using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaBookingSystem.Model.Models
{
    [Table("Movies")]
    public class Movie
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MovieId { get; set; }
        [Required]
        [MaxLength(256)]
        public string MovieName { get; set; }
        [Required]
        [MaxLength(50)]
        public string Director { get; set; }
        [Required]
        [MaxLength(1000)]
        public string Cast { get; set; }
        [Required]
        public DateTime ReleaseDate { get; set; }
        [Required]
        [MaxLength(256)]
        public string Genres { get; set; }
        [Required]
        public int RunningTime { get; set; }
        [MaxLength(256)]
        public string? Rated { get; set; }
        [MaxLength(256)]
        public string? TrailerURL { get; set; }
        [MaxLength(256)]
        public string? ThumpnailImg { get; set; }
        [Required]
        [MaxLength(5000)]
        public string Description { get; set; }
        public IEnumerable<Screening> Screenings { get; set; }
        public IEnumerable<Comment> Comments { get; set; }
    }
}
