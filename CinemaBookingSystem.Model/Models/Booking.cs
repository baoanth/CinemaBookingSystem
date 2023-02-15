using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaBookingSystem.Model.Models
{
    [Table("Bookings")]
    public class Booking
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BookingId { get; set; }
        [Required]
        public int PaymentId { get; set; }
        [Required]
        public DateTime BookedAt { get; set; }
        [Required]
        public bool IsPayed { get; set; }
        [ForeignKey("PaymentId")]
        public virtual Payment Payment { get; set; }
        [Required]
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        [MaxLength(100)]
        public string? VerifyCode { get; set; }
    }
}
