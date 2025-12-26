using System.Linq.Expressions;

namespace WootchatCRM.Core.Interfaces;

public interface IRepository<T> where T : class
{
   // ═══════════════ Query ═══════════════
   Task<T?> GetByIdAsync(int id);
   Task<T?> GetByIdAsync(long id);
   Task<IEnumerable<T>> GetAllAsync();
   Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
   Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
   Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
   Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null);

   // ═══════════════ Command ═══════════════
   Task<T> AddAsync(T entity);
   Task AddRangeAsync(IEnumerable<T> entities);
   Task UpdateAsync(T entity);
   Task UpdateRangeAsync(IEnumerable<T> entities);
   Task DeleteAsync(T entity);
   Task DeleteRangeAsync(IEnumerable<T> entities);

   // ═══════════════ Advanced ═══════════════
   IQueryable<T> Query();
   Task<int> SaveChangesAsync();
}
