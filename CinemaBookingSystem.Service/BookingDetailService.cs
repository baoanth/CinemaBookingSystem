using CinemaBookingSystem.Data.Infrastructure;
using CinemaBookingSystem.Data.Repositories;
using CinemaBookingSystem.Model.Models;

namespace CinemaBookingSystem.Service
{
    public interface IBookingDetailService
    {
        void Add(BookingDetail bookingDetail);

        void Update(BookingDetail bookingDetail);

        void Delete(int id);

        IEnumerable<BookingDetail> GetAll();

        BookingDetail GetById(int id);

        void SaveChanges();
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

        public void Add(BookingDetail bookingDetail)
        {
            _bookingDetailRepository.Add(bookingDetail);
        }

        public void Delete(int id)
        {
            _bookingDetailRepository.Delete(id);
        }

        public IEnumerable<BookingDetail> GetAll()
        {
            return _bookingDetailRepository.GetAll();
        }

        public BookingDetail GetById(int id)
        {
            return _bookingDetailRepository.GetSingleById(id);
        }

        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }

        public void Update(BookingDetail bookingDetail)
        {
            _bookingDetailRepository.Update(bookingDetail);
        }
    }
}