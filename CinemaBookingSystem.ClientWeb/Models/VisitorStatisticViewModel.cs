namespace CinemaBookingSystem.ClientWeb.Models
{
    public class VisitorStatisticViewModel
    {
        public Guid ID { get; set; }
        public DateTime VisitedDate { get; set; }
        public string IPAddress { get; set; }
    }
}