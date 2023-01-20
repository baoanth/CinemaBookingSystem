namespace CinemaBookingSystem.ViewModels
{
    public class ScreeningViewModel
    {
        public int ScreeningID { get; set; }
        public DateOnly ShowDate { get; set; }
        public DateTime ShowTime { get; set; }
        public bool ShowStatus { get; set; }
        public bool IsFull { get; set; }
        public int EmptySeats { get; set; }
        public int TheatreID { get; set; }
        public int MovieID { get; set; }
        public virtual TheatreViewModel? Theatre { get; set; }
        public virtual MovieViewModel? Movie { get; set; }
        public IEnumerable<ScreeningPositionViewModel>? ScreeningPositions { get; set; }
    }
}