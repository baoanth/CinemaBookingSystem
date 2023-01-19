using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaBookingSystem.Model.Models
{
    [Table("Errors")]
    public class Error
    {
        [Key]
        public int ErrorId { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}