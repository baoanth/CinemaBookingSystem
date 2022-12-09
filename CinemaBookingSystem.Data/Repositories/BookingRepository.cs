using CinemaBookingSystem.Data.Infrastructure;
using CinemaBookingSystem.Model.Models;

namespace CinemaBookingSystem.Data.Repositories
{
    public interface IBookingRepository
    {

    }
    public class BookingRepository : RepositoryBase<Booking>, IBookingRepository
    {
        public BookingRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }
    }
}
