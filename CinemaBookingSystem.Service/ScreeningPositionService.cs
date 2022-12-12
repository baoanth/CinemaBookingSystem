using CinemaBookingSystem.Data.Infrastructure;
using CinemaBookingSystem.Data.Repositories;

namespace CinemaBookingSystem.Service
{
    public interface IScreeningPositionService
    {
    }

    public class ScreeningPositionService : IScreeningPositionService
    {
        private IScreeningPositionRepository _screeningPositionRepository;
        private IUnitOfWork _unitOfWork;

        public ScreeningPositionService(IScreeningPositionRepository screeningPositionRepository, IUnitOfWork unitOfWork)
        {
            _screeningPositionRepository = screeningPositionRepository;
            _unitOfWork = unitOfWork;
        }
    }
}