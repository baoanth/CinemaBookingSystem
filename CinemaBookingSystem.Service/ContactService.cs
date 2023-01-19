using CinemaBookingSystem.Data.Infrastructure;
using CinemaBookingSystem.Data.Repositories;
using CinemaBookingSystem.Model.Models;

namespace CinemaBookingSystem.Service
{
    public interface IContactService
    {
        void Add(Contact contact);

        void Update(Contact contact);

        void Delete(int id);

        IEnumerable<Contact> GetAll();

        Contact GetById(int id);

        void SaveChanges();
    }

    public class ContactService : IContactService
    {
        private ISupportOnlineRepository _contactRepository;
        private IUnitOfWork _unitOfWork;

        public ContactService(ISupportOnlineRepository contactRepository, IUnitOfWork unitOfWork)
        {
            _contactRepository = contactRepository;
            _unitOfWork = unitOfWork;
        }

        public void Add(Contact contact)
        {
            _contactRepository.Add(contact);
        }

        public void Delete(int id)
        {
            _contactRepository.Delete(id);
        }

        public IEnumerable<Contact> GetAll()
        {
            return _contactRepository.GetAll();
        }

        public Contact GetById(int id)
        {
            return _contactRepository.GetSingleById(id);
        }

        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }

        public void Update(Contact contact)
        {
            _contactRepository.Update(contact);
        }
    }
}