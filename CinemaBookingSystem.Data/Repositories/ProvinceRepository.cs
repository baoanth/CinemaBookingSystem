using CinemaBookingSystem.Data.Infrastructure;
using CinemaBookingSystem.Model.Models;

namespace CinemaBookingSystem.Data.Repositories
{
    public interface IProvinceRepository
    {
        IEnumerable<Province> GetByRegion(string region);
    }
    public class ProvinceRepository : RepositoryBase<Province>,IProvinceRepository
    {
        public ProvinceRepository(DbFactory dbFactory) : base(dbFactory)
        {

        }

        public IEnumerable<Province> GetByRegion(string region)
        {
            return this.DbContext.Provinces.Where(x=>x.Region == region).ToList();
        }
    }
}
