using CinemaBookingSystem.Data.Infrastructure;
using CinemaBookingSystem.Model.Models;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;

namespace CinemaBookingSystem.Data.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        bool Login(string username, string password);
        bool UsernameCheck(string username);
        string PasswordHashing(string password);
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
            bool IsValid = (user != null && user.Password == PasswordHashing(password));
            return IsValid;
        }

        public bool UsernameCheck(string username)
        {
            bool isValid = true;
            var user = DbContext.Users.Where(x => x.Username == username).FirstOrDefault();
            if (user == null) return isValid;
            else isValid = false;
            return isValid;
        }
    }
}
