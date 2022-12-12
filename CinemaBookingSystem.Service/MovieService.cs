using CinemaBookingSystem.Data.Infrastructure;
using CinemaBookingSystem.Data.Repositories;

namespace CinemaBookingSystem.Service
{
    public interface IMovieService
    {
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
    }
}