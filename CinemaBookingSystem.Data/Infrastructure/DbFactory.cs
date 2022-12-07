namespace CinemaBookingSystem.Data.Infrastructure
{
    public class DbFactory : Disposable, IDbFactory
    {
        CinemaBookingSystemDbContext dbContext;
        public CinemaBookingSystemDbContext Init()
        {
            return dbContext ?? (dbContext = new CinemaBookingSystemDbContext());
        }
        protected override void DisposeCore()
        {
            if (dbContext != null)
            {
                dbContext.Dispose();
            }
        }
    }
}
