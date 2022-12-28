using CinemaBookingSystem.Data.Infrastructure;
using CinemaBookingSystem.Model.Models;

namespace CinemaBookingSystem.Data.Repositories
{
    public interface IProvinceRepository : IRepository<Province>
    {
        IEnumerable<Province> GetByRegion(string region);
    }

    public class ProvinceRepository : RepositoryBase<Province>, IProvinceRepository
    {
        public ProvinceRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public IEnumerable<Province> GetByRegion(string region)
        {
            return DbContext.Provinces.Where(x => x.Region == region).ToList();
        }
    }
}