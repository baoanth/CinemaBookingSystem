using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaBookingSystem.Model.Models
{
    [Table("Articles")]
    public class Article
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ArticleId { get; set; }
        [Required]
        [MaxLength(50)]
        public string ArticleTitle { get; set; }
        [MaxLength(500)]
        public string ArticleImage { get; set; }
        [MaxLength(500)]
        public string ArticleVideo { get; set; }
        [Required]
        [MaxLength(5000)]
        public string ArticleContent { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }
        [Required]
        public int CreatedBy { get; set; }
        [ForeignKey("CreatedBy")]
        public virtual User User { get; set; }
    }
}