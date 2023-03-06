using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaBookingSystem.ViewModels
{
    public class TopCustomerViewModel
    {
        public int UserId { get; set; }
        public string Fullname { get; set; }
        public int TotalSell { get; set; }
        public int TotalPrice { get; set; }
    }
}
