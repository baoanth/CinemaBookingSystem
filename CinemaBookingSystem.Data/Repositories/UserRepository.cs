using CinemaBookingSystem.Data.Infrastructure;
using CinemaBookingSystem.Model.Models;
using System.Linq;
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
        IEnumerable<User> GetByRole(int roleId);
        IEnumerable<User> Search(string keyworks);
        User GetByUsername(string username);
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
            var user = DbContext.Users.Where(x => x.Username.ToLower().Trim() == username.ToLower().Trim()).FirstOrDefault();
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

        public IEnumerable<User> GetByRole(int roleId)
        {
            return DbContext.Users.Where(x => x.RoleID == roleId).ToList();
        }

        public User GetByUsername(string username)
        {
            var str = LowerTrim(username);
            return DbContext.Users.Where(x => x.Username.ToLower().Trim() == str).FirstOrDefault();
        }

        public IEnumerable<User> Search(string keyworks)
        {
            var key = LowerTrim(keyworks);
            var user = DbContext.Users.Where(x => key.Contains(x.LastName.ToLower().Trim())).ToList();
            if (user.Count() == 0)
            {
                user = DbContext.Users.Where(x => key.Contains(x.FirstName.ToLower().Trim())).ToList();
            }
            return user;
        }

        public string GetFullName(string firstName, string lastName)
        {
            return LowerTrim(firstName + lastName);
        }
        public string LowerTrim(string str)
        {
            return str.ToLower().Trim();
        }
    }
}
