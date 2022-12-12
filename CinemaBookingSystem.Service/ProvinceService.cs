using CinemaBookingSystem.Data.Infrastructure;
using CinemaBookingSystem.Data.Repositories;
using CinemaBookingSystem.Model.Models;

namespace CinemaBookingSystem.Service
{
    public interface IProvinceService
    {
        void Add(Province province);

        void Update(Province province);

        void Delete(int id);

        IEnumerable<Province> GetAll();

        IEnumerable<Province> GetByRegion(string region);

        Province GetByID(int id);

        void SaveChanges();
    }

    public class ProvinceService : IProvinceService
    {
        private IProvinceRepository _provinceRepository;
        private IUnitOfWork _unitOfWork;

        public ProvinceService(IProvinceRepository provinceRepository, IUnitOfWork unitOfWork)
        {
            _provinceRepository = provinceRepository;
            _unitOfWork = unitOfWork;
        }

        public void Add(Province province)
        {
            _provinceRepository.Add(province);
        }

        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }

        public void Delete(int id)
        {
            _provinceRepository.Delete(id);
        }

        public IEnumerable<Province> GetAll()
        {
            return _provinceRepository.GetAll();
        }

        public Province GetByID(int id)
        {
            return _provinceRepository.GetSingleById(id);
        }

        public void Update(Province province)
        {
            _provinceRepository.Update(province);
        }

        public IEnumerable<Province> GetByRegion(string region)
        {
            return _provinceRepository.GetByRegion(region);
        }
    }
}