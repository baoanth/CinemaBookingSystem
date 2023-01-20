namespace CinemaBookingSystem.ViewModels
{
    public class LocationViewModel
    {
        public int LocationId { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public virtual IEnumerable<CinemaViewModel> Cinemas { get; set; }
    }
}