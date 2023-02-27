using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaBookingSystem.Model.Models
{
    [Table("CustomerContacts")]
    public class CustomerContact
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(500)]
        public string CustomerName { get; set; }

        [Required]
        [MaxLength(500)]
        public string CustomerEmail { get; set; }

        [Required]
        [MaxLength(500)]
        public string CustomerPhone { get; set; }

        [Required]
        [MaxLength(5000)]
        public string Content { get; set; }
        
        public DateTime SendedAt { get; set; }

        public bool Status { get; set; }
    }
}