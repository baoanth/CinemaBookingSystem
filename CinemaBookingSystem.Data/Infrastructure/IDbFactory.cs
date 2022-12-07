namespace CinemaBookingSystem.Data.Infrastructure
{
    public interface IDbFactory : IDisposable
    {
        CinemaBookingSystemDbContext Init();
    }
}
