using CinemaBookingSystem.Data.Infrastructure;
using CinemaBookingSystem.Model.Models;

namespace CinemaBookingSystem.Data.Repositories
{
    public interface IScreeningRepository : IRepository<Screening>
    {
        IEnumerable<Screening> GetAllByTheatre(int theatreId);
    }
    public class ScreeningRepository : RepositoryBase<Screening>, IScreeningRepository
    {
        public ScreeningRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }

        public IEnumerable<Screening> GetAllByTheatre(int theatreId)
        {
            return DbContext.Screenings.Where(s => s.TheatreId == theatreId).ToList();
        }
    }
}
