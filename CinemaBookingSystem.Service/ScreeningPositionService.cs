using CinemaBookingSystem.Data.Infrastructure;
using CinemaBookingSystem.Data.Repositories;
using CinemaBookingSystem.Model.Models;

namespace CinemaBookingSystem.Service
{
    public interface IScreeningPositionService
    {
        void Add(ScreeningPosition screeningPosition);

        void Update(ScreeningPosition screeningPosition);

        void Delete(int id);

        void DeleteByScreening(int screeningId);

        IEnumerable<ScreeningPosition> GetAll();

        ScreeningPosition GetById(int id);

        void SaveChanges();
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

        public void Add(ScreeningPosition screeningPosition)
        {
            _screeningPositionRepository.Add(screeningPosition);
        }

        public void Delete(int id)
        {
            _screeningPositionRepository.Delete(id);
        }

        public void DeleteByScreening(int screeningId)
        {
            _screeningPositionRepository.DeleteByScreening(screeningId);
        }

        public IEnumerable<ScreeningPosition> GetAll()
        {
            return _screeningPositionRepository.GetAll();
        }

        public ScreeningPosition GetById(int id)
        {
            return _screeningPositionRepository.GetSingleById(id);
        }

        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }

        public void Update(ScreeningPosition screeningPosition)
        {
            _screeningPositionRepository.Update(screeningPosition);
        }
    }
}