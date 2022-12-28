using CinemaBookingSystem.Data.Infrastructure;
using CinemaBookingSystem.Model.Models;

namespace CinemaBookingSystem.Data.Repositories
{
    public interface IBookingDetailRepository : IRepository<BookingDetail>
    {
    }

    public class BookingDetailRepository : RepositoryBase<BookingDetail>, IBookingDetailRepository
    {
        public BookingDetailRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}