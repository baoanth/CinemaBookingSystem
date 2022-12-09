using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaBookingSystem.Model.Models
{
    [Table("ScreeningPositions")]
    public class ScreeningPosition
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PositionID { get; set; }
        [Required]
        public string Row { get; set; }
        [Required]
        public string Column { get; set; }
        [Required]
        public bool IsBooked { get; set; }
        [Required]
        public int ScreeningID { get; set; }
        [ForeignKey("ScreeningID")]
        public virtual Screening Screening { get; set; }

    }
}
