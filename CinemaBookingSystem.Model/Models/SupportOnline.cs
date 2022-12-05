using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CinemaBookingSystem.Model.Models
{
    [Table("SupportOnlines")]
    public class SupportOnline
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SupportID { get; set; }
        [Required]
        [MaxLength(50)]
        public string SupportName { get; set; }
        [Required]
        [MaxLength(50)]
        public string Department { get; set; }
        [MaxLength(50)]
        public string Mobile { get; set; }
        [MaxLength(50)]
        public string FacebookURL { get; set; }
        [MaxLength(50)]
        public string SupportEmail { get; set; }
        public bool IsWorking { get; set; }
    }
}
