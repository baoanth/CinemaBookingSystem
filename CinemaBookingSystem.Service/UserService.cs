using CinemaBookingSystem.Data.Infrastructure;
using CinemaBookingSystem.Data.Repositories;
using CinemaBookingSystem.Model.Models;

namespace CinemaBookingSystem.Service
{
    public interface IUserService
    {
        void Add(User user);

        void Update(User user);

        void Delete(int id);

        IEnumerable<User> GetAll();

        User GetByID(int id);
        bool Login(string username, string password);
        bool Signup(User user);

        void SaveChanges();
    }

    public class UserService : IUserService
    {
        private IUserRepository _userRepository;
        private IUnitOfWork _unitOfWork;

        public UserService(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public void Add(User user)
        {
            _userRepository.Add(user);
        }

        public void Delete(int id)
        {
            _userRepository.Delete(id);
        }

        public IEnumerable<User> GetAll()
        {
            return _userRepository.GetAll();
        }

        public User GetByID(int id)
        {
            return _userRepository.GetSingleById(id);
        }

        public bool Login(string username, string password)
        {
            return _userRepository.Login(username,password);
        }

        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }

        public bool Signup(User user)
        {
            bool isValidUser = _userRepository.UsernameCheck(user);
            if (isValidUser)
            {
                _userRepository.Add(user);
                return true;
            }
            else return false;
            
        }

        public void Update(User user)
        {
            _userRepository.Update(user);
        }
    }
}