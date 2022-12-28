namespace CinemaBookingSystem.WebAI.ViewModels
{
    public class UserViewModel
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public int RoleID { get; set; }
        public virtual RoleViewModel Role { get; set; }
        public IEnumerable<CommentViewModel> Comments { get; set; }
        public IEnumerable<BookingViewModel> Bookings { get; set; }
    }
}