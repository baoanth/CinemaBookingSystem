using CinemaBookingSystem.Data.Infrastructure;
using CinemaBookingSystem.Data.Repositories;
using CinemaBookingSystem.Model.Models;

namespace CinemaBookingSystem.Service
{
    public interface ICinemaService
    {
        void Add(Cinema cinema);

        void Update(Cinema cinema);

        void Delete(int id);

        IEnumerable<Cinema> GetAll();

        Cinema GetById(int id);

        void SaveChanges();
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

        public void Add(Cinema cinema)
        {
            _cinemaRepository.Add(cinema);
        }

        public void Delete(int id)
        {
            _cinemaRepository.Delete(id);
        }

        public IEnumerable<Cinema> GetAll()
        {
            return _cinemaRepository.GetAll();
        }

        public Cinema GetById(int id)
        {
            return _cinemaRepository.GetSingleById(id);
        }


        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }

        public void Update(Cinema cinema)
        {
            _cinemaRepository.Update(cinema);
        }
    }
}