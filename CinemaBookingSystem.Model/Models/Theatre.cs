using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaBookingSystem.Model.Models
{
    [Table("Theatres")]
    public class Theatre
    {
        [Key]
        public int TheatreID { get; set; }
        [Required]
        public string TheatreName { get; set; }
        [Required]
        public int Capacity { get; set; }
        [Required]
        public int CinemaID { get; set; }
        [ForeignKey("CinemaID")]
        public virtual Cinema Cinema { get; set; }
        public IEnumerable<Seat> Seats { get; set; }
        public IEnumerable<Screening> Screenings { get; set; }
    }
}
