using CinemaBookingSystem.Data.Infrastructure;
using CinemaBookingSystem.Model.Models;

namespace CinemaBookingSystem.Data.Repositories
{
    public interface ITheatreRepository
    {

    }
    public class TheatreRepository : RepositoryBase<Theatre>, ITheatreRepository
    {
        public TheatreRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }
    }
}
