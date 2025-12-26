using Microsoft.EntityFrameworkCore;
using WootchatCRM.Core.Entities;
using WootchatCRM.Core.Interfaces.Services;
using WootchatCRM.Infrastructure.Data;

namespace WootchatCRM.Infrastructure.Services;

public class TagService : ITagService
{
   private readonly AppDbContext _context;

   public TagService(AppDbContext context)
   {
      _context = context;
   }

   public async Task<IEnumerable<Tag>> GetAllAsync()
   {
      return await _context.Tags
          .OrderBy(t => t.Name)
          .ToListAsync();
   }

   public async Task<Tag?> GetByIdAsync(int id)
   {
      return await _context.Tags.FindAsync(id);
   }

   public async Task<Tag?> GetByNameAsync(string name)
   {
      return await _context.Tags
          .FirstOrDefaultAsync(t => t.Name.ToLower() == name.ToLower());
   }

   public async Task<Tag> CreateAsync(Tag tag)
   {
      _context.Tags.Add(tag);
      await _context.SaveChangesAsync();
      return tag;
   }

   public async Task<Tag> UpdateAsync(Tag tag)
   {
      _context.Tags.Update(tag);
      await _context.SaveChangesAsync();
      return tag;
   }

   public async Task DeleteAsync(int id)
   {
      var tag = await _context.Tags.FindAsync(id);
      if (tag != null)
      {
         _context.Tags.Remove(tag);
         await _context.SaveChangesAsync();
      }
   }
}
