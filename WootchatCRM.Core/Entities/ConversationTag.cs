namespace WootchatCRM.Core.Entities;

/// <summary>
/// Many-to-Many relationship between Conversation and Tag
/// </summary>
public class ConversationTag  :BaseEntity
{
   public int ConversationId { get; set; }
   public int TagId { get; set; }

   public DateTime AssignedAt { get; set; } = DateTime.UtcNow;

   // Navigation
   public virtual Conversation Conversation { get; set; } = null!;
   public virtual Tag Tag { get; set; } = null!;
}
