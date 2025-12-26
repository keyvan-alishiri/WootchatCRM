using WootchatCRM.Core.Entities;
using WootchatCRM.Core.Enums;

namespace WootchatCRM.Core.Interfaces.Services;

public interface IMessageService
{
   // ═══════════════ Query ═══════════════
   Task<IEnumerable<Message>> GetAllAsync();
   Task<Message?> GetByIdAsync(int id);
   Task<Message?> GetByChatwootIdAsync(int chatwootMessageId);
   Task<IEnumerable<Message>> GetByConversationIdAsync(int conversationId);
   Task<IEnumerable<Message>> GetByConversationIdPagedAsync(int conversationId, int page, int pageSize);
   Task<IEnumerable<Message>> GetByDirectionAsync(int conversationId, MessageDirection direction);
   Task<IEnumerable<Message>> GetBySenderIdAsync(int senderId);
   Task<IEnumerable<Message>> GetPrivateNotesAsync(int conversationId);
   Task<IEnumerable<Message>> GetByContentTypeAsync(int conversationId, string contentType);
   Task<IEnumerable<Message>> SearchAsync(string searchTerm);
   Task<IEnumerable<Message>> SearchInConversationAsync(int conversationId, string searchTerm);
   Task<Message?> GetLastMessageAsync(int conversationId);
   Task<IEnumerable<Message>> GetByDateRangeAsync(int conversationId, DateTime startDate, DateTime endDate);

   // ═══════════════ Command ═══════════════
   Task<Message> CreateAsync(Message message);
   Task<Message> UpdateAsync(Message message);
   Task DeleteAsync(int id);
   Task UpdateStatusAsync(int messageId, MessageStatus status);

   // ═══════════════ Reply ═══════════════
   Task<Message> CreateReplyAsync(int replyToMessageId, Message message);
   Task<IEnumerable<Message>> GetRepliesAsync(int messageId);

   // ═══════════════ Private Notes ═══════════════
   Task<Message> CreatePrivateNoteAsync(int conversationId, int senderId, string content);

   // ═══════════════ Statistics ═══════════════
   Task<int> GetMessageCountAsync(int conversationId);
   Task<int> GetPrivateNoteCountAsync(int conversationId);
   Task<Dictionary<string, int>> GetContentTypeStatsAsync(int conversationId);
}
