using CinemaBookingSystem.Model.Models;
using System.Data.Entity;

namespace CinemaBookingSystem.Data
{
    public class CinemaBookingSystemDbContext : DbContext
    {
        public CinemaBookingSystemDbContext() : base(ContextConfigurations.CONNECTION_STRING)
        {
            this.Configuration.LazyLoadingEnabled = true;
        }

        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Slide> Slides { get; set; }
        public DbSet<Cinema> Cinemas { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Screening> Screenings { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Theatre> Theatres { get; set; }
        public DbSet<ScreeningPosition> ScreeningPositions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<VisitorStatistic> VisitorStatistics { get; set; }
        public DbSet<BookingDetail> TicketBookings { get; set; }
        public DbSet<Error> Errors { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

        }
    }
}
