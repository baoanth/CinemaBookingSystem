using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaBookingSystem.Model.Models
{
    [Table("Roles")]
    public class Role
    {
        public Role()
        {
            Users = new HashSet<User>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RoleID { get; set; }
        [Required]
        [MaxLength(100)]
        public string RoleName { get; set; }
        [MaxLength(100)]
        public string? RoleDescription { get; set; }
        public IEnumerable<User> Users { get; set; }
    }
}
