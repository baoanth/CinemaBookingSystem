using CinemaBookingSystem.Data.Infrastructure;
using CinemaBookingSystem.Data.Repositories;
using CinemaBookingSystem.Model.Models;

namespace CinemaBookingSystem.Service
{
    public interface ISupportOnlineService
    {
        void Add(SupportOnline supportOnline);

        void Update(SupportOnline supportOnline);

        void Delete(int id);

        IEnumerable<SupportOnline> GetAll();

        SupportOnline GetByID(int id);

        void SaveChanges();
    }

    public class SupportOnlineService : ISupportOnlineService
    {
        private ISupportOnlineRepository _supportOnlineRepository;
        private IUnitOfWork _unitOfWork;

        public SupportOnlineService(ISupportOnlineRepository supportOnlineRepository, IUnitOfWork unitOfWork)
        {
            _supportOnlineRepository = supportOnlineRepository;
            _unitOfWork = unitOfWork;
        }

        public void Add(SupportOnline supportOnline)
        {
            _supportOnlineRepository.Add(supportOnline);
        }

        public void Delete(int id)
        {
            _supportOnlineRepository.Delete(id);
        }

        public IEnumerable<SupportOnline> GetAll()
        {
            return _supportOnlineRepository.GetAll();
        }

        public SupportOnline GetByID(int id)
        {
            return _supportOnlineRepository.GetSingleById(id);
        }

        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }

        public void Update(SupportOnline supportOnline)
        {
            _supportOnlineRepository.Update(supportOnline);
        }
    }
}