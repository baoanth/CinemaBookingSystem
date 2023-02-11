namespace CinemaBookingSystem.ViewModels
{
    public class CommentViewModel
    {
        public int CommentId { get; set; }
        public string Content { get; set; }
        public DateTime CommentedAt { get; set; }
        public int CommentedBy { get; set; }
        public int MovieId { get; set; }
        public virtual MovieViewModel? Movie { get; set; }
        public virtual UserViewModel? CommentedUser { get; set; }
        public int? StarRated { get; set; }
    }
}