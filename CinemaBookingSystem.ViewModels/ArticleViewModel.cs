using System.ComponentModel.DataAnnotations;

namespace CinemaBookingSystem.ViewModels
{
    public class ArticleViewModel
    {
        public int ArticleId { get; set; }
        [Required(ErrorMessage = "Tiêu đề bài viết là thông tin bắt buộc *")]
        public string ArticleTitle { get; set; }
        public string? ArticleImage { get; set; }
        public string? ArticleVideo { get; set; }
        [Required(ErrorMessage = "Nội dung của bài viết là thông tin bắt buộc *")]
        public string ArticleContent { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public virtual UserViewModel? User { get; set; }
    }
}