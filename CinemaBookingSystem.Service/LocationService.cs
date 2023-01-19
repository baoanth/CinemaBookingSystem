using CinemaBookingSystem.Data.Infrastructure;
using CinemaBookingSystem.Data.Repositories;
using CinemaBookingSystem.Model.Models;

namespace CinemaBookingSystem.Service
{
    public interface ILocationService
    {
        void Add(Location location);

        void Update(Location location);

        void Delete(int id);

        IEnumerable<Location> GetAll();

        Location GetById(int id);

        void SaveChanges();
    }

    public class LocationService : ILocationService
    {
        private ILocationRepository _locationRepository;
        private IUnitOfWork _unitOfWork;

        public LocationService(ILocationRepository locationRepository, IUnitOfWork unitOfWork)
        {
            _locationRepository = locationRepository;
            _unitOfWork = unitOfWork;
        }

        public void Add(Location location)
        {
            _locationRepository.Add(location);
        }

        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }

        public void Delete(int id)
        {
            _locationRepository.Delete(id);
        }

        public IEnumerable<Location> GetAll()
        {
            return _locationRepository.GetAll();
        }

        public Location GetById(int id)
        {
            return _locationRepository.GetSingleById(id);
        }

        public void Update(Location location)
        {
            _locationRepository.Update(location);
        }
    }
}