namespace WootchatCRM.Core.Entities;

public class Note : BaseEntity
{
   /// <summary>
   /// Related contact (optional)
   /// </summary>
   public int? ContactId { get; set; }

   /// <summary>
   /// Related conversation (optional)
   /// </summary>
   public int? ConversationId { get; set; }

   /// <summary>
   /// Author user
   /// </summary>
   public int UserId { get; set; }

   /// <summary>
   /// Note title (optional)
   /// </summary>
   public string? Title { get; set; }

   /// <summary>
   /// Note content (supports rich text)
   /// </summary>
   public string Content { get; set; } = string.Empty;

   /// <summary>
   /// Is this a pinned/important note?
   /// </summary>
   public bool IsPinned { get; set; } = false;

   /// <summary>
   /// Note color for UI (hex: #FFEB3B)
   /// </summary>
   public string? Color { get; set; }

   // Navigation
   public virtual Contact? Contact { get; set; }
   public virtual Conversation? Conversation { get; set; }
   public virtual User User { get; set; } = null!;
}
