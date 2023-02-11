using System.ComponentModel.DataAnnotations;

namespace CinemaBookingSystem.ViewModels
{
    public class CinemaViewModel
    {
        public int CinemaId { get; set; }
        [Required(ErrorMessage = "Tên rạp là thông tin bắt buộc *")]
        public string CinemaName { get; set; }
        [Required(ErrorMessage = "Mã FAX là thông tin bắt buộc *")]
        public string FAX { get; set; }
        [Required(ErrorMessage = "Hotline rạp là thông tin bắt buộc *")]
        public string Hotline { get; set; }
        [Required(ErrorMessage = "Địa chỉ rạp là thông tin bắt buộc *")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Tên thành phố là thông tin bắt buộc *")]
        public string City { get; set; }
        [Required(ErrorMessage = "Tên miền là thông tin bắt buộc *")]
        public string Region { get; set; }
        public IEnumerable<TheatreViewModel>? Theatres { get; set; }
    }
}