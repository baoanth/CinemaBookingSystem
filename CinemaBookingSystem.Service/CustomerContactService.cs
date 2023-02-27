using CinemaBookingSystem.Data.Infrastructure;
using CinemaBookingSystem.Data.Repositories;
using CinemaBookingSystem.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaBookingSystem.Service
{
    public interface ICustomerContactService
    {
        void Add(CustomerContact customerContact);

        void Update(CustomerContact customerContact);

        void Delete(int id);

        IEnumerable<CustomerContact> GetAll();

        CustomerContact GetById(int id);

        void SaveChanges();
    }

    public class CustomerContactService : ICustomerContactService
    {
        private ICustomerContactRepository _customerContactRepository;
        private IUnitOfWork _unitOfWork;

        public CustomerContactService(ICustomerContactRepository customerContactRepository, IUnitOfWork unitOfWork)
        {
            _customerContactRepository = customerContactRepository;
            _unitOfWork = unitOfWork;
        }

        public void Add(CustomerContact customerContact)
        {
            _customerContactRepository.Add(customerContact);
        }

        public void Delete(int id)
        {
            _customerContactRepository.Delete(id);
        }

        public IEnumerable<CustomerContact> GetAll()
        {
            return _customerContactRepository.GetAll();
        }

        public CustomerContact GetById(int id)
        {
            return _customerContactRepository.GetSingleById(id);
        }

        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }

        public void Update(CustomerContact customerContact)
        {
            _customerContactRepository.Update(customerContact);
        }
    }
}
