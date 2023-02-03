namespace CinemaBookingSystem.ViewModels
{
    public class ScreeningViewModel
    {
        public int ScreeningId { get; set; }
        public DateTime ShowTime { get; set; }
        public bool ShowStatus { get; set; }
        public int TheatreId { get; set; }
        public int MovieId { get; set; }
        public virtual TheatreViewModel? Theatre { get; set; }
        public virtual MovieViewModel? Movie { get; set; }
        public IEnumerable<ScreeningPositionViewModel>? ScreeningPositions { get; set; }
    }
}