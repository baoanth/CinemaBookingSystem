using CinemaBookingSystem.Data.Infrastructure;
using CinemaBookingSystem.Data.Repositories;
using CinemaBookingSystem.Model.Models;

namespace CinemaBookingSystem.Service
{
    public interface IScreeningService
    {
        bool Add(Screening screening);

        void Delete(int id);

        bool Update(Screening screening);

        IEnumerable<Screening> GetAll();

        IEnumerable<Screening> GetAllByTheatre(int theatreId);

        IEnumerable<Screening> GetAllByMovie(int movieId);

        IEnumerable<Screening> GetAllByCinemaAndMovie(int cinemaId, int movieId);

        Screening GetById(int id);

        void SaveChanges();
    }

    public class ScreeningService : IScreeningService
    {
        private IScreeningRepository _screeningRepository;
        private IMovieRepository _movieRepository;
        private IUnitOfWork _unitOfWork;

        public ScreeningService(IScreeningRepository screeningRepository, IUnitOfWork unitOfWork, IMovieRepository movieRepository)
        {
            _screeningRepository = screeningRepository;
            _unitOfWork = unitOfWork;
            _movieRepository = movieRepository;
        }

        public bool Add(Screening screening)
        {
            Movie movie = _movieRepository.GetSingleById(screening.MovieId);
            DateTime screeningStart = screening.ShowTime;
            DateTime screeningEnd = screening.ShowTime.AddMinutes(movie.RunningTime + 20);
            bool overlap = _screeningRepository.GetAll().Where(x => x.TheatreId == screening.TheatreId && x.ShowTime.Date == screening.ShowTime.Date)
                .Any(x => x.ShowTime <= screeningEnd && screeningStart <= x.ShowTime.AddMinutes(x.Movie.RunningTime + 20));
            if (!overlap)
            {
                _screeningRepository.Add(screening);
            }
            return !overlap;
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

        public IEnumerable<Screening> GetAllByMovie(int movieId)
        {
            return _screeningRepository.GetAllByMovie(movieId);
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

        public bool Update(Screening screening)
        {
            Movie movie = _movieRepository.GetSingleById(screening.MovieId);
            DateTime screeningStart = screening.ShowTime;
            DateTime screeningEnd = screening.ShowTime.AddMinutes(movie.RunningTime + 20);
            bool overlap = _screeningRepository.GetAll().Where(x => x.TheatreId == screening.TheatreId && x.ShowTime.Date == screening.ShowTime.Date && x.ScreeningId != screening.ScreeningId)
                .Any(x => x.ShowTime <= screeningEnd && screeningStart <= x.ShowTime.AddMinutes(x.Movie.RunningTime + 20));
            if (!overlap)
            {
                _screeningRepository.Update(screening);
            }
            return !overlap;
        }
    }
}