using CinemaBookingSystem.Data.Infrastructure;
using CinemaBookingSystem.Data.Repositories;
using CinemaBookingSystem.Model.Models;

namespace CinemaBookingSystem.Service
{
    public interface IVisitorStatisticService
    {
        IEnumerable<VisitorStatistic> GetAll();

        VisitorStatistic GetById(int id);

        IEnumerable<VisitorStatistic> GetByIPAddress(string IPAddress);

        void Delete(int id);

        void SaveChanges();
    }

    public class VisitorStatisticService : IVisitorStatisticService
    {
        private IVisitorStatisticRepository _visitorStatisticRepository;
        private IUnitOfWork _unitOfWork;

        public VisitorStatisticService(IVisitorStatisticRepository visitorStatisticRepository, IUnitOfWork unitOfWork)
        {
            _visitorStatisticRepository = visitorStatisticRepository;
            _unitOfWork = unitOfWork;
        }

        public void Delete(int id)
        {
            _visitorStatisticRepository.Delete(id);
        }

        public IEnumerable<VisitorStatistic> GetAll()
        {
            return _visitorStatisticRepository.GetAll();
        }

        public VisitorStatistic GetById(int id)
        {
            return _visitorStatisticRepository.GetSingleById(id);
        }

        public IEnumerable<VisitorStatistic> GetByIPAddress(string IPAddress)
        {
            return _visitorStatisticRepository.GetByIPAddress(IPAddress);
        }

        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }
    }
}