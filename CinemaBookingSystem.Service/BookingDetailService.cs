using CinemaBookingSystem.Data.Infrastructure;
using CinemaBookingSystem.Data.Repositories;

namespace CinemaBookingSystem.Service
{
    public interface IBookingDetailService
    {
    }

    public class BookingDetailService : IBookingDetailService
    {
        private IBookingDetailRepository _bookingDetailRepository;
        private IUnitOfWork _unitOfWork;

        public BookingDetailService(IBookingDetailRepository bookingDetailRepository, IUnitOfWork unitOfWork)
        {
            _bookingDetailRepository = bookingDetailRepository;
            _unitOfWork = unitOfWork;
        }
    }
}