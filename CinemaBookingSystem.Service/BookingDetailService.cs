using CinemaBookingSystem.Data.Infrastructure;
using CinemaBookingSystem.Data.Repositories;
using CinemaBookingSystem.Model.Models;

namespace CinemaBookingSystem.Service
{
    public interface IBookingDetailService
    {
        void Add(BookingDetail bookingDetail);

        void Update(BookingDetail bookingDetail);

        bool DeleteMulti(int bookingId);

        IEnumerable<BookingDetail> GetAll();

        IEnumerable<BookingDetail> GetAllByBooking(int bookingId);

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

        public bool DeleteMulti(int bookingId)
        {
            return _bookingDetailRepository.DeleteMulti(bookingId);
        }

        public IEnumerable<BookingDetail> GetAll()
        {
            return _bookingDetailRepository.GetAll();
        }

        public IEnumerable<BookingDetail> GetAllByBooking(int bookingId)
        {
            return _bookingDetailRepository.GetAllByBooking(bookingId);
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