using CinemaBookingSystem.Data.Infrastructure;
using CinemaBookingSystem.Model.Models;
using System.Linq.Expressions;

namespace CinemaBookingSystem.Data.Repositories
{
    public interface IBookingRepository : IRepository<Booking>
    {

    }
    public class BookingRepository : RepositoryBase<Booking>, IBookingRepository
    {
        public BookingRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }
    }
}
