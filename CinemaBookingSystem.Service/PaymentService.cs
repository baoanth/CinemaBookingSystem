using CinemaBookingSystem.Data.Infrastructure;
using CinemaBookingSystem.Data.Repositories;

namespace CinemaBookingSystem.Service
{
    public interface IPaymentService
    {
    }

    public class PaymentService
    {
        private IPaymentRepository _paymentRepository;
        private IUnitOfWork _unitOfWork;

        public PaymentService(IPaymentRepository paymentRepository, IUnitOfWork unitOfWork)
        {
            _paymentRepository = paymentRepository;
            _unitOfWork = unitOfWork;
        }
    }
}