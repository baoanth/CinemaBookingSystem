using CinemaBookingSystem.Data.Infrastructure;
using CinemaBookingSystem.Data.Repositories;
using CinemaBookingSystem.Model.Models;

namespace CinemaBookingSystem.Service
{
    public interface ICarouselService
    {
        void Add(Carousel carousel);
        void Update(Carousel carousel);
        void Delete(int id);
        IEnumerable<Carousel> GetAll();
        Carousel GetById(int id);
        void SaveChanges();
    }
    public class CarouselService : ICarouselService
    {
        ICarouselRepository _carouselRepository;
        IUnitOfWork _unitOfWork;
        public CarouselService(ICarouselRepository carouselRepository, IUnitOfWork unitOfWork)
        {
            _carouselRepository = carouselRepository;
            _unitOfWork = unitOfWork;
        }

        public void Add(Carousel carousel)
        {
            _carouselRepository.Add(carousel);
        }

        public void Delete(int id)
        {
            _carouselRepository.Delete(id);
        }

        public IEnumerable<Carousel> GetAll()
        {
            return _carouselRepository.GetAll();
        }

        public Carousel GetById(int id)
        {
            return _carouselRepository.GetSingleById(id);
        }

        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }

        public void Update(Carousel carousel)
        {
            _carouselRepository.Update(carousel);
        }
    }
}