using CinemaBookingSystem.Data.Infrastructure;
using CinemaBookingSystem.Model.Models;

namespace CinemaBookingSystem.Data.Repositories
{
    public interface ITheatreRepository : IRepository<Theatre>
    {
        IEnumerable<Theatre> GetAllByCinema(int cinemaId);
    }
    public class TheatreRepository : RepositoryBase<Theatre>, ITheatreRepository
    {
        public TheatreRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }

        public IEnumerable<Theatre> GetAllByCinema(int cinemaId)
        {
            return DbContext.Theatres.Where(x => x.CinemaId == cinemaId).ToList();
        }
    }
}
