using System.ComponentModel.DataAnnotations;

namespace CinemaBookingSystem.ViewModels
{
    public class UserViewModel
    {
        public int UserId { get; set; }
        [Required(ErrorMessage = "Tên tài khoản là thông tin bắt buộc *")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Mật khẩu là thông tin bắt buộc *")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Họ và tên là thông tin bắt buộc *")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "Ngày sinh là thông tin bắt buộc *")]
        public DateTime? DOB { get; set; }
        [Required(ErrorMessage = "Email là thông tin bắt buộc *")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail không hợp lệ")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Số điện thoại là thông tin bắt buộc *")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Số điện thoại không hợp lệ")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Địa chỉ là thông tin bắt buộc *")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Quyền hệ thống là thông tin bắt buộc *")]
        public int RoleId { get; set; }
        public virtual RoleViewModel? Role { get; set; }
        public IEnumerable<CommentViewModel>? Comments { get; set; }
        public IEnumerable<BookingViewModel>? Bookings { get; set; }
    }
}