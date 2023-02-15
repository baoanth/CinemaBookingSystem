using CinemaBookingSystem.Data.Infrastructure;
using CinemaBookingSystem.Data.Repositories;
using CinemaBookingSystem.Model.Models;

namespace CinemaBookingSystem.Service
{
    public interface IBookingService
    {
        void Add(Booking booking);

        void Update(Booking booking);

        void Delete(int id);

        IEnumerable<Booking> GetAll();

        IEnumerable<Booking> GetAllByUser(int userId);

        Booking GetById(int id);

        void SaveChanges();
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

        public void Add(Booking booking)
        {
            _bookingRepository.Add(booking);
        }

        public void Delete(int id)
        {
            _bookingRepository.Delete(id);
        }

        public IEnumerable<Booking> GetAll()
        {
            return _bookingRepository.GetAll();
        }

        public IEnumerable<Booking> GetAllByUser(int userId)
        {
            return _bookingRepository.GetAllByUser(userId);
        }

        public Booking GetById(int id)
        {
            return _bookingRepository.GetSingleById(id);
        }

        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }

        public void Update(Booking booking)
        {
            _bookingRepository.Update(booking);
        }
    }
}