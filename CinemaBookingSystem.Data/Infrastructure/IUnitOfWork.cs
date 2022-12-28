namespace CinemaBookingSystem.Data.Infrastructure
{
    public interface IUnitOfWork
    {
        void Commit();
    }
}