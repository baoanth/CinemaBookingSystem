using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaBookingSystem.Model.Models
{
    [Table("Provinces")]
    public class Province
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProvinceID { get; set; }
        [Required]
        [MaxLength(100)]
        public string ProvinceName { get; set; }
        [Required]
        [MaxLength(100)]
        public string Region { get; set; }
        public virtual IEnumerable<Cinema> Cinemas { get; set; }
    }
}
