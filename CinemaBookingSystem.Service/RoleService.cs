using CinemaBookingSystem.Data.Infrastructure;
using CinemaBookingSystem.Data.Repositories;
using CinemaBookingSystem.Model.Models;

namespace CinemaBookingSystem.Service
{
    public interface IRoleService
    {
        void Add(Role role);

        void Update(Role role);

        void Delete(int id);

        IEnumerable<Role> GetAll();

        Role GetById(int id);

        void SaveChanges();
    }

    public class RoleService : IRoleService
    {
        private IRoleRepository _roleRepository;
        private IUnitOfWork _unitOfWork;

        public RoleService(IRoleRepository roleRepository, IUnitOfWork unitOfWork)
        {
            _roleRepository = roleRepository;
            _unitOfWork = unitOfWork;
        }

        public void Add(Role role)
        {
            _roleRepository.Add(role);
        }

        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }

        public void Delete(int id)
        {
            _roleRepository.Delete(id);
        }

        public IEnumerable<Role> GetAll()
        {
            return _roleRepository.GetAll();
        }

        public Role GetById(int id)
        {
            return _roleRepository.GetSingleById(id);
        }

        public void Update(Role role)
        {
            _roleRepository.Update(role);
        }
    }
}