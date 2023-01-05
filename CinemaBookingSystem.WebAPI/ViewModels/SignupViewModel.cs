using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CinemaBookingSystem.WebAPI.ViewModels
{
    public class SignupViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string EmailAddress { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public int RoleID { get; set; }
    }
}
