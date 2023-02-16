using CinemaBookingSystem.Data.Infrastructure;
using CinemaBookingSystem.Model.Models;
using Microsoft.Identity.Client;

namespace CinemaBookingSystem.Data.Repositories
{
    public interface IMovieRepository : IRepository<Movie>
    {
    }
    public class MovieRepository : RepositoryBase<Movie>, IMovieRepository
    {
        public MovieRepository(IDbFactory dbFactory) : base(dbFactory)
        {
            
        }
    }
}
