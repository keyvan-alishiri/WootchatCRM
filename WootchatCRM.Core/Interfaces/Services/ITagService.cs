using WootchatCRM.Core.Entities;

namespace WootchatCRM.Core.Interfaces.Services;

public interface ITagService
{
   Task<IEnumerable<Tag>> GetAllAsync();
   Task<Tag?> GetByIdAsync(int id);
   Task<Tag?> GetByNameAsync(string name);
   Task<Tag> CreateAsync(Tag tag);
   Task<Tag> UpdateAsync(Tag tag);
   Task DeleteAsync(int id);
}
