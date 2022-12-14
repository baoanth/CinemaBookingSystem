using CinemaBookingSystem.Data.Infrastructure;
using CinemaBookingSystem.Data.Repositories;
using CinemaBookingSystem.Model.Models;

namespace CinemaBookingSystem.Service
{
    public interface IScreeningService
    {
        void Add(Screening screening);

        void Delete(int id);

        void Update(Screening screening);

        IEnumerable<Screening> GetAll();

        Screening GetById(int id);

        void SaveChanges();
    }

    public class ScreeningService : IScreeningService
    {
        private IScreeningRepository _screeningRepository;
        private IUnitOfWork _unitOfWork;

        public ScreeningService(IScreeningRepository screeningRepository, IUnitOfWork unitOfWork)
        {
            _screeningRepository = screeningRepository;
            _unitOfWork = unitOfWork;
        }

        public void Add(Screening screening)
        {
            _screeningRepository.Add(screening);
        }

        public void Delete(int id)
        {
            _screeningRepository.Delete(id);
        }

        public IEnumerable<Screening> GetAll()
        {
            return _screeningRepository.GetAll();
        }

        public Screening GetById(int id)
        {
            return _screeningRepository.GetSingleById(id);
        }

        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }

        public void Update(Screening screening)
        {
            _screeningRepository.Update(screening);
        }
    }
}