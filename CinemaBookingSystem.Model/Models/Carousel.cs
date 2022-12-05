using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CinemaBookingSystem.Model.Models
{
    [Table("Carousels")]
    public class Carousel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CarouselID { get; set; }
        [Required]
        [MaxLength(256)]
        public string CarouselName { get; set; }
        [MaxLength(256)]
        public string? CarouselDescription { get; set; }
        [MaxLength(256)]
        public string? ImageURL { get; set; }
        public int? DisplayOrder { get; set; }
    }
}
