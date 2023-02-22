namespace CinemaBookingSystem.ViewModels
{
    public class BookingViewModel
    {
        public int BookingId { get; set; }
        public int PaymentId { get; set; }
        public DateTime BookedAt { get; set; }
        public bool IsPaid { get; set; }
        public virtual PaymentViewModel? Payment { get; set; }
        public int UserId { get; set; }
        public virtual UserViewModel? User { get; set; }
        public string? VerifyCode { get; set; }
    }
}