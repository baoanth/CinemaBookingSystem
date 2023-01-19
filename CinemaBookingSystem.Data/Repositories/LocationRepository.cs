using CinemaBookingSystem.Data.Infrastructure;
using CinemaBookingSystem.Model.Models;

namespace CinemaBookingSystem.Data.Repositories
{
    public interface ILocationRepository : IRepository<Location>
    {
    }

    public class LocationRepository : RepositoryBase<Location>, ILocationRepository
    {
        public LocationRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}