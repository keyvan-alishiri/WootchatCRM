using WootchatCRM.Core.Entities;
using WootchatCRM.Core.Enums;

namespace WootchatCRM.Core.Interfaces.Services;

public interface ICampaignService
{
   #region ═══════════════════ Query Methods ═══════════════════

   /// <summary>
   /// دریافت همه کمپین‌ها
   /// </summary>
   Task<IEnumerable<Campaign>> GetAllAsync();

   /// <summary>
   /// دریافت کمپین بر اساس شناسه
   /// </summary>
   Task<Campaign?> GetByIdAsync(int id);

   /// <summary>
   /// دریافت کمپین‌ها بر اساس وضعیت
   /// </summary>
   Task<IEnumerable<Campaign>> GetByStatusAsync(CampaignStatus status);

   #endregion

   #region ═══════════════════ Command Methods ═══════════════════

   /// <summary>
   /// ایجاد کمپین جدید
   /// </summary>
   Task<Campaign> CreateAsync(Campaign campaign);

   /// <summary>
   /// به‌روزرسانی کمپین
   /// </summary>
   Task<Campaign> UpdateAsync(Campaign campaign);

   /// <summary>
   /// حذف کمپین
   /// </summary>
   Task DeleteAsync(int id);

   #endregion

   #region ═══════════════════ Recipients Management ═══════════════════

   /// <summary>
   /// اضافه کردن یک گیرنده به کمپین
   /// </summary>
   Task AddRecipientAsync(int campaignId, int contactId);

   /// <summary>
   /// حذف یک گیرنده از کمپین
   /// </summary>
   Task RemoveRecipientAsync(int campaignId, int contactId);

   /// <summary>
   /// دریافت لیست گیرندگان کمپین
   /// </summary>
   Task<IEnumerable<Contact>> GetRecipientsAsync(int campaignId);

   /// <summary>
   /// اضافه کردن همه مخاطبین یک تگ به کمپین
   /// </summary>
   Task AddRecipientsFromTagAsync(int campaignId, int tagId);

   #endregion

   #region ═══════════════════ Campaign Execution ═══════════════════

   /// <summary>
   /// شروع اجرای کمپین
   /// </summary>
   Task StartCampaignAsync(int campaignId);

   /// <summary>
   /// توقف موقت کمپین
   /// </summary>
   Task PauseCampaignAsync(int campaignId);

   /// <summary>
   /// اتمام کمپین
   /// </summary>
   Task CompleteCampaignAsync(int campaignId);

   /// <summary>
   /// لغو کمپین
   /// </summary>
   Task CancelCampaignAsync(int campaignId);

   /// <summary>
   /// زمان‌بندی کمپین برای اجرا در آینده
   /// </summary>
   Task ScheduleCampaignAsync(int campaignId, DateTime scheduledAt);

   #endregion

   #region ═══════════════════ Statistics ═══════════════════

   /// <summary>
   /// دریافت تعداد گیرندگان یک کمپین
   /// </summary>
   Task<int> GetRecipientCountAsync(int campaignId);

   /// <summary>
   /// دریافت آمار کمپین‌ها بر اساس وضعیت
   /// </summary>
   Task<Dictionary<CampaignStatus, int>> GetCampaignStatisticsAsync();

   #endregion
}
