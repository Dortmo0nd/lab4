using System;
using Places.Abstract.UnitOfWork;
using Places.Abstract.Repository;

namespace Places.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly PlacesDbContext _context;
        private bool _disposed = false;

        public UnitOfWork(PlacesDbContext context)
        {
            _context = context;
        }

        public IRepository<T> GetRepository<T>() where T : class
        {
            return new GenericRepository<T>(_context);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}