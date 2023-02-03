using CinemaBookingSystem.Data.Infrastructure;
using CinemaBookingSystem.Model.Models;

namespace CinemaBookingSystem.Data.Repositories
{
    public interface IScreeningPositionRepository : IRepository<ScreeningPosition>
    {
        void DeleteByScreening(int screeningId);
    }

    public class ScreeningPositionRepository : RepositoryBase<ScreeningPosition>, IScreeningPositionRepository
    {
        public ScreeningPositionRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public void DeleteByScreening(int screeningId)
        {
            IEnumerable<ScreeningPosition> list = DbContext.ScreeningPositions.Where(x => x.ScreeningId == screeningId).ToList();
            foreach (var item in list)
            {
                DbContext.ScreeningPositions.Remove(item);
            }
        }
    }
}