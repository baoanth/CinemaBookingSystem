using CinemaBookingSystem.Data.Infrastructure;
using CinemaBookingSystem.Data.Repositories;
using CinemaBookingSystem.Model.Models;

namespace CinemaBookingSystem.Service
{
    public interface IPaymentService
    {
        void Add(Payment payment);

        void Update(Payment payment);

        void Delete(int id);

        IEnumerable<Payment> GetAll();

        Payment GetById(int id);

        void SaveChanges();
    }

    public class PaymentService : IPaymentService
    {
        private IPaymentRepository _paymentRepository;
        private IUnitOfWork _unitOfWork;

        public PaymentService(IPaymentRepository paymentRepository, IUnitOfWork unitOfWork)
        {
            _paymentRepository = paymentRepository;
            _unitOfWork = unitOfWork;
        }

        public void Add(Payment payment)
        {
            _paymentRepository.Add(payment);
        }

        public void Delete(int id)
        {
            _paymentRepository.Delete(id);
        }

        public IEnumerable<Payment> GetAll()
        {
            return _paymentRepository.GetAll();
        }

        public Payment GetById(int id)
        {
            return _paymentRepository.GetSingleById(id);
        }

        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }

        public void Update(Payment payment)
        {
            _paymentRepository.Update(payment);
        }
    }
}