namespace CinemaBookingSystem.ViewModels
{
    public class RoleViewModel
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string? RoleDescription { get; set; }
        public IEnumerable<UserViewModel> Users { get; set; }
    }
}