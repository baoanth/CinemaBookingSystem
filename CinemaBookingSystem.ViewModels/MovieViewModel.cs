using System.ComponentModel.DataAnnotations;

namespace CinemaBookingSystem.ViewModels
{
    public class MovieViewModel
    {
        public int MovieId { get; set; }
        [Required(ErrorMessage = "Tên phim là thông tin bắt buộc *")]
        public string MovieName { get; set; }
        [Required(ErrorMessage = "Tên đạo diễn là thông tin bắt buộc *")]
        public string Director { get; set; }
        [Required(ErrorMessage = "Tên các diễn viên là thông tin bắt buộc *")]
        public string Cast { get; set; }
        [Required(ErrorMessage = "Ngày khởi chiếu là thông tin bắt buộc *")]
        public DateTime ReleaseDate { get; set; }
        [Required(ErrorMessage = "Thể loại phim là thông tin bắt buộc *")]
        public string Genres { get; set; }
        [Required(ErrorMessage = "Thời gian chiếu là thông tin bắt buộc *")]
        public int RunningTime { get; set; }
        [Required(ErrorMessage = "Đánh giá phim là thông tin bắt buộc *")]
        public string Rated { get; set; }
        public string? TrailerURL { get; set; }
        public string? ThumpnailImg { get; set; }
        public string? BannerImg { get; set; }
        [Required(ErrorMessage = "Mô tả phim là thông tin bắt buộc *")]
        public string Description { get; set; }
        public IEnumerable<ScreeningViewModel>? Screenings { get; set; }
        public IEnumerable<CommentViewModel>? Comments { get; set; }
    }
}