using Places.Models;

namespace Places.Abstract
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Place> PlaceRepository { get; }
        IRepository<User> UserRepository { get; }
        IRepository<Role> RoleRepository { get; }
        IRepository<Review> ReviewRepository { get; }
        IRepository<Question> QuestionRepository { get; }
        IRepository<Media> MediaRepository { get; }
        IRepository<Answer> AnswerRepository { get; }
        void SaveChanges();
    }
}