using CinemaBookingSystem.Data.Infrastructure;
using CinemaBookingSystem.Data.Repositories;
using CinemaBookingSystem.Model.Models;

namespace CinemaBookingSystem.Service
{
    public interface IArticleService
    {
        void Add(Article article);

        void Update(Article article);

        void Delete(int id);

        IEnumerable<Article> GetAll();

        Article GetById(int id);

        void SaveChanges();
    }

    public class ArticleService : IArticleService
    {
        private IArticleRepository _articleRepository;
        private IUnitOfWork _unitOfWork;

        public ArticleService(IArticleRepository articleRepository, IUnitOfWork unitOfWork)
        {
            _articleRepository = articleRepository;
            _unitOfWork = unitOfWork;
        }

        public void Add(Article article)
        {
            _articleRepository.Add(article);
        }

        public void Delete(int id)
        {
            _articleRepository.Delete(id);
        }

        public IEnumerable<Article> GetAll()
        {
            return _articleRepository.GetAll();
        }

        public Article GetById(int id)
        {
            return _articleRepository.GetSingleById(id);
        }

        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }

        public void Update(Article article)
        {
            _articleRepository.Update(article);
        }
    }
}