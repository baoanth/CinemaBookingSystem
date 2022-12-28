namespace CinemaBookingSystem.WebAPI.ViewModels
{
    public class SupportOnlineViewModel
    {
        public int SupportID { get; set; }
        public string SupportName { get; set; }
        public string Department { get; set; }
        public string Mobile { get; set; }
        public string FacebookURL { get; set; }
        public string SupportEmail { get; set; }
        public bool IsWorking { get; set; }
    }
}