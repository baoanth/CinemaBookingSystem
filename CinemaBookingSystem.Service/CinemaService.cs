using CinemaBookingSystem.Data.Infrastructure;
using CinemaBookingSystem.Data.Repositories;

namespace CinemaBookingSystem.Service
{
    public interface ICinemaService
    {
    }

    public class CinemaService : ICinemaService
    {
        private ICinemaRepository _cinemaRepository;
        private IUnitOfWork _unitOfWork;

        public CinemaService(ICinemaRepository cinemaRepository, IUnitOfWork unitOfWork)
        {
            _cinemaRepository = cinemaRepository;
            _unitOfWork = unitOfWork;
        }
    }
}