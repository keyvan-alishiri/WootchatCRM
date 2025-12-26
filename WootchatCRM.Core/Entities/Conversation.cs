using WootchatCRM.Core.Enums;

namespace WootchatCRM.Core.Entities;

public class Conversation : BaseEntity
{
   // ═══════════════ Foreign Keys ═══════════════
   public int ContactId { get; set; }
   public int InboxId { get; set; }
   public int? AssigneeId { get; set; }
   public int? TeamId { get; set; }

   // ═══════════════ Properties ═══════════════

   /// <summary>
   /// شناسه مکالمه در Chatwoot
   /// </summary>
   public int? ChatwootConversationId { get; set; }

   public ConversationStatus Status { get; set; } = ConversationStatus.Open;
   public Priority Priority { get; set; } = Priority.Medium;

   /// <summary>
   /// پیش‌نمایش آخرین پیام
   /// </summary>
   public string? LastMessagePreview { get; set; }

   public DateTime? LastMessageAt { get; set; }

   /// <summary>
   /// تعداد پیام‌های خوانده نشده
   /// </summary>
   public int UnreadCount { get; set; } = 0;

   // ═══════════════ Navigation Properties ═══════════════

   public virtual Contact Contact { get; set; } = null!;
   public virtual Inbox Inbox { get; set; } = null!;
   public virtual User? Assignee { get; set; }
   public virtual Team? Team { get; set; }

   public virtual ICollection<Message> Messages { get; set; } = new List<Message>();
   public virtual ICollection<Note> Notes { get; set; } = new List<Note>();

   /// <summary>
   /// تسک‌های مرتبط با این مکالمه - نام باید CrmTasks باشد
   /// </summary>
   public virtual ICollection<CrmTask> CrmTasks { get; set; } = new List<CrmTask>();

   public virtual ICollection<ConversationTag> ConversationTags { get; set; } = new List<ConversationTag>();
}
