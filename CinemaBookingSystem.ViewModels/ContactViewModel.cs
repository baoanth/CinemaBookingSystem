using System.ComponentModel.DataAnnotations;

namespace CinemaBookingSystem.ViewModels
{
    public class ContactViewModel
    {
        public int ContactId { get; set; }
        [Required(ErrorMessage = "Tên người hỗ trợ là thông tin bắt buộc *")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Địa chỉ là thông tin bắt buộc *")]
        public string Department { get; set; }
        [Required(ErrorMessage = "Số điện thoại là thông tin bắt buộc *")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Số điện thoại không hợp lệ")]
        public string Mobile { get; set; }
        [Required(ErrorMessage = "Link facebook là thông tin bắt buộc *")]
        public string FacebookURL { get; set; }
        [Required(ErrorMessage = "Địa chỉ email là thông tin bắt buộc *")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Nội dung tin nhắn là thông tin bắt buộc *")]
        public string Message { get; set; }
        [Required(ErrorMessage = "Hãy set trạng thái hoạt động")]
        public bool IsWorking { get; set; }
    }
}