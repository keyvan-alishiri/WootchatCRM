using WootchatCRM.Core.Entities;
using WootchatCRM.Core.Enums;
using WootchatCRM.Core.Interfaces;
using WootchatCRM.Core.Interfaces.Services;

namespace WootchatCRM.Infrastructure.Services;

public class ConversationService : IConversationService
{
   private readonly IRepository<Conversation> _conversationRepo;
   private readonly IRepository<ConversationTag> _conversationTagRepo;
   private readonly IRepository<Tag> _tagRepo;
   private readonly IRepository<User> _userRepo;

   public ConversationService(
       IRepository<Conversation> conversationRepo,
       IRepository<ConversationTag> conversationTagRepo,
       IRepository<Tag> tagRepo,
       IRepository<User> userRepo)
   {
      _conversationRepo = conversationRepo;
      _conversationTagRepo = conversationTagRepo;
      _tagRepo = tagRepo;
      _userRepo = userRepo;
   }

   #region ═══════════════ Query Methods ═══════════════

   public async Task<IEnumerable<Conversation>> GetAllAsync()
   {
      var query = await _conversationRepo.GetAllAsync();
      return query.OrderByDescending(c => c.LastMessageAt ?? c.CreatedAt);
   }

   public async Task<Conversation?> GetByIdAsync(int id)
   {
      return await _conversationRepo.GetByIdAsync(id);
   }

   public async Task<Conversation?> GetByChatwootIdAsync(int chatwootConversationId)
   {
      return await _conversationRepo.FirstOrDefaultAsync(
          c => c.ChatwootConversationId == chatwootConversationId);
   }

   public async Task<IEnumerable<Conversation>> GetByContactIdAsync(int contactId)
   {
      return await _conversationRepo.FindAsync(c => c.ContactId == contactId);
   }

   public async Task<IEnumerable<Conversation>> GetByInboxIdAsync(int inboxId)
   {
      return await _conversationRepo.FindAsync(c => c.InboxId == inboxId);
   }

   public async Task<IEnumerable<Conversation>> GetByAssigneeIdAsync(int assigneeId)
   {
      return await _conversationRepo.FindAsync(c => c.AssigneeId == assigneeId);
   }

   public async Task<IEnumerable<Conversation>> GetByTeamIdAsync(int teamId)
   {
      return await _conversationRepo.FindAsync(c => c.TeamId == teamId);
   }

   public async Task<IEnumerable<Conversation>> GetByStatusAsync(ConversationStatus status)
   {
      return await _conversationRepo.FindAsync(c => c.Status == status);
   }

   public async Task<IEnumerable<Conversation>> GetByPriorityAsync(Priority priority)
   {
      return await _conversationRepo.FindAsync(c => c.Priority == priority);
   }

   public async Task<IEnumerable<Conversation>> GetUnassignedAsync()
   {
      return await _conversationRepo.FindAsync(c => c.AssigneeId == null);
   }

   public async Task<IEnumerable<Conversation>> GetWithUnreadMessagesAsync()
   {
      return await _conversationRepo.FindAsync(c => c.UnreadCount > 0);
   }

   public async Task<IEnumerable<Conversation>> SearchAsync(string searchTerm)
   {
      if (string.IsNullOrWhiteSpace(searchTerm))
         return Enumerable.Empty<Conversation>();

      var lowerTerm = searchTerm.ToLower();

      return await _conversationRepo.FindAsync(c =>
          (c.LastMessagePreview != null && c.LastMessagePreview.ToLower().Contains(lowerTerm)));
   }

   public async Task<IEnumerable<Conversation>> GetRecentAsync(int count)
   {
      var all = await _conversationRepo.GetAllAsync();
      return all
          .OrderByDescending(c => c.LastMessageAt ?? c.CreatedAt)
          .Take(count);
   }

   public async Task<IEnumerable<Conversation>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
   {
      return await _conversationRepo.FindAsync(c =>
          c.CreatedAt >= startDate && c.CreatedAt <= endDate);
   }

   #endregion

   #region ═══════════════ Command Methods ═══════════════

   public async Task<Conversation> CreateAsync(Conversation conversation)
   {
      conversation.CreatedAt = DateTime.UtcNow;
      conversation.UpdatedAt = DateTime.UtcNow;

      return await _conversationRepo.AddAsync(conversation);
   }

   public async Task<Conversation> UpdateAsync(Conversation conversation)
   {
      conversation.UpdatedAt = DateTime.UtcNow;
      await _conversationRepo.UpdateAsync(conversation);
      return conversation;
   }

   public async Task DeleteAsync(int id)
   {
      var conversation = await _conversationRepo.GetByIdAsync(id);
      if (conversation != null)
      {
         await _conversationRepo.DeleteAsync(conversation);
      }
   }

   public async Task UpdateStatusAsync(int conversationId, ConversationStatus status)
   {
      var conversation = await _conversationRepo.GetByIdAsync(conversationId);
      if (conversation == null) return;

      conversation.Status = status;
      conversation.UpdatedAt = DateTime.UtcNow;

      await _conversationRepo.UpdateAsync(conversation);
   }

   public async Task UpdatePriorityAsync(int conversationId, Priority priority)
   {
      var conversation = await _conversationRepo.GetByIdAsync(conversationId);
      if (conversation == null) return;

      conversation.Priority = priority;
      conversation.UpdatedAt = DateTime.UtcNow;

      await _conversationRepo.UpdateAsync(conversation);
   }

   public async Task AssignToUserAsync(int conversationId, int userId)
   {
      var conversation = await _conversationRepo.GetByIdAsync(conversationId);
      if (conversation == null) return;

      // چک کردن وجود کاربر
      var user = await _userRepo.GetByIdAsync(userId);
      if (user == null)
         throw new InvalidOperationException($"User with ID {userId} not found.");

      conversation.AssigneeId = userId;
      conversation.UpdatedAt = DateTime.UtcNow;

      await _conversationRepo.UpdateAsync(conversation);
   }

   public async Task AssignToTeamAsync(int conversationId, int teamId)
   {
      var conversation = await _conversationRepo.GetByIdAsync(conversationId);
      if (conversation == null) return;

      conversation.TeamId = teamId;
      conversation.UpdatedAt = DateTime.UtcNow;

      await _conversationRepo.UpdateAsync(conversation);
   }

   public async Task UnassignAsync(int conversationId)
   {
      var conversation = await _conversationRepo.GetByIdAsync(conversationId);
      if (conversation == null) return;

      conversation.AssigneeId = null;
      conversation.TeamId = null;
      conversation.UpdatedAt = DateTime.UtcNow;

      await _conversationRepo.UpdateAsync(conversation);
   }

   #endregion

   #region ═══════════════ Unread Management ═══════════════

   public async Task IncrementUnreadCountAsync(int conversationId)
   {
      var conversation = await _conversationRepo.GetByIdAsync(conversationId);
      if (conversation == null) return;

      conversation.UnreadCount++;
      conversation.UpdatedAt = DateTime.UtcNow;

      await _conversationRepo.UpdateAsync(conversation);
   }

   public async Task ResetUnreadCountAsync(int conversationId)
   {
      var conversation = await _conversationRepo.GetByIdAsync(conversationId);
      if (conversation == null) return;

      conversation.UnreadCount = 0;
      conversation.UpdatedAt = DateTime.UtcNow;

      await _conversationRepo.UpdateAsync(conversation);
   }

   public async Task UpdateLastMessageAsync(int conversationId, string preview)
   {
      var conversation = await _conversationRepo.GetByIdAsync(conversationId);
      if (conversation == null) return;

      conversation.LastMessagePreview = preview.Length > 100
          ? preview.Substring(0, 100) + "..."
          : preview;
      conversation.LastMessageAt = DateTime.UtcNow;
      conversation.UpdatedAt = DateTime.UtcNow;

      await _conversationRepo.UpdateAsync(conversation);
   }

   #endregion

   #region ═══════════════ Tags ═══════════════

   public async Task AddTagAsync(int conversationId, int tagId)
   {
      // چک کردن وجود رابطه
      var existing = await _conversationTagRepo.FirstOrDefaultAsync(
          ct => ct.ConversationId == conversationId && ct.TagId == tagId);

      if (existing != null) return;

      var conversationTag = new ConversationTag
      {
         ConversationId = conversationId,
         TagId = tagId,
         CreatedAt = DateTime.UtcNow,
         UpdatedAt = DateTime.UtcNow
      };

      await _conversationTagRepo.AddAsync(conversationTag);
   }

   public async Task RemoveTagAsync(int conversationId, int tagId)
   {
      var conversationTag = await _conversationTagRepo.FirstOrDefaultAsync(
          ct => ct.ConversationId == conversationId && ct.TagId == tagId);

      if (conversationTag != null)
      {
         await _conversationTagRepo.DeleteAsync(conversationTag);
      }
   }

   public async Task<IEnumerable<Tag>> GetTagsAsync(int conversationId)
   {
      var conversationTags = await _conversationTagRepo.FindAsync(
          ct => ct.ConversationId == conversationId);

      var tagIds = conversationTags.Select(ct => ct.TagId).ToList();

      var allTags = await _tagRepo.GetAllAsync();
      return allTags.Where(t => tagIds.Contains(t.Id));
   }

   public async Task<IEnumerable<Conversation>> GetByTagIdAsync(int tagId)
   {
      var conversationTags = await _conversationTagRepo.FindAsync(ct => ct.TagId == tagId);
      var conversationIds = conversationTags.Select(ct => ct.ConversationId).ToList();

      return await _conversationRepo.FindAsync(c => conversationIds.Contains(c.Id));
   }

   #endregion

   #region ═══════════════ Statistics ═══════════════

   public async Task<int> GetCountByStatusAsync(ConversationStatus status)
   {
      var conversations = await _conversationRepo.FindAsync(c => c.Status == status);
      return conversations.Count();
   }

   public async Task<int> GetUnassignedCountAsync()
   {
      var conversations = await _conversationRepo.FindAsync(c => c.AssigneeId == null);
      return conversations.Count();
   }

   public async Task<Dictionary<ConversationStatus, int>> GetStatusStatsAsync()
   {
      var all = await _conversationRepo.GetAllAsync();

      return all
          .GroupBy(c => c.Status)
          .ToDictionary(g => g.Key, g => g.Count());
   }

   public async Task<Dictionary<Priority, int>> GetPriorityStatsAsync()
   {
      var all = await _conversationRepo.GetAllAsync();

      return all
          .GroupBy(c => c.Priority)
          .ToDictionary(g => g.Key, g => g.Count());
   }

   #endregion
}
