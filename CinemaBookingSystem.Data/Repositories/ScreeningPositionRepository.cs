using CinemaBookingSystem.Data.Infrastructure;
using CinemaBookingSystem.Model.Models;

namespace CinemaBookingSystem.Data.Repositories
{
    public interface IScreeningPositionRepository
    {

    }
    public class ScreeningPositionRepository : RepositoryBase<ScreeningPosition>, IScreeningPositionRepository
    {
        public ScreeningPositionRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }
    }
}
