namespace CinemaBookingSystem.WebAPI.ViewModels
{
    public class SlideViewModel
    {
        public int SlideId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? Url { get; set; }
        public string? Image { get; set; }
        public int? SortOrder { get; set; }
    }
}