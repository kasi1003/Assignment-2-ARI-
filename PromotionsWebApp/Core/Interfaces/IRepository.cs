using PromotionsWebApp.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PromotionsWebApp.Core.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> AllIncluding(params Expression<Func<T, object>>[] includeProperties);
        Task<IEnumerable<T>> GetAll();
        Task<IEnumerable<T>> GetAllDeleted();
        Task<T> GetSingle(int Id);
        Task<T> GetSingle(Expression<Func<T, bool>> predicate);
        Task<T> GetSingle(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);
        Task<IEnumerable<T>> FindBy(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> FindByIncluding(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);
        Task<int> Add(T entity);
        Task AddRange(IEnumerable<T> entities);
        Task Update(T entity);
        Task Delete(int id);

    }
}
