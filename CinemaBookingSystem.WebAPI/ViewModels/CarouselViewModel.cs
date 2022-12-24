namespace CinemaBookingSystem.WebAPI.ViewModels
{
    public class CarouselViewModel
    {
        public int CarouselID { get; set; }
        public string CarouselName { get; set; }
        public string? CarouselDescription { get; set; }
        public string? ImageURL { get; set; }
        public int? DisplayOrder { get; set; }
    }
}