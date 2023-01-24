using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaBookingSystem.Model.Models
{
    [Table("Cinemas")]
    public class Cinema
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CinemaId { get; set; }
        [Required]
        [MaxLength(256)]
        public string CinemaName { get; set; }
        [Required]
        [MaxLength(50)]
        public string FAX { get; set; }
        [Required]
        [MaxLength(50)]
        public string Hotline { get; set; }
        [Required]
        [MaxLength(100)]
        public string Address { get; set; }
        [Required]
        [MaxLength(100)]
        public string City { get; set; }
        [Required]
        [MaxLength(100)]
        public string Region { get; set; }
        public IEnumerable<Theatre> Theatres { get; set; }

    }
}
