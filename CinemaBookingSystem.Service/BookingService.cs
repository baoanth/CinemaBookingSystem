using CinemaBookingSystem.Data.Infrastructure;
using CinemaBookingSystem.Data.Repositories;

namespace CinemaBookingSystem.Service
{
    public interface IBookingService
    {
    }

    public class BookingService : IBookingService
    {
        private IBookingRepository _bookingRepository;
        private IUnitOfWork _unitOfWork;
        public BookingService(IBookingRepository bookingRepository, IUnitOfWork unitOfWork)
        {
            _bookingRepository = bookingRepository;
            _unitOfWork = unitOfWork;
        }
    }
}