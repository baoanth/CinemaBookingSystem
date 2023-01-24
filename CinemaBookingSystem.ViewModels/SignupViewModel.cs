namespace CinemaBookingSystem.ViewModels
{
    public class SignupViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string EmailAddress { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public int RoleId { get; set; }
    }
}