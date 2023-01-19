using CinemaBookingSystem.Data.Infrastructure;
using CinemaBookingSystem.Model.Models;

namespace CinemaBookingSystem.Data.Repositories
{
    public interface ICommentRepository : IRepository<Comment>
    {
        IEnumerable<Comment> GetAllByMovie(int movieId);
    }
    public class CommentRepository : RepositoryBase<Comment>, ICommentRepository
    {
        public CommentRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }

        public IEnumerable<Comment> GetAllByMovie(int movieId)
        {
            return DbContext.Comments.Where(x => x.MovieId == movieId);
        }
    }
}
