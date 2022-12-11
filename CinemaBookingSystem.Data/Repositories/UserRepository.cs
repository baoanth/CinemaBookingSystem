using CinemaBookingSystem.Data.Infrastructure;
using CinemaBookingSystem.Model.Models;
using System.Security.Cryptography;
using System.Text;

namespace CinemaBookingSystem.Data.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        bool Login(string username, string password);
        bool Signup(User user);
    }
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(IDbFactory dbFactory) : base(dbFactory)
        {
            
        }
        [Obsolete]
        public string PasswordHashing(string password)
        {
            SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();
            byte[] passwordBytes = Encoding.ASCII.GetBytes(password);
            byte[] encryptedBytes = sha1.ComputeHash(passwordBytes);
            return Convert.ToBase64String(encryptedBytes);
        }

        [Obsolete]
        public bool Login(string username, string password)
        {
            var user = DbContext.Users.Where(x => x.Username == username).FirstOrDefault();
            bool validUser = user != null && user.Password == PasswordHashing(password);
            if (validUser) return true;
            else return false;
        }

        public bool Signup(User user)
        {
            throw new NotImplementedException();
        }
    }
}
