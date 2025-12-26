using WootchatCRM.Core.Entities;

namespace WootchatCRM.Core.Interfaces.Services;

public interface IContactService
{
   // ─────────────── Query ───────────────
   Task<IEnumerable<Contact>> GetAllAsync();
   Task<Contact?> GetByIdAsync(int id);
   Task<Contact?> GetByChatwootIdAsync(long chatwootContactId);
   Task<IEnumerable<Contact>> SearchAsync(string searchTerm);
   Task<IEnumerable<Contact>> GetByTagAsync(int tagId);

   // ─────────────── Command ───────────────
   Task<Contact> CreateAsync(Contact contact);
   Task<Contact> UpdateAsync(Contact contact);
   Task DeleteAsync(int id);

   // ─────────────── Tags ───────────────
   Task AddTagAsync(int contactId, int tagId);
   Task RemoveTagAsync(int contactId, int tagId);
   Task<IEnumerable<Tag>> GetContactTagsAsync(int contactId);
}
