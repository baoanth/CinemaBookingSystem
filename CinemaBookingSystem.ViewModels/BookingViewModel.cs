namespace CinemaBookingSystem.ViewModels
{
    public class BookingViewModel
    {
        public int BookingId { get; set; }
        public int PaymentId { get; set; }
        public DateTime BookedAt { get; set; }
        public bool IsPayed { get; set; }
        public virtual PaymentViewModel? Payment { get; set; }
        public int UserId { get; set; }
        public virtual UserViewModel? User { get; set; }
    }
}