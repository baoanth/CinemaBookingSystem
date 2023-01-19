using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CinemaBookingSystem.Model.Models
{
    [Table("Contacts")]
    public class Contact
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ContactId { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required]
        [MaxLength(50)]
        public string Department { get; set; }
        [MaxLength(50)]
        public string Mobile { get; set; }
        [MaxLength(50)]
        public string FacebookURL { get; set; }
        [MaxLength(50)]
        public string Email { get; set; }
        [MaxLength(200)]
        public string Message { get; set; }
        public bool IsWorking { get; set; }
    }
}
