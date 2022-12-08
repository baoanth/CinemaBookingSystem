namespace CinemaBookingSystem.Data.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbFactory dbFactory;
        private CinemaBookingSystemDbContext dbContext;
        public UnitOfWork()
        {
            this.dbFactory = dbFactory;
        }
        public CinemaBookingSystemDbContext DbContext
        {
            get { return dbContext ?? (dbContext = dbFactory.Init()); }
        }
        public void Commit()
        {
            DbContext.SaveChanges();
        }
    }
}
