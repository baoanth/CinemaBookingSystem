namespace CinemaBookingSystem.WebAPI.ViewModels
{
    public class ProvinceViewModel
    {
        public int ProvinceID { get; set; }
        public string ProvinceName { get; set; }
        public string Region { get; set; }
        public virtual IEnumerable<CinemaViewModel> Cinemas { get; set; }
    }
}