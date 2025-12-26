using WootchatCRM.Core.Enums;

namespace WootchatCRM.Core.Entities;

public class Message : BaseEntity
{
   // ═══════════════ Properties ═══════════════

   public string Content { get; set; } = string.Empty;
   public MessageDirection Direction { get; set; } = MessageDirection.Incoming;
   public MessageStatus Status { get; set; } = MessageStatus.Sent;

   /// <summary>
   /// شناسه پیام در Chatwoot
   /// </summary>
   public int? ChatwootMessageId { get; set; }

   /// <summary>
   /// نوع محتوا: text, image, file, etc.
   /// </summary>
   public string ContentType { get; set; } = "text";

   /// <summary>
   /// آیا پیام خصوصی است (یادداشت داخلی)
   /// </summary>
   public bool IsPrivate { get; set; } = false;

   // ═══════════════ Foreign Keys ═══════════════

   public int ConversationId { get; set; }
   public int? SenderId { get; set; }
   public int? ReplyToMessageId { get; set; }

   // ═══════════════ Navigation Properties ═══════════════

   public virtual Conversation Conversation { get; set; } = null!;
   public virtual User? Sender { get; set; }
   public virtual Message? ReplyToMessage { get; set; }
   public virtual ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();
}
