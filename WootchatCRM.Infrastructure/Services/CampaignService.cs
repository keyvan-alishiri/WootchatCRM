using Microsoft.EntityFrameworkCore;
using WootchatCRM.Core.Entities;
using WootchatCRM.Core.Enums;
using WootchatCRM.Core.Interfaces;
using WootchatCRM.Core.Interfaces.Services;

namespace WootchatCRM.Infrastructure.Services;

public class CampaignService : ICampaignService
{
   private readonly IRepository<Campaign> _campaignRepository;
   private readonly IRepository<CampaignRecipient> _recipientRepository;
   private readonly IRepository<Contact> _contactRepository;
   private readonly IRepository<Tag> _tagRepository;
   private readonly IRepository<ContactTag> _contactTagRepository;

   public CampaignService(
       IRepository<Campaign> campaignRepository,
       IRepository<CampaignRecipient> recipientRepository,
       IRepository<Contact> contactRepository,
       IRepository<Tag> tagRepository,
       IRepository<ContactTag> contactTagRepository)
   {
      _campaignRepository = campaignRepository;
      _recipientRepository = recipientRepository;
      _contactRepository = contactRepository;
      _tagRepository = tagRepository;
      _contactTagRepository = contactTagRepository;
   }

   #region ═══════════════════ Query Methods ═══════════════════

   public async Task<IEnumerable<Campaign>> GetAllAsync()
   {
      return await _campaignRepository.Query()
          .Include(c => c.Recipients)
              .ThenInclude(cr => cr.Contact)
          .OrderByDescending(c => c.CreatedAt)
          .ToListAsync();
   }

   public async Task<Campaign?> GetByIdAsync(int id)
   {
      return await _campaignRepository.Query()
          .Include(c => c.Recipients)
              .ThenInclude(cr => cr.Contact)
          .FirstOrDefaultAsync(c => c.Id == id);
   }

   public async Task<IEnumerable<Campaign>> GetByStatusAsync(CampaignStatus status)
   {
      return await _campaignRepository.Query()
          .Where(c => c.Status == status)
          .Include(c => c.Recipients)
          .OrderByDescending(c => c.CreatedAt)
          .ToListAsync();
   }

   #endregion

   #region ═══════════════════ Command Methods ═══════════════════

   public async Task<Campaign> CreateAsync(Campaign campaign)
   {
      campaign.CreatedAt = DateTime.UtcNow;
      campaign.Status = CampaignStatus.Draft;

      return await _campaignRepository.AddAsync(campaign);
   }

   public async Task<Campaign> UpdateAsync(Campaign campaign)
   {
      var existing = await _campaignRepository.GetByIdAsync(campaign.Id);

      if (existing == null)
         throw new InvalidOperationException($"Campaign with ID {campaign.Id} not found.");

      existing.Name = campaign.Name;
      existing.Description = campaign.Description;
      existing.MessageTemplate = campaign.MessageTemplate;
      existing.ScheduledAt = campaign.ScheduledAt;
      existing.UpdatedAt = DateTime.UtcNow;

      await _campaignRepository.UpdateAsync(existing);

      return existing;
   }

   public async Task DeleteAsync(int id)
   {
      var campaign = await _campaignRepository.Query()
          .Include(c => c.Recipients)
          .FirstOrDefaultAsync(c => c.Id == id);

      if (campaign != null)
      {
         // حذف گیرندگان مرتبط
         if (campaign.Recipients.Any())
         {
            await _recipientRepository.DeleteRangeAsync(campaign.Recipients);
         }

         await _campaignRepository.DeleteAsync(campaign);
      }
   }

   #endregion

   #region ═══════════════════ Recipients Management ═══════════════════

   public async Task AddRecipientAsync(int campaignId, int contactId)
   {
      // بررسی وجود کمپین
      var campaignExists = await _campaignRepository.AnyAsync(c => c.Id == campaignId);
      if (!campaignExists)
         throw new InvalidOperationException($"Campaign with ID {campaignId} not found.");

      // بررسی وجود مخاطب
      var contactExists = await _contactRepository.AnyAsync(c => c.Id == contactId);
      if (!contactExists)
         throw new InvalidOperationException($"Contact with ID {contactId} not found.");

      // بررسی عدم تکرار
      var alreadyExists = await _recipientRepository
          .AnyAsync(r => r.CampaignId == campaignId && r.ContactId == contactId);

      if (!alreadyExists)
      {
         await _recipientRepository.AddAsync(new CampaignRecipient
         {
            CampaignId = campaignId,
            ContactId = contactId
         });
      }
   }

   public async Task RemoveRecipientAsync(int campaignId, int contactId)
   {
      var recipient = await _recipientRepository
          .FirstOrDefaultAsync(r => r.CampaignId == campaignId && r.ContactId == contactId);

      if (recipient != null)
      {
         await _recipientRepository.DeleteAsync(recipient);
      }
   }

   public async Task<IEnumerable<Contact>> GetRecipientsAsync(int campaignId)
   {
      var recipients = await _recipientRepository.Query()
          .Where(r => r.CampaignId == campaignId)
          .Include(r => r.Contact)
          .Select(r => r.Contact)
          .ToListAsync();

      return recipients;
   }

   public async Task AddRecipientsFromTagAsync(int campaignId, int tagId)
   {
      // بررسی وجود کمپین
      var campaignExists = await _campaignRepository.AnyAsync(c => c.Id == campaignId);
      if (!campaignExists)
         throw new InvalidOperationException($"Campaign with ID {campaignId} not found.");

      // بررسی وجود تگ
      var tagExists = await _tagRepository.AnyAsync(t => t.Id == tagId);
      if (!tagExists)
         throw new InvalidOperationException($"Tag with ID {tagId} not found.");

      // دریافت مخاطبین با این تگ
      var contactIds = await _contactTagRepository.Query()
          .Where(ct => ct.TagId == tagId)
          .Select(ct => ct.ContactId)
          .ToListAsync();

      // دریافت گیرندگان فعلی برای جلوگیری از تکرار
      var existingRecipientIds = await _recipientRepository.Query()
          .Where(r => r.CampaignId == campaignId)
          .Select(r => r.ContactId)
          .ToListAsync();

      // اضافه کردن فقط مخاطبین جدید
      var newRecipients = contactIds
          .Where(cId => !existingRecipientIds.Contains(cId))
          .Select(cId => new CampaignRecipient
          {
             CampaignId = campaignId,
             ContactId = cId
          })
          .ToList();

      if (newRecipients.Any())
      {
         await _recipientRepository.AddRangeAsync(newRecipients);
      }
   }

   #endregion

   #region ═══════════════════ Campaign Execution ═══════════════════

   public async Task StartCampaignAsync(int campaignId)
   {
      var campaign = await _campaignRepository.GetByIdAsync(campaignId);

      if (campaign == null)
         throw new InvalidOperationException($"Campaign with ID {campaignId} not found.");

      if (campaign.Status != CampaignStatus.Draft && campaign.Status != CampaignStatus.Scheduled)
         throw new InvalidOperationException($"Cannot start campaign with status '{campaign.Status}'.");

      campaign.Status = CampaignStatus.Running;
      campaign.StartedAt = DateTime.UtcNow;
      campaign.UpdatedAt = DateTime.UtcNow;

      await _campaignRepository.UpdateAsync(campaign);
   }

   public async Task PauseCampaignAsync(int campaignId)
   {
      var campaign = await _campaignRepository.GetByIdAsync(campaignId);

      if (campaign == null)
         throw new InvalidOperationException($"Campaign with ID {campaignId} not found.");

      if (campaign.Status != CampaignStatus.Running)
         throw new InvalidOperationException($"Cannot pause campaign with status '{campaign.Status}'.");

      campaign.Status = CampaignStatus.Paused;
      campaign.UpdatedAt = DateTime.UtcNow;

      await _campaignRepository.UpdateAsync(campaign);
   }

   public async Task CompleteCampaignAsync(int campaignId)
   {
      var campaign = await _campaignRepository.GetByIdAsync(campaignId);

      if (campaign == null)
         throw new InvalidOperationException($"Campaign with ID {campaignId} not found.");

      campaign.Status = CampaignStatus.Completed;
      campaign.CompletedAt = DateTime.UtcNow;
      campaign.UpdatedAt = DateTime.UtcNow;

      await _campaignRepository.UpdateAsync(campaign);
   }

   public async Task CancelCampaignAsync(int campaignId)
   {
      var campaign = await _campaignRepository.GetByIdAsync(campaignId);

      if (campaign == null)
         throw new InvalidOperationException($"Campaign with ID {campaignId} not found.");

      if (campaign.Status == CampaignStatus.Completed)
         throw new InvalidOperationException("Cannot cancel a completed campaign.");

      campaign.Status = CampaignStatus.Cancelled;
      campaign.UpdatedAt = DateTime.UtcNow;

      await _campaignRepository.UpdateAsync(campaign);
   }

   public async Task ScheduleCampaignAsync(int campaignId, DateTime scheduledAt)
   {
      var campaign = await _campaignRepository.GetByIdAsync(campaignId);

      if (campaign == null)
         throw new InvalidOperationException($"Campaign with ID {campaignId} not found.");

      if (campaign.Status != CampaignStatus.Draft)
         throw new InvalidOperationException($"Cannot schedule campaign with status '{campaign.Status}'.");

      if (scheduledAt <= DateTime.UtcNow)
         throw new ArgumentException("Scheduled time must be in the future.");

      campaign.Status = CampaignStatus.Scheduled;
      campaign.ScheduledAt = scheduledAt;
      campaign.UpdatedAt = DateTime.UtcNow;

      await _campaignRepository.UpdateAsync(campaign);
   }

   #endregion

   #region ═══════════════════ Statistics ═══════════════════

   public async Task<int> GetRecipientCountAsync(int campaignId)
   {
      return await _recipientRepository.CountAsync(r => r.CampaignId == campaignId);
   }

   public async Task<Dictionary<CampaignStatus, int>> GetCampaignStatisticsAsync()
   {
      return await _campaignRepository.Query()
          .GroupBy(c => c.Status)
          .Select(g => new { Status = g.Key, Count = g.Count() })
          .ToDictionaryAsync(x => x.Status, x => x.Count);
   }

   #endregion
}
