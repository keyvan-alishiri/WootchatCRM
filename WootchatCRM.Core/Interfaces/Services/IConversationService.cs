using WootchatCRM.Core.Entities;
using WootchatCRM.Core.Enums;

namespace WootchatCRM.Core.Interfaces.Services;

public interface IConversationService
{
   // ═══════════════ Query ═══════════════
   Task<IEnumerable<Conversation>> GetAllAsync();
   Task<Conversation?> GetByIdAsync(int id);
   Task<Conversation?> GetByChatwootIdAsync(int chatwootConversationId);
   Task<IEnumerable<Conversation>> GetByContactIdAsync(int contactId);
   Task<IEnumerable<Conversation>> GetByInboxIdAsync(int inboxId);
   Task<IEnumerable<Conversation>> GetByAssigneeIdAsync(int assigneeId);
   Task<IEnumerable<Conversation>> GetByTeamIdAsync(int teamId);
   Task<IEnumerable<Conversation>> GetByStatusAsync(ConversationStatus status);
   Task<IEnumerable<Conversation>> GetByPriorityAsync(Priority priority);
   Task<IEnumerable<Conversation>> GetUnassignedAsync();
   Task<IEnumerable<Conversation>> GetWithUnreadMessagesAsync();
   Task<IEnumerable<Conversation>> SearchAsync(string searchTerm);
   Task<IEnumerable<Conversation>> GetRecentAsync(int count);
   Task<IEnumerable<Conversation>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);

   // ═══════════════ Command ═══════════════
   Task<Conversation> CreateAsync(Conversation conversation);
   Task<Conversation> UpdateAsync(Conversation conversation);
   Task DeleteAsync(int id);
   Task UpdateStatusAsync(int conversationId, ConversationStatus status);
   Task UpdatePriorityAsync(int conversationId, Priority priority);
   Task AssignToUserAsync(int conversationId, int userId);
   Task AssignToTeamAsync(int conversationId, int teamId);
   Task UnassignAsync(int conversationId);

   // ═══════════════ Unread Management ═══════════════
   Task IncrementUnreadCountAsync(int conversationId);
   Task ResetUnreadCountAsync(int conversationId);
   Task UpdateLastMessageAsync(int conversationId, string preview);

   // ═══════════════ Tags ═══════════════
   Task AddTagAsync(int conversationId, int tagId);
   Task RemoveTagAsync(int conversationId, int tagId);
   Task<IEnumerable<Tag>> GetTagsAsync(int conversationId);
   Task<IEnumerable<Conversation>> GetByTagIdAsync(int tagId);

   // ═══════════════ Statistics ═══════════════
   Task<int> GetCountByStatusAsync(ConversationStatus status);
   Task<int> GetUnassignedCountAsync();
   Task<Dictionary<ConversationStatus, int>> GetStatusStatsAsync();
   Task<Dictionary<Priority, int>> GetPriorityStatsAsync();
}
