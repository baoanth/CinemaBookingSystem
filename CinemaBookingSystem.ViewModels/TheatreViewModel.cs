namespace CinemaBookingSystem.ViewModels
{
    public class TheatreViewModel
    {
        public int TheatreId { get; set; }
        public string TheatreName { get; set; }
        public int Capacity { get; set; }
        public int CinemaId { get; set; }
        public virtual CinemaViewModel? Cinema { get; set; }
        public IEnumerable<ScreeningViewModel>? Screenings { get; set; }
    }
}