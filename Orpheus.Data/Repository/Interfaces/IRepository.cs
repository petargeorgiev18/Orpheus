using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Orpheus.Data.Repository.Interfaces
{
    public interface IRepository<TEntity, TId> where TEntity : class
    {
        Task<TEntity?> GetByIdAsync(TId id);
        Task<IEnumerable<TEntity>> GetAllAsync();
        IQueryable<TEntity> GetAllTracked();
        IQueryable<TEntity> GetAllAsNoTracking();
        Task AddAsync(TEntity entity);
        Task AddWithoutSavingAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(TId id);
        Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
        void RemoveRange(IEnumerable<TEntity> items);
        void Remove(TEntity item);
        Task SaveChangesAsync();
    }
}
