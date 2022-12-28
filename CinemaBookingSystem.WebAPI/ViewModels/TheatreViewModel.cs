namespace CinemaBookingSystem.WebAPI.ViewModels
{
    public class TheatreViewModel
    {
        public int TheatreID { get; set; }
        public string TheatreName { get; set; }
        public int Capacity { get; set; }
        public int CinemaID { get; set; }
        public virtual CinemaViewModel Cinema { get; set; }
        public IEnumerable<ScreeningViewModel> Screenings { get; set; }
    }
}