namespace CinemaBookingSystem.WebAPI.ViewModels
{
    public class BookingViewModel
    {
        public int BookingID { get; set; }
        public int PaymentID { get; set; }
        public DateTime BookedAt { get; set; }
        public bool IsPayed { get; set; }
        public virtual PaymentViewModel Payment { get; set; }
        public int UserID { get; set; }
        public virtual UserViewModel User { get; set; }
    }
}