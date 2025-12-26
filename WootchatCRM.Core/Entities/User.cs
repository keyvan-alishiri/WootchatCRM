namespace WootchatCRM.Core.Entities;

public class User : BaseEntity
{
   public string Name { get; set; } = string.Empty;
   public string? Email { get; set; }
   public string? Role { get; set; }
   public int? ChatwootUserId { get; set; }
   public bool IsActive { get; set; } = true;

   // Foreign Key
   public int? TeamId { get; set; }

   // ═══════════════ Navigation Properties ═══════════════

   /// <summary>
   /// تیمی که کاربر عضو آن است
   /// </summary>
   public virtual Team? Team { get; set; }

   /// <summary>
   /// مکالماتی که به این کاربر اختصاص داده شده
   /// </summary>
   public virtual ICollection<Conversation> AssignedConversations { get; set; } = new List<Conversation>();

   /// <summary>
   /// تسک‌هایی که به این کاربر اختصاص داده شده
   /// </summary>
   public virtual ICollection<CrmTask> AssignedTasks { get; set; } = new List<CrmTask>();

   /// <summary>
   /// پیام‌هایی که توسط این کاربر ارسال شده
   /// </summary>
   public virtual ICollection<Message> Messages { get; set; } = new List<Message>();
}
