namespace CinemaBookingSystem.ViewModels
{
    public class ArticleViewModel
    {
        public int ArticleId { get; set; }
        public string ArticleTitle { get; set; }
        public string? ArticleImage { get; set; }
        public string? ArticleVideo { get; set; }
        public string ArticleContent { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public virtual UserViewModel? User { get; set; }
    }
}