using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaBookingSystem.Model.Models
{
    [Table("Comments")]
    public class Comment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CommentID { get; set; }
        [Required]
        [MaxLength(500)]
        public string Content { get; set; }
        [Required]
        public DateTime CommentedAt { get; set; }
        [Required]
        public int CommentedBy { get; set; }
        [ForeignKey("CommentBy")]
        public virtual User CommentedUser { get; set; }
        public int? StarRated { get; set; }
    }
}
