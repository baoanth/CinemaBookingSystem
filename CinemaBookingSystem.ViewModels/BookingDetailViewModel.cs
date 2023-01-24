namespace CinemaBookingSystem.ViewModels
{
    public class BookingDetailViewModel
    {
        public int BookingId { get; set; }
        public int PositionId { get; set; }
        public virtual BookingViewModel Booking { get; set; }
        public virtual ScreeningPositionViewModel ScreeningPosition { get; set; }
    }
}