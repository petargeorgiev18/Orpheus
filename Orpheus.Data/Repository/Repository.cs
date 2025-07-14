using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Orpheus.Data.Repository.Interfaces;

namespace Orpheus.Data.Repository
{
    public class Repository<TEntity, TId> : IRepository<TEntity, TId> where TEntity : class
    {
        private readonly OrpheusDbContext context;
        private readonly DbSet<TEntity> dbSet;
        public Repository(OrpheusDbContext context)
        {
            this.context = context;
            this.dbSet = this.context.Set<TEntity>();
        }
        public async Task AddAsync(TEntity item)
        {
            await this.context.AddAsync(item);
            await this.context.SaveChangesAsync();
        }

        public async Task DeleteAsync(TId id)
        {
            TEntity? item = await this.GetByIdAsync(id);
            if (item == null)
            {
                throw new ArgumentException($"Entity with id {id} not found.");
            }
            this.dbSet.Remove(item);
            await this.context.SaveChangesAsync();
        }

        public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await this.dbSet.FirstOrDefaultAsync(predicate);
        }

        public IQueryable<TEntity> GetAllAsNoTracking()
        {
            return this.dbSet.AsNoTracking();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await this.dbSet.ToListAsync();
        }

        public IQueryable<TEntity> GetAllTracked()
        {
            return this.dbSet.AsQueryable();
        }

        public async Task<TEntity?> GetByIdAsync(TId id)
        {
            return await this.dbSet.FindAsync(id);
        }

        public async Task SaveChangesAsync()
        {
            await this.context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TEntity item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item), "Entity cannot be null.");
            }
            this.context.Entry(item).State = EntityState.Modified;
            await this.context.SaveChangesAsync();
        }
    }
}
