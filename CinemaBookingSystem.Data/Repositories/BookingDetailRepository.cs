using CinemaBookingSystem.Data.Infrastructure;
using CinemaBookingSystem.Model.Models;

namespace CinemaBookingSystem.Data.Repositories
{
    public interface IBookingDetailRepository : IRepository<BookingDetail>
    {
        IEnumerable<BookingDetail> GetAllByBooking(int bookingId);

        bool DeleteMulti(int bookingId);
    }

    public class BookingDetailRepository : RepositoryBase<BookingDetail>, IBookingDetailRepository
    {
        public BookingDetailRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public bool DeleteMulti(int bookingId)
        {
            var bookingDetails = DbContext.TicketBookings.Where(x => x.BookingId == bookingId);
            if (bookingDetails != null)
            {
                try
                {
                    foreach (var item in bookingDetails)
                    {
                        DbContext.TicketBookings.Remove(item);
                    }
                }
                catch (Exception ex)
                {
                    return false;
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public IEnumerable<BookingDetail> GetAllByBooking(int bookingId)
        {
            return DbContext.TicketBookings.Where(x => x.BookingId== bookingId).ToList();
        }
    }
}