namespace CinemaBookingSystem.ClientWeb.Models
{
    public class RoleViewModel
    {
        public int RoleID { get; set; }
        public string RoleName { get; set; }
        public string? RoleDescription { get; set; }
        public IEnumerable<UserViewModel> Users { get; set; }
    }
}