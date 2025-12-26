using WootchatCRM.Core.Enums;
using TaskStatus = WootchatCRM.Core.Enums.TaskStatus;

namespace WootchatCRM.Core.Entities;

public class CrmTask : BaseEntity
{
   // ═══════════════ Properties ═══════════════

   public string Title { get; set; } = string.Empty;
   public string? Description { get; set; }
   public DateTime? DueDate { get; set; }
   public TaskStatus Status { get; set; } = TaskStatus.Pending;
   public Priority Priority { get; set; } = Priority.Medium;
   public DateTime? CompletedAt { get; set; }

   // ═══════════════ Foreign Keys ═══════════════

   /// <summary>
   /// کاربری که تسک به او اختصاص داده شده
   /// </summary>
   public int? AssignedToId { get; set; }

   /// <summary>
   /// مخاطب مرتبط با تسک
   /// </summary>
   public int? ContactId { get; set; }

   /// <summary>
   /// مکالمه مرتبط با تسک
   /// </summary>
   public int? ConversationId { get; set; }

   // ═══════════════ Navigation Properties ═══════════════

   public virtual User? AssignedTo { get; set; }
   public virtual Contact? Contact { get; set; }
   public virtual Conversation? Conversation { get; set; }
}
