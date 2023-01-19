using CinemaBookingSystem.Data.Infrastructure;
using CinemaBookingSystem.Model.Models;

namespace CinemaBookingSystem.Data.Repositories
{
    public interface ISupportOnlineRepository : IRepository<Contact>
    {

    }
    public class ContactRepository : RepositoryBase<Contact>, ISupportOnlineRepository
    {
        public ContactRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }
    }
}
