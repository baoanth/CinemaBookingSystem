using System.ComponentModel.DataAnnotations;

namespace CinemaBookingSystem.ViewModels
{
    public class PaymentViewModel
    {
        public int PaymentId { get; set; }
        [Required(ErrorMessage = "Phương thức thanh toán là thông tin bắt buộc *")]
        public string PaymentMethod { get; set; }
    }
}