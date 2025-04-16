using Places.Abstract.Repository;
using Places.Models;
namespace Places.Abstract.UnitOfWork;
public interface IUnitOfWork : IDisposable
{
    IRepository<T> GetRepository<T>() where T : class;
    IRepository<User> UserRepository { get; }
    void SaveChanges();
}