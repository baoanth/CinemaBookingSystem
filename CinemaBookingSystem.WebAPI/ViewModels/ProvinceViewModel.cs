using System.ComponentModel.DataAnnotations;

namespace CinemaBookingSystem.WebAPI.ViewModels
{
    public class ProvinceViewModel
    {
        public int ProvinceID { get; set; }
        [Required]
        public string ProvinceName { get; set; }
        [Required]
        public string Region { get; set; }
        public virtual IEnumerable<CinemaViewModel>? Cinemas { get; set; }
    }
}