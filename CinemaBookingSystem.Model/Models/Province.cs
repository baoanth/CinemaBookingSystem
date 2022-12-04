using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaBookingSystem.Model.Models
{
    [Table("Provinces")]
    public class Province
    {
        [Key]
        public int ProvinceID { get; set; }
        [Required]
        public string ProvinceName { get; set; }
        public virtual IEnumerable<Cinema> Cinemas { get; set; }
    }
}
