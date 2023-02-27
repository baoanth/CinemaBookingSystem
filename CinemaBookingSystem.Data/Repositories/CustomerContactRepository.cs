using CinemaBookingSystem.Data.Infrastructure;
using CinemaBookingSystem.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaBookingSystem.Data.Repositories
{
    public interface ICustomerContactRepository : IRepository<CustomerContact>
    {
    }

    public class CustomerContactRepository : RepositoryBase<CustomerContact>, ICustomerContactRepository
    {
        public CustomerContactRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
