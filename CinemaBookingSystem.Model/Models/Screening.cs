using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaBookingSystem.Model.Models
{
    [Table("Screening")]
    public class Screening
    {
        [Key]
        public int ScreeningID { get; set; }
        [Required]
        public DateOnly ShowDate { get; set; }
        [Required]
        public DateTime ShowTime { get; set; }
        [Required]
        public bool ShowStatus { get; set; }
        [Required]
        public bool IsFull { get; set; }
        [Required]
        public int EmptySeats { get; set; }
        [Required]
        public int TheatreID { get; set; }
        [Required]
        public int MovieID { get; set; }
        [ForeignKey("TheatreID")]
        public virtual Theatre Theatre { get; set; }
        [ForeignKey("MovieID")]
        public virtual Movie Movie { get; set; }
    }
}
