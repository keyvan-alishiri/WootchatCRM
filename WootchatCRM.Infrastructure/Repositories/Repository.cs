using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using WootchatCRM.Core.Interfaces;
using WootchatCRM.Infrastructure.Data;

namespace WootchatCRM.Infrastructure.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
   protected readonly AppDbContext _context;
   protected readonly DbSet<T> _dbSet;

   public Repository(AppDbContext context)
   {
      _context = context;
      _dbSet = context.Set<T>();
   }

   #region ═══════════════ Query ═══════════════

   public async Task<T?> GetByIdAsync(int id)
   {
      return await _dbSet.FindAsync(id);
   }

   public async Task<T?> GetByIdAsync(long id)
   {
      return await _dbSet.FindAsync(id);
   }

   public async Task<IEnumerable<T>> GetAllAsync()
   {
      return await _dbSet.ToListAsync();
   }

   public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
   {
      return await _dbSet.Where(predicate).ToListAsync();
   }

   public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
   {
      return await _dbSet.FirstOrDefaultAsync(predicate);
   }

   public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
   {
      return await _dbSet.AnyAsync(predicate);
   }

   public async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null)
   {
      if (predicate == null)
         return await _dbSet.CountAsync();

      return await _dbSet.CountAsync(predicate);
   }

   #endregion

   #region ═══════════════ Command ═══════════════

   public async Task<T> AddAsync(T entity)
   {
      var entry = await _dbSet.AddAsync(entity);
      await SaveChangesAsync();
      return entry.Entity;
   }

   public async Task AddRangeAsync(IEnumerable<T> entities)
   {
      await _dbSet.AddRangeAsync(entities);
      await SaveChangesAsync();
   }

   public async Task UpdateAsync(T entity)
   {
      _dbSet.Update(entity);
      await SaveChangesAsync();
   }

   public async Task UpdateRangeAsync(IEnumerable<T> entities)
   {
      _dbSet.UpdateRange(entities);
      await SaveChangesAsync();
   }

   public async Task DeleteAsync(T entity)
   {
      _dbSet.Remove(entity);
      await SaveChangesAsync();
   }

   public async Task DeleteRangeAsync(IEnumerable<T> entities)
   {
      _dbSet.RemoveRange(entities);
      await SaveChangesAsync();
   }

   #endregion

   #region ═══════════════ Advanced ═══════════════

   public IQueryable<T> Query()
   {
      return _dbSet.AsQueryable();
   }

   public async Task<int> SaveChangesAsync()
   {
      return await _context.SaveChangesAsync();
   }

   #endregion
}
