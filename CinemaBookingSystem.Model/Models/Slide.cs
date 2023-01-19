using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CinemaBookingSystem.Model.Models
{
    [Table("Slides")]
    public class Slide
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SlideId { get; set; }
        [Required]
        [MaxLength(256)]
        public string Name { get; set; }
        [MaxLength(256)]
        public string? Description { get; set; }
        [MaxLength(256)]
        public string? Url { get; set; }
        [MaxLength(256)]
        public string? Image { get; set; }
        public int? SortOrder { get; set; }
    }
}
