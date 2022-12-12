using CinemaBookingSystem.Data.Infrastructure;
using CinemaBookingSystem.Model.Models;

namespace CinemaBookingSystem.Data.Repositories
{
    public interface IVisitorStatisticRepository : IRepository<VisitorStatistic>
    {
        IEnumerable<VisitorStatistic> GetByIPAddress(string IPAddress);
    }
    public class VisitorStatisticRepository : RepositoryBase<VisitorStatistic>, IVisitorStatisticRepository
    {
        public VisitorStatisticRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }

        public IEnumerable<VisitorStatistic> GetByIPAddress(string ipAddress)
        {
            return DbContext.VisitorStatistics.Where(x => x.IPAddress == ipAddress).ToList();
        }
    }
}
