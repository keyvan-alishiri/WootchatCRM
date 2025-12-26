using Microsoft.EntityFrameworkCore;
using WootchatCRM.Core.Entities;
using WootchatCRM.Core.Enums;
using WootchatCRM.Core.Interfaces;
using WootchatCRM.Core.Interfaces.Services;

namespace WootchatCRM.Infrastructure.Services;

public class MessageService : IMessageService
{
   private readonly IRepository<Message> _messageRepository;
   private readonly IRepository<Conversation> _conversationRepository;

   public MessageService(
       IRepository<Message> messageRepository,
       IRepository<Conversation> conversationRepository)
   {
      _messageRepository = messageRepository;
      _conversationRepository = conversationRepository;
   }

   #region ═══════════════════ Query Methods ═══════════════════

   public async Task<IEnumerable<Message>> GetAllAsync()
   {
      return await _messageRepository.Query()
          .Include(m => m.Conversation)
          .Include(m => m.Sender)
          .Include(m => m.ReplyToMessage)
          .OrderByDescending(m => m.CreatedAt)
          .ToListAsync();
   }

   public async Task<Message?> GetByIdAsync(int id)
   {
      return await _messageRepository.Query()
          .Include(m => m.Conversation)
          .Include(m => m.Sender)
          .Include(m => m.ReplyToMessage)
         
          .FirstOrDefaultAsync(m => m.Id == id);
   }

   public async Task<Message?> GetByChatwootIdAsync(int chatwootMessageId)
   {
      return await _messageRepository.Query()
          .Include(m => m.Conversation)
          .Include(m => m.Sender)
          .FirstOrDefaultAsync(m => m.ChatwootMessageId == chatwootMessageId);
   }

   public async Task<IEnumerable<Message>> GetByConversationIdAsync(int conversationId)
   {
      return await _messageRepository.Query()
          .Where(m => m.ConversationId == conversationId)
          .Include(m => m.Sender)
          .Include(m => m.ReplyToMessage)
          .OrderBy(m => m.CreatedAt)
          .ToListAsync();
   }

   public async Task<IEnumerable<Message>> GetByConversationIdPagedAsync(int conversationId, int page, int pageSize)
   {
      return await _messageRepository.Query()
          .Where(m => m.ConversationId == conversationId)
          .Include(m => m.Sender)
          .Include(m => m.ReplyToMessage)
          .OrderByDescending(m => m.CreatedAt)
          .Skip((page - 1) * pageSize)
          .Take(pageSize)
          .ToListAsync();
   }

   public async Task<IEnumerable<Message>> GetByDirectionAsync(int conversationId, MessageDirection direction)
   {
      return await _messageRepository.Query()
          .Where(m => m.ConversationId == conversationId && m.Direction == direction)
          .Include(m => m.Sender)
          .OrderBy(m => m.CreatedAt)
          .ToListAsync();
   }

   public async Task<IEnumerable<Message>> GetBySenderIdAsync(int senderId)
   {
      return await _messageRepository.Query()
          .Where(m => m.SenderId == senderId)
          .Include(m => m.Conversation)
          .OrderByDescending(m => m.CreatedAt)
          .ToListAsync();
   }

   public async Task<IEnumerable<Message>> GetPrivateNotesAsync(int conversationId)
   {
      return await _messageRepository.Query()
          .Where(m => m.ConversationId == conversationId && m.IsPrivate)
          .Include(m => m.Sender)
          .OrderBy(m => m.CreatedAt)
          .ToListAsync();
   }

   public async Task<IEnumerable<Message>> GetByContentTypeAsync(int conversationId, string contentType)
   {
      return await _messageRepository.Query()
          .Where(m => m.ConversationId == conversationId && m.ContentType == contentType)
          .Include(m => m.Sender)
          .OrderBy(m => m.CreatedAt)
          .ToListAsync();
   }

   public async Task<IEnumerable<Message>> SearchAsync(string searchTerm)
   {
      if (string.IsNullOrWhiteSpace(searchTerm))
         return Enumerable.Empty<Message>();

      var lowerSearchTerm = searchTerm.ToLower();

      return await _messageRepository.Query()
          .Where(m => m.Content != null && m.Content.ToLower().Contains(lowerSearchTerm))
          .Include(m => m.Conversation)
          .Include(m => m.Sender)
          .OrderByDescending(m => m.CreatedAt)
          .Take(100) // محدودیت برای جلوگیری از کوئری سنگین
          .ToListAsync();
   }

   public async Task<IEnumerable<Message>> SearchInConversationAsync(int conversationId, string searchTerm)
   {
      if (string.IsNullOrWhiteSpace(searchTerm))
         return Enumerable.Empty<Message>();

      var lowerSearchTerm = searchTerm.ToLower();

      return await _messageRepository.Query()
          .Where(m => m.ConversationId == conversationId &&
                      m.Content != null &&
                      m.Content.ToLower().Contains(lowerSearchTerm))
          .Include(m => m.Sender)
          .OrderBy(m => m.CreatedAt)
          .ToListAsync();
   }

   public async Task<Message?> GetLastMessageAsync(int conversationId)
   {
      return await _messageRepository.Query()
          .Where(m => m.ConversationId == conversationId)
          .Include(m => m.Sender)
          .OrderByDescending(m => m.CreatedAt)
          .FirstOrDefaultAsync();
   }

   public async Task<IEnumerable<Message>> GetByDateRangeAsync(int conversationId, DateTime startDate, DateTime endDate)
   {
      return await _messageRepository.Query()
          .Where(m => m.ConversationId == conversationId &&
                      m.CreatedAt >= startDate &&
                      m.CreatedAt <= endDate)
          .Include(m => m.Sender)
          .OrderBy(m => m.CreatedAt)
          .ToListAsync();
   }

   #endregion

   #region ═══════════════════ Command Methods ═══════════════════

   public async Task<Message> CreateAsync(Message message)
   {
      // بررسی وجود مکالمه
      var conversationExists = await _conversationRepository.AnyAsync(c => c.Id == message.ConversationId);
      if (!conversationExists)
         throw new InvalidOperationException($"Conversation with ID {message.ConversationId} not found.");

      message.CreatedAt = DateTime.UtcNow;

      return await _messageRepository.AddAsync(message);
   }

   public async Task<Message> UpdateAsync(Message message)
   {
      var existing = await _messageRepository.GetByIdAsync(message.Id);

      if (existing == null)
         throw new InvalidOperationException($"Message with ID {message.Id} not found.");

      existing.Content = message.Content;
      existing.ContentType = message.ContentType;
      existing.Status = message.Status;
      existing.UpdatedAt = DateTime.UtcNow;

      await _messageRepository.UpdateAsync(existing);

      return existing;
   }

   public async Task DeleteAsync(int id)
   {
      var message = await _messageRepository.GetByIdAsync(id);

      if (message != null)
      {
         await _messageRepository.DeleteAsync(message);
      }
   }

   public async Task UpdateStatusAsync(int messageId, MessageStatus status)
   {
      var message = await _messageRepository.GetByIdAsync(messageId);

      if (message == null)
         throw new InvalidOperationException($"Message with ID {messageId} not found.");

      message.Status = status;
      message.UpdatedAt = DateTime.UtcNow;

      await _messageRepository.UpdateAsync(message);
   }

   #endregion

   #region ═══════════════════ Reply Methods ═══════════════════

   public async Task<Message> CreateReplyAsync(int replyToMessageId, Message message)
   {
      var parentMessage = await _messageRepository.GetByIdAsync(replyToMessageId);

      if (parentMessage == null)
         throw new InvalidOperationException($"Parent message with ID {replyToMessageId} not found.");

      message.ReplyToMessageId = replyToMessageId;
      message.ConversationId = parentMessage.ConversationId;
      message.CreatedAt = DateTime.UtcNow;

      return await _messageRepository.AddAsync(message);
   }

   public async Task<IEnumerable<Message>> GetRepliesAsync(int messageId)
   {
      return await _messageRepository.Query()
          .Where(m => m.ReplyToMessageId == messageId)
          .Include(m => m.Sender)
          .OrderBy(m => m.CreatedAt)
          .ToListAsync();
   }

   #endregion

   #region ═══════════════════ Private Notes ═══════════════════

   public async Task<Message> CreatePrivateNoteAsync(int conversationId, int senderId, string content)
   {
      // بررسی وجود مکالمه
      var conversationExists = await _conversationRepository.AnyAsync(c => c.Id == conversationId);
      if (!conversationExists)
         throw new InvalidOperationException($"Conversation with ID {conversationId} not found.");

      var privateNote = new Message
      {
         ConversationId = conversationId,
         SenderId = senderId,
         Content = content,
         ContentType = "text",
         IsPrivate = true,
         Direction = MessageDirection.Outgoing,
         Status = MessageStatus.Sent,
         CreatedAt = DateTime.UtcNow
      };

      return await _messageRepository.AddAsync(privateNote);
   }

   #endregion

   #region ═══════════════════ Statistics ═══════════════════

   public async Task<int> GetMessageCountAsync(int conversationId)
   {
      return await _messageRepository.CountAsync(m => m.ConversationId == conversationId && !m.IsPrivate);
   }

   public async Task<int> GetPrivateNoteCountAsync(int conversationId)
   {
      return await _messageRepository.CountAsync(m => m.ConversationId == conversationId && m.IsPrivate);
   }

   public async Task<Dictionary<string, int>> GetContentTypeStatsAsync(int conversationId)
   {
      return await _messageRepository.Query()
          .Where(m => m.ConversationId == conversationId)
          .GroupBy(m => m.ContentType ?? "unknown")
          .Select(g => new { ContentType = g.Key, Count = g.Count() })
          .ToDictionaryAsync(x => x.ContentType, x => x.Count);
   }

   #endregion
}
