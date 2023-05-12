using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace InnoGotchi_backend.Repositories.Abstract
{
    public interface IRepositoryBase<T>
    {
        Task<IQueryable<T>> GetAll(bool trackChanges);
        Task<IQueryable<T>> GetByCondition(Expression<Func<T, bool>> expression, bool trackChanges);
        Task Create(T entity);
        Task Update(T entity);
        Task Delete(T entity);
    }
}
