using System.ComponentModel.DataAnnotations;

namespace CinemaBookingSystem.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Tài khoản là thông tin bắt buộc *")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Mật khẩu là thông tin bắt buộc *")]
        public string Password { get; set; }
    }
}
