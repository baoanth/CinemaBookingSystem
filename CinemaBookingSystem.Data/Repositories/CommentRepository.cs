using CinemaBookingSystem.Data.Infrastructure;
using CinemaBookingSystem.Model.Models;

namespace CinemaBookingSystem.Data.Repositories
{
    public interface ICommentRepository : IRepository<Comment>
    {
        IEnumerable<Comment> GetAll(int id);
        void RateTheMovie(int id, int starRated);
    }
    public class CommentRepository : RepositoryBase<Comment>, ICommentRepository
    {
        public CommentRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }

        public IEnumerable<Comment> GetAll(int id)
        {
            return DbContext.Comments.Where(x => x.MovieID == id).ToList();
        }

        public void RateTheMovie(int id, int starRated)
        {
            var comment = GetSingleById(id);
            comment.StarRated = starRated;
            Update(comment);
        }
    }
}
