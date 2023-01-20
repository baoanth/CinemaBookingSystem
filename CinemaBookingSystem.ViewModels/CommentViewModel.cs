namespace CinemaBookingSystem.ViewModels
{
    public class CommentViewModel
    {
        public int CommentID { get; set; }
        public string Content { get; set; }
        public DateTime CommentedAt { get; set; }
        public int CommentedBy { get; set; }
        public int MovieID { get; set; }
        public virtual MovieViewModel Movie { get; set; }
        public virtual UserViewModel CommentedUser { get; set; }
        public int? StarRated { get; set; }
    }
}