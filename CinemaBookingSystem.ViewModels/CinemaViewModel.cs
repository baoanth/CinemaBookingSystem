namespace CinemaBookingSystem.ViewModels
{
    public class CinemaViewModel
    {
        public int CinemaId { get; set; }
        public string CinemaName { get; set; }
        public string FAX { get; set; }
        public string Hotline { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public IEnumerable<TheatreViewModel>? Theatres { get; set; }
    }
}