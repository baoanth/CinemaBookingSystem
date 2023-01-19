using CinemaBookingSystem.Data.Infrastructure;
using CinemaBookingSystem.Data.Repositories;
using CinemaBookingSystem.Model.Models;

namespace CinemaBookingSystem.Service
{
    public interface ICommentService
    {
        void Add(Comment comment);

        void Update(Comment comment);

        void Delete(int id);

        IEnumerable<Comment> GetAllByMovie(int movieId);

        Comment GetById(int id);

        void SaveChanges();
    }

    public class CommentService : ICommentService
    {
        private ICommentRepository _commentRepository;
        private IUnitOfWork _unitOfWork;

        public CommentService(ICommentRepository commentRepository, IUnitOfWork unitOfWork)
        {
            _commentRepository = commentRepository;
            _unitOfWork = unitOfWork;
        }

        public void Add(Comment comment)
        {
            _commentRepository.Add(comment);
        }

        public void Delete(int id)
        {
            _commentRepository.Delete(id);
        }

        public IEnumerable<Comment> GetAll()
        {
            return _commentRepository.GetAll();
        }

        public IEnumerable<Comment> GetAllByMovie(int movieId)
        {
            return _commentRepository.GetAllByMovie(movieId);
        }

        public Comment GetById(int id)
        {
            return _commentRepository.GetSingleById(id);
        }

        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }

        public void Update(Comment comment)
        {
            _commentRepository.Update(comment);
        }
    }
}