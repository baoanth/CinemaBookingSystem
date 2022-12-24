namespace CinemaBookingSystem.WebAPI.ViewModels
{
    public class ScreeningPositionViewModel
    {
        public int PositionID { get; set; }
        public string Row { get; set; }
        public string Column { get; set; }
        public bool IsBooked { get; set; }
        public int ScreeningID { get; set; }
        public virtual ScreeningViewModel Screening { get; set; }
    }
}