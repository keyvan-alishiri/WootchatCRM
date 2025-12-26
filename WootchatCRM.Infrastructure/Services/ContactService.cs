using Microsoft.EntityFrameworkCore;
using WootchatCRM.Core.Entities;
using WootchatCRM.Core.Interfaces;
using WootchatCRM.Core.Interfaces.Services;

namespace WootchatCRM.Infrastructure.Services;

public class ContactService : IContactService
{
   private readonly IRepository<Contact> _contactRepository;
   private readonly IRepository<Tag> _tagRepository;
   private readonly IRepository<ContactTag> _contactTagRepository;

   public ContactService(
       IRepository<Contact> contactRepository,
       IRepository<Tag> tagRepository,
       IRepository<ContactTag> contactTagRepository)
   {
      _contactRepository = contactRepository;
      _tagRepository = tagRepository;
      _contactTagRepository = contactTagRepository;
   }

   #region ═══════════════════ Query Methods ═══════════════════

   public async Task<IEnumerable<Contact>> GetAllAsync()
   {
      return await _contactRepository.Query()
          .Include(c => c.ContactTags)
              .ThenInclude(ct => ct.Tag)
          .OrderBy(c => c.Name)
          .ToListAsync();
   }

   public async Task<Contact?> GetByIdAsync(int id)
   {
      return await _contactRepository.Query()
          .Include(c => c.ContactTags)
              .ThenInclude(ct => ct.Tag)
          .Include(c => c.Conversations)
          .FirstOrDefaultAsync(c => c.Id == id);
   }

   public async Task<Contact?> GetByChatwootIdAsync(long chatwootContactId)
   {
      return await _contactRepository.Query()
          .Include(c => c.ContactTags)
              .ThenInclude(ct => ct.Tag)
          .FirstOrDefaultAsync(c => c.ChatwootContactId == chatwootContactId);
   }

   public async Task<IEnumerable<Contact>> SearchAsync(string searchTerm)
   {
      if (string.IsNullOrWhiteSpace(searchTerm))
         return Enumerable.Empty<Contact>();

      var lowerSearchTerm = searchTerm.ToLower();

      return await _contactRepository.Query()
          .Where(c => (c.Name != null && c.Name.ToLower().Contains(lowerSearchTerm)) ||
                      (c.Email != null && c.Email.ToLower().Contains(lowerSearchTerm)) ||
                      (c.PhoneNumber != null && c.PhoneNumber.Contains(searchTerm)))
          .Include(c => c.ContactTags)
              .ThenInclude(ct => ct.Tag)
          .OrderBy(c => c.Name)
          .Take(50) // محدودیت برای جلوگیری از کوئری سنگین
          .ToListAsync();
   }

   public async Task<IEnumerable<Contact>> GetByTagAsync(int tagId)
   {
      return await _contactTagRepository.Query()
          .Where(ct => ct.TagId == tagId)
          .Include(ct => ct.Contact)
              .ThenInclude(c => c.ContactTags)
                  .ThenInclude(ct => ct.Tag)
          .Select(ct => ct.Contact)
          .OrderBy(c => c.Name)
          .ToListAsync();
   }

   #endregion

   #region ═══════════════════ Command Methods ═══════════════════

   public async Task<Contact> CreateAsync(Contact contact)
   {
      // بررسی تکراری نبودن ChatwootContactId
      if (contact.ChatwootContactId.HasValue)
      {
         var exists = await _contactRepository
             .AnyAsync(c => c.ChatwootContactId == contact.ChatwootContactId);

         if (exists)
            throw new InvalidOperationException($"Contact with Chatwoot ID {contact.ChatwootContactId} already exists.");
      }

      contact.CreatedAt = DateTime.UtcNow;

      return await _contactRepository.AddAsync(contact);
   }

   public async Task<Contact> UpdateAsync(Contact contact)
   {
      var existing = await _contactRepository.GetByIdAsync(contact.Id);

      if (existing == null)
         throw new InvalidOperationException($"Contact with ID {contact.Id} not found.");

      // به‌روزرسانی فیلدها
      existing.Name = contact.Name;
      existing.Email = contact.Email;
      existing.PhoneNumber = contact.PhoneNumber;
      existing.UpdatedAt = DateTime.UtcNow;

      await _contactRepository.UpdateAsync(existing);

      return existing;
   }

   public async Task DeleteAsync(int id)
   {
      var contact = await _contactRepository.Query()
          .Include(c => c.ContactTags)
          .FirstOrDefaultAsync(c => c.Id == id);

      if (contact != null)
      {
         // حذف تگ‌های مرتبط
         if (contact.ContactTags.Any())
         {
            await _contactTagRepository.DeleteRangeAsync(contact.ContactTags);
         }

         await _contactRepository.DeleteAsync(contact);
      }
   }

   #endregion

   #region ═══════════════════ Tag Management ═══════════════════

   public async Task AddTagAsync(int contactId, int tagId)
   {
      // بررسی وجود مخاطب
      var contactExists = await _contactRepository.AnyAsync(c => c.Id == contactId);
      if (!contactExists)
         throw new InvalidOperationException($"Contact with ID {contactId} not found.");

      // بررسی وجود تگ
      var tagExists = await _tagRepository.AnyAsync(t => t.Id == tagId);
      if (!tagExists)
         throw new InvalidOperationException($"Tag with ID {tagId} not found.");

      // بررسی عدم تکرار
      var alreadyExists = await _contactTagRepository
          .AnyAsync(ct => ct.ContactId == contactId && ct.TagId == tagId);

      if (!alreadyExists)
      {
         await _contactTagRepository.AddAsync(new ContactTag
         {
            ContactId = contactId,
            TagId = tagId
         });
      }
   }

   public async Task RemoveTagAsync(int contactId, int tagId)
   {
      var contactTag = await _contactTagRepository
          .FirstOrDefaultAsync(ct => ct.ContactId == contactId && ct.TagId == tagId);

      if (contactTag != null)
      {
         await _contactTagRepository.DeleteAsync(contactTag);
      }
   }

   public async Task<IEnumerable<Tag>> GetContactTagsAsync(int contactId)
   {
      var tags = await _contactTagRepository.Query()
          .Where(ct => ct.ContactId == contactId)
          .Include(ct => ct.Tag)
          .Select(ct => ct.Tag)
          .OrderBy(t => t.Name)
          .ToListAsync();

      return tags;
   }

   #endregion
}
