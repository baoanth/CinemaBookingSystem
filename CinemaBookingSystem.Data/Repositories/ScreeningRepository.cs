using CinemaBookingSystem.Data.Infrastructure;
using CinemaBookingSystem.Model.Models;

namespace CinemaBookingSystem.Data.Repositories
{
    public interface IScreeningRepository : IRepository<Screening>
    {
        IEnumerable<Screening> GetAll();
        IEnumerable<Screening> GetAllByTheatre(int theatreId);
        IEnumerable<Screening> GetAllByCinemaAndMovie(int cinemaId,int movieId);
    }
    public class ScreeningRepository : RepositoryBase<Screening>, IScreeningRepository
    {
        public ScreeningRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }

        public IEnumerable<Screening> GetAll()
        {
            return DbContext.Screenings.AsNoTracking().ToList();
        }

        public IEnumerable<Screening> GetAllByCinemaAndMovie(int cinemaId, int movieId)
        {
            return DbContext.Screenings.Where(x => x.Theatre.CinemaId == cinemaId && x.MovieId == movieId).ToList();
        }

        public IEnumerable<Screening> GetAllByTheatre(int theatreId)
        {
            return DbContext.Screenings.Where(s => s.TheatreId == theatreId).ToList();
        }
    }
}
