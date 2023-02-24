using CinemaBookingSystem.Data.Infrastructure;
using CinemaBookingSystem.Data.Repositories;
using CinemaBookingSystem.Model.Models;

namespace CinemaBookingSystem.Service
{
    public interface IScreeningService
    {
        bool Add(Screening screening);

        void Delete(int id);

        void Update(Screening screening);

        IEnumerable<Screening> GetAll();

        IEnumerable<Screening> GetAllByTheatre(int theatreId);

        IEnumerable<Screening> GetAllByCinemaAndMovie(int cinemaId, int movieId);

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

        public bool Add(Screening screening)
        {
            bool check = _screeningRepository.GetAll().Where(x => x.TheatreId == screening.TheatreId).Any(x => (x.ShowTime <= screening.ShowTime
            && x.ShowTime.AddMinutes(x.Movie.RunningTime + 30) >= screening.ShowTime)
            || (x.ShowTime <= screening.ShowTime.AddMinutes(screening.Movie.RunningTime + 30)
            && x.ShowTime.AddMinutes(x.Movie.RunningTime + 30) >= screening.ShowTime.AddMinutes(screening.Movie.RunningTime + 30)));
            /*            foreach (var item in list)
                        {
                            DateTime startShow = item.ShowTime;
                            DateTime endShow = item.ShowTime.AddMinutes(item.Movie.RunningTime + 30);
                            if (screening.ShowTime < endShow && screening.ShowTime > startShow && screening.TheatreId == item.TheatreId)
                            {
                                return false;
                            }
                        }*/
            if (!check)
            {
                _screeningRepository.Add(screening);
            }
            return !check;
        }

        public void Delete(int id)
        {
            _screeningRepository.Delete(id);
        }

        public IEnumerable<Screening> GetAll()
        {
            return _screeningRepository.GetAll();
        }

        public IEnumerable<Screening> GetAllByCinemaAndMovie(int cinemaId, int movieId)
        {
            return _screeningRepository.GetAllByCinemaAndMovie(cinemaId, movieId);
        }

        public IEnumerable<Screening> GetAllByTheatre(int theatreId)
        {
            return _screeningRepository.GetAllByTheatre(theatreId);
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