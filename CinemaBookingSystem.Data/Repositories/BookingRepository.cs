using CinemaBookingSystem.Data.Infrastructure;
using CinemaBookingSystem.Model.Models;

namespace CinemaBookingSystem.Data.Repositories
{
    public interface IBookingRepository : IRepository<Booking>
    {
        IEnumerable<Booking> GetAllByUser(int userId);
    }

    public class BookingRepository : RepositoryBase<Booking>, IBookingRepository
    {
        public BookingRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public IEnumerable<Booking> GetAllByUser(int userId)
        {
            return DbContext.Bookings.Where(x => x.UserId == userId).ToList();
        }
    }
}