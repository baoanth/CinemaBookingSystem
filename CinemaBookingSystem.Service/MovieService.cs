using CinemaBookingSystem.Data.Infrastructure;
using CinemaBookingSystem.Data.Repositories;
using CinemaBookingSystem.Model.Models;

namespace CinemaBookingSystem.Service
{
    public interface IMovieService
    {
        void Add(Movie movie);

        void Update(Movie movie);

        void Delete(int id);

        IEnumerable<Movie> GetAll();

        Movie GetById(int id);

        void SaveChanges();
    }

    public class MovieService : IMovieService
    {
        private IMovieRepository _movieRepository;
        private IUnitOfWork _unitOfWork;

        public MovieService(IMovieRepository movieRepository, IUnitOfWork unitOfWork)
        {
            _movieRepository = movieRepository;
            _unitOfWork = unitOfWork;
        }

        public void Add(Movie movie)
        {
            _movieRepository.Add(movie);
        }

        public void Delete(int id)
        {
            _movieRepository.Delete(id);
        }

        public IEnumerable<Movie> GetAll()
        {
            return _movieRepository.GetAll();
        }

        public Movie GetById(int id)
        {
            return _movieRepository.GetSingleById(id);
        }

        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }

        public void Update(Movie movie)
        {
            _movieRepository.Update(movie);
        }
    }
}