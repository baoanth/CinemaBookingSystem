using CinemaBookingSystem.Data.Infrastructure;
using CinemaBookingSystem.Model.Models;

namespace CinemaBookingSystem.Data.Repositories
{
    public interface ICarouselRepository : IRepository<Carousel>
    {

    }
    public class CarouselRepository : RepositoryBase<Carousel>, ICarouselRepository
    {
        public CarouselRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }
    }
}
