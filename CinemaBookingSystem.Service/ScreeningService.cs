using CinemaBookingSystem.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaBookingSystem.Service
{
    public interface IScreeningService
    {

    }
    public class ScreeningService
    {
        IScreeningService _screeningService;
        IUnitOfWork _unitOfWork;
        public ScreeningService(IScreeningService screeningService, IUnitOfWork unitOfWork)
        {
            _screeningService = screeningService;
            _unitOfWork = unitOfWork;
        }
    }
}
