using Places.Abstract;
using Places.DAL.Repositories;
using Places.Models;

namespace Places.DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly PlacesDbContext _context;
        private IRepository<Place> _placeRepository;
        private IRepository<User> _userRepository;
        private IRepository<Role> _roleRepository;
        private IRepository<Review> _reviewRepository;
        private IRepository<Question> _questionRepository;
        private IRepository<Media> _mediaRepository;
        private IRepository<Answer> _answerRepository;

        public UnitOfWork(PlacesDbContext context)
        {
            _context = context;
        }

        public IRepository<Place> PlaceRepository
        {
            get { return _placeRepository ??= new GenericRepository<Place>(_context); }
        }

        public IRepository<User> UserRepository
        {
            get { return _userRepository ??= new GenericRepository<User>(_context); }
        }

        public IRepository<Role> RoleRepository
        {
            get { return _roleRepository ??= new GenericRepository<Role>(_context); }
        }

        public IRepository<Review> ReviewRepository
        {
            get { return _reviewRepository ??= new GenericRepository<Review>(_context); }
        }

        public IRepository<Question> QuestionRepository
        {
            get { return _questionRepository ??= new GenericRepository<Question>(_context); }
        }

        public IRepository<Media> MediaRepository
        {
            get { return _mediaRepository ??= new GenericRepository<Media>(_context); }
        }

        public IRepository<Answer> AnswerRepository
        {
            get { return _answerRepository ??= new GenericRepository<Answer>(_context); }
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}