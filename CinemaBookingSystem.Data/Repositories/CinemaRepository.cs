using CinemaBookingSystem.Data.Infrastructure;
using CinemaBookingSystem.Model.Models;

namespace CinemaBookingSystem.Data.Repositories
{
    public interface ICinemaRepository : IRepository<Cinema>
    {
        IEnumerable<Cinema> GetByProvince(int provinceId);
    }
    public class CinemaRepository : RepositoryBase<Cinema>, ICinemaRepository
    {
        public CinemaRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }

        public IEnumerable<Cinema> GetByProvince(int provinceId)
        {
            return DbContext.Cinemas.Where(x => x.ProvinceID == provinceId);
        }
    }
}
