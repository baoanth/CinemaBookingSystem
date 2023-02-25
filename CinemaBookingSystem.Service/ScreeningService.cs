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
            bool check = _screeningRepository.GetAll().Any(x => x.TheatreId == screening.TheatreId
            && (x.ShowTime <= screening.ShowTime && x.ShowTime.AddMinutes(x.Movie.RunningTime + 30) >= screening.ShowTime.AddMinutes(movie.RunningTime + 30)
            || x.ShowTime <= screening.ShowTime.AddMinutes(movie.RunningTime + 30) && x.ShowTime.AddMinutes(x.Movie.RunningTime + 30) >= screening.ShowTime.AddMinutes(movie.RunningTime + 30)));
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

        public bool Update(Screening screening)
        {
            if (Check(screening))
            {
                _screeningRepository.Update(screening);
            }
            return Check(screening);
        }
        private bool Check(Screening screening)
        {
            Movie movie = _movieRepository.GetSingleById(screening.MovieId);
            bool check = _screeningRepository.GetAll().Any(x => x.TheatreId == screening.TheatreId && x.ScreeningId != screening.ScreeningId
            && (x.ShowTime <= screening.ShowTime && x.ShowTime.AddMinutes(x.Movie.RunningTime + 30) >= screening.ShowTime.AddMinutes(movie.RunningTime + 30)
            || x.ShowTime <= screening.ShowTime.AddMinutes(movie.RunningTime + 30) && x.ShowTime.AddMinutes(x.Movie.RunningTime + 30) >= screening.ShowTime.AddMinutes(movie.RunningTime + 30)));
            return !check;
        }
    }
}