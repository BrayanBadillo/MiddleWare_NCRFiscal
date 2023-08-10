using Microsoft.EntityFrameworkCore;
using NCRFiscalManager.Core.Entities;
using NCRFiscalManager.Core.Interfaces.Repositories;
using NCRFiscalManager.Infraestructure.Repositories.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NCRFiscalManager.Infraestructure.Repositories
{
    public class RepositoryAsync<T> : IRepositoryAsync<T> where T : BaseEntity, new()
    {
        private readonly NCRFiscalContext _ncrFiscalcontext;
        internal DbSet<T> dbSet;
        public RepositoryAsync( NCRFiscalContext ncrFiscalContext)
        {
            _ncrFiscalcontext = ncrFiscalContext;
            this.dbSet = _ncrFiscalcontext.Set<T>();
        }

        public async virtual Task<T> CreateAsync(T entity)
        {
            await dbSet.AddAsync(entity);
            await _ncrFiscalcontext.SaveChangesAsync();
            return entity;
        }

        public async virtual Task DeleteAsync(T entity)
        {
            dbSet.Remove(entity);
            await _ncrFiscalcontext.SaveChangesAsync(); 
        }

        public async virtual Task<IEnumerable<T>> GetAllAsync()
        {
            return await dbSet.ToListAsync();
        }

        public async virtual Task<IEnumerable<T>> GetByFilter(Expression<Func<T, bool>> predicate)
        {
            return await dbSet
                .Where(predicate)
                .ToListAsync();
        }

        public async virtual Task<T> GetByIdAsync(long id)
        {
            return await dbSet.FindAsync(id);
        }

        public async Task<T> UpdateAsync(T entity)
        {
            _ncrFiscalcontext.Entry(entity).State = EntityState.Modified;
            await _ncrFiscalcontext.SaveChangesAsync();
            return entity;
        }
    }
}
