namespace CinemaBookingSystem.ViewModels
{
    public class CustomerContactViewModel
    {
        public int Id { get; set; }

        public string CustomerName { get; set; }

        public string CustomerEmail { get; set; }

        public string CustomerPhone { get; set; }

        public string Content { get; set; }

        public DateTime SendedAt { get; set; }

        public bool Status { get; set; }
    }
}