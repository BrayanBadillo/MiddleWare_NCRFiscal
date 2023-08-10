using System.Linq.Expressions;


namespace NCRFiscalManager.Core.Interfaces.Repositories
{
    public interface IRepositoryAsync<T>
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(long id);
        Task<T> CreateAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<IEnumerable<T>> GetByFilter(Expression<Func<T, bool>> predicate);
    }
}
