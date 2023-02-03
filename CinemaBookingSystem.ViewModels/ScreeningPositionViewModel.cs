namespace CinemaBookingSystem.ViewModels
{
    public class ScreeningPositionViewModel
    {
        public int PositionId { get; set; }
        public string Row { get; set; }
        public string Column { get; set; }
        public int Price { get; set; }
        public bool IsBooked { get; set; }
        public int ScreeningId { get; set; }
        public virtual ScreeningViewModel? Screening { get; set; }
    }
}