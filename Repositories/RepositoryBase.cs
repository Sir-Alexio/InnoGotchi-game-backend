using InnoGotchi_backend.DataContext;
using InnoGotchi_backend.Services;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace InnoGotchi_backend.Repositories
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        private readonly ApplicationContext _db;

        public RepositoryBase(ApplicationContext db)
        {
            _db = db;
        }
        public void Create(T entity)
        {
            _db.Set<T>().Add(entity);
        }

        public void Delete(T entity)
        {
            _db.Set<T>().Remove(entity);
        }

        public IQueryable<T> GetAll(bool trackChanges)
        {
            if (!trackChanges)
            {
                return _db.Set<T>().AsNoTracking();
            }
            else
            {
                return _db.Set<T>();
            }
        }

        public IQueryable<T> GetByCondition(Expression<Func<T, bool>> expression, bool trackChanges)
        {
            if (!trackChanges)
            {
                return _db.Set<T>().Where(expression).AsNoTracking();
            }
            else
            {
                return _db.Set<T>().Where(expression);
            }
        }

        public void Update(T entity)
        {
            _db.Set<T>().Update(entity);
        }
    }
}
