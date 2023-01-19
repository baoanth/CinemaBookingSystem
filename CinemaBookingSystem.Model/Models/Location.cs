using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaBookingSystem.Model.Models
{
    [Table("Locations")]
    public class Location
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LocationId { get; set; }
        [Required]
        [MaxLength(100)]
        public string Address { get; set; }
        [Required]
        [MaxLength(100)]
        public string City { get; set; }
        [Required]
        [MaxLength(100)]
        public string Region { get; set; }
        public virtual IEnumerable<Cinema> Cinemas { get; set; }
    }
}
