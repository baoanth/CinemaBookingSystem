namespace CinemaBookingSystem.ClientWeb.Models
{
    public class BookingDetailViewModel
    {
        public int BookingID { get; set; }
        public int PositionID { get; set; }
        public virtual BookingViewModel Booking { get; set; }
        public virtual ScreeningPositionViewModel ScreeningPosition { get; set; }
    }
}