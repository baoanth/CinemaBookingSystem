using CinemaBookingSystem.Data.Infrastructure;
using CinemaBookingSystem.Data.Repositories;
using CinemaBookingSystem.Model.Models;

namespace CinemaBookingSystem.Service
{
    public interface ITheatreService
    {
        void Add(Theatre theatre);

        void Update(Theatre theatre);

        void Delete(int id);

        IEnumerable<Theatre> GetAll();

        IEnumerable<Theatre> GetAllByCinema(int cinemaId);

        Theatre GetById(int id);

        void SaveChanges();
    }

    public class TheatreService : ITheatreService
    {
        private ITheatreRepository _theatreRepository;
        private IUnitOfWork _unitOfWork;

        public TheatreService(ITheatreRepository theatreRepository, IUnitOfWork unitOfWork)
        {
            _theatreRepository = theatreRepository;
            _unitOfWork = unitOfWork;
        }

        public void Add(Theatre theatre)
        {
            _theatreRepository.Add(theatre);
        }

        public void Delete(int id)
        {
            _theatreRepository.Delete(id);
        }

        public IEnumerable<Theatre> GetAll()
        {
            return _theatreRepository.GetAll();
        }

        public IEnumerable<Theatre> GetAllByCinema(int cinemaId)
        {
            return _theatreRepository.GetAllByCinema(cinemaId);
        }

        public Theatre GetById(int id)
        {
            return _theatreRepository.GetSingleById(id);
        }

        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }

        public void Update(Theatre theatre)
        {
            _theatreRepository.Update(theatre);
        }
    }
}