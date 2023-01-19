using CinemaBookingSystem.Data.Infrastructure;
using CinemaBookingSystem.Data.Repositories;
using CinemaBookingSystem.Model.Models;

namespace CinemaBookingSystem.Service
{
    public interface ISlideService
    {
        void Add(Slide slide);

        void Update(Slide slide);

        void Delete(int id);

        IEnumerable<Slide> GetAll();

        Slide GetById(int id);

        void SaveChanges();
    }

    public class SlideService : ISlideService
    {
        private ISlideRepository _slideRepository;
        private IUnitOfWork _unitOfWork;

        public SlideService(ISlideRepository slideRepository, IUnitOfWork unitOfWork)
        {
            _slideRepository = slideRepository;
            _unitOfWork = unitOfWork;
        }

        public void Add(Slide slide)
        {
            _slideRepository.Add(slide);
        }

        public void Delete(int id)
        {
            _slideRepository.Delete(id);
        }

        public IEnumerable<Slide> GetAll()
        {
            return _slideRepository.GetAll();
        }

        public Slide GetById(int id)
        {
            return _slideRepository.GetSingleById(id);
        }

        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }

        public void Update(Slide slide)
        {
            _slideRepository.Update(slide);
        }
    }
}