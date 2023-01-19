using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaBookingSystem.Model.Models
{
    [Table("Cinemas")]
    public class Cinema
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CinemaID { get; set; }
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
        public int LocationID { get; set; }
        [ForeignKey("LocationID")]
        public virtual Location Location { get; set; }
        public IEnumerable<Theatre> Theatres { get; set; }

    }
}
