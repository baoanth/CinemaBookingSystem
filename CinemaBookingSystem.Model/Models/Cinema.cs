using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaBookingSystem.Model.Models
{
    [Table("Cinemas")]
    public class Cinema
    {
        [Key]
        public int CinemaID { get; set; }
        [Required]
        public string CinemaName { get; set; }
        [Required]
        public string Location { get; set; }
        [Required]
        public string FAX { get; set; }
        [Required]
        [StringLength(10)]
        public string Hotline { get; set; }
        [Required]
        public int ProvinceID { get; set; }
        [ForeignKey("ProvinceID")]
        public virtual Province Province { get; set; }
        public IEnumerable<Theatre> Theatres { get; set; }

    }
}
