using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaBookingSystem.Model.Models
{
    [Table("ScreeningPositions")]
    public class ScreeningPosition
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PositionId { get; set; }
        [Required]
        public string Row { get; set; }
        [Required]
        public string Column { get; set; }
        [Required]
        public int Price { get; set; }
        [Required]
        public bool IsBooked { get; set; }
        [Required]
        public int ScreeningId { get; set; }
        [ForeignKey("ScreeningId")]
        public virtual Screening Screening { get; set; }

    }
}
