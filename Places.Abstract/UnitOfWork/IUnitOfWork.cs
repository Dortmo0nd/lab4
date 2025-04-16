using Places.Abstract.Repository;
namespace Places.Abstract.UnitOfWork;
public interface IUnitOfWork : IDisposable
{
    IRepository<T> GetRepository<T>() where T : class;
    void SaveChanges();
}