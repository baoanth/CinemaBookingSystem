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
            string str = region.ToLower().Trim();
            return DbContext.Provinces.Where(x => (x.Region.ToLower().Trim()) == str).ToList();
        }
    }
}