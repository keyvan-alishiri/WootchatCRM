using WootchatCRM.Infrastructure.Chatwoot.DTOs;

namespace WootchatCRM.Infrastructure.Chatwoot;

/// <summary>
/// رابط برای ارتباط با Chatwoot API
/// </summary>
public interface IChatwootApiClient
{
   /// <summary>
   /// بازنشانی تنظیمات (بعد از تغییر در Settings فراخوانی شود)
   /// </summary>
   void ResetConfiguration();

   // ═══════════════════════════════════════════════════════════════
   // Profile & Connection Test
   // ═══════════════════════════════════════════════════════════════

   /// <summary>
   /// دریافت پروفایل کاربر فعلی (برای تست اتصال)
   /// </summary>
   Task<ChatwootApiResult<ChatwootProfileResponse>> GetProfileAsync();

   // ═══════════════════════════════════════════════════════════════
   // Inboxes
   // ═══════════════════════════════════════════════════════════════

   /// <summary>
   /// دریافت لیست Inbox ها
   /// </summary>
   Task<ChatwootApiResult<List<ChatwootInbox>>> GetInboxesAsync();

   /// <summary>
   /// دریافت یک Inbox با شناسه
   /// </summary>
   Task<ChatwootApiResult<ChatwootInbox>> GetInboxAsync(int inboxId);

   // ═══════════════════════════════════════════════════════════════
   // Contacts
   // ═══════════════════════════════════════════════════════════════

   /// <summary>
   /// دریافت لیست مخاطبین با صفحه‌بندی
   /// </summary>
   Task<ChatwootApiResult<ChatwootContactListResponse>> GetContactsAsync(int page = 1);

   /// <summary>
   /// دریافت یک مخاطب با شناسه
   /// </summary>
   Task<ChatwootApiResult<ChatwootContact>> GetContactAsync(int contactId);

   /// <summary>
   /// ایجاد مخاطب جدید
   /// </summary>
   Task<ChatwootApiResult<ChatwootContact>> CreateContactAsync(ChatwootContactCreateRequest request);

   /// <summary>
   /// بروزرسانی مخاطب
   /// </summary>
   Task<ChatwootApiResult<ChatwootContact>> UpdateContactAsync(int contactId, ChatwootContactUpdateRequest request);

   /// <summary>
   /// حذف مخاطب
   /// </summary>
   Task<ChatwootApiResult<bool>> DeleteContactAsync(int contactId);

   /// <summary>
   /// جستجوی مخاطب با شماره تلفن
   /// </summary>
   Task<ChatwootApiResult<ChatwootContact?>> SearchContactByPhoneAsync(string phoneNumber);

   /// <summary>
   /// جستجوی مخاطب با ایمیل
   /// </summary>
   Task<ChatwootApiResult<ChatwootContact?>> SearchContactByEmailAsync(string email);

   // ═══════════════════════════════════════════════════════════════
   // Conversations
   // ═══════════════════════════════════════════════════════════════

   /// <summary>
   /// دریافت لیست مکالمات
   /// </summary>
   Task<ChatwootApiResult<ChatwootConversationListResponse>> GetConversationsAsync(
       string status = "open",
       int page = 1,
       int? inboxId = null,
       int? assigneeId = null);

   /// <summary>
   /// دریافت یک مکالمه با شناسه
   /// </summary>
   Task<ChatwootApiResult<ChatwootConversation>> GetConversationAsync(int conversationId);

   /// <summary>
   /// ایجاد مکالمه جدید
   /// </summary>
   Task<ChatwootApiResult<ChatwootConversation>> CreateConversationAsync(ChatwootConversationCreateRequest request);

   /// <summary>
   /// تغییر وضعیت مکالمه
   /// </summary>
   Task<ChatwootApiResult<bool>> UpdateConversationStatusAsync(int conversationId, string status);

   /// <summary>
   /// تخصیص اپراتور به مکالمه
   /// </summary>
   Task<ChatwootApiResult<bool>> AssignAgentToConversationAsync(int conversationId, int agentId);

   // ═══════════════════════════════════════════════════════════════
   // Messages
   // ═══════════════════════════════════════════════════════════════

   /// <summary>
   /// دریافت پیام‌های یک مکالمه
   /// </summary>
   Task<ChatwootApiResult<List<ChatwootMessage>>> GetMessagesAsync(int conversationId);

   /// <summary>
   /// ارسال پیام در مکالمه
   /// </summary>
   Task<ChatwootApiResult<ChatwootMessage>> SendMessageAsync(int conversationId, ChatwootMessageCreateRequest request);

   /// <summary>
   /// ارسال پیام متنی ساده
   /// </summary>
   Task<ChatwootApiResult<ChatwootMessage>> SendTextMessageAsync(int conversationId, string content, bool isPrivate = false);

   // ═══════════════════════════════════════════════════════════════
   // Agents
   // ═══════════════════════════════════════════════════════════════

   /// <summary>
   /// دریافت لیست اپراتورها
   /// </summary>
   Task<ChatwootApiResult<List<ChatwootAgent>>> GetAgentsAsync();

   /// <summary>
   /// دریافت اپراتورهای یک Inbox
   /// </summary>
   Task<ChatwootApiResult<List<ChatwootAgent>>> GetInboxAgentsAsync(int inboxId);

   // ═══════════════════════════════════════════════════════════════
   // Labels
   // ═══════════════════════════════════════════════════════════════

   /// <summary>
   /// دریافت لیست برچسب‌ها
   /// </summary>
   Task<ChatwootApiResult<List<ChatwootLabel>>> GetLabelsAsync();

   /// <summary>
   /// دریافت برچسب‌های یک مکالمه
   /// </summary>
   Task<ChatwootApiResult<List<string>>> GetConversationLabelsAsync(int conversationId);

   /// <summary>
   /// اضافه کردن برچسب به مکالمه
   /// </summary>
   Task<ChatwootApiResult<bool>> AddLabelsToConversationAsync(int conversationId, List<string> labels);
}
