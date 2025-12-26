namespace WootchatCRM.Core.Entities;

public class Tag : BaseEntity
{
   /// <summary>
   /// Tag name (unique)
   /// </summary>
   public string Name { get; set; } = string.Empty;

   /// <summary>
   /// Tag color for UI (hex: #4CAF50)
   /// </summary>
   public string Color { get; set; } = "#607D8B";

   /// <summary>
   /// Tag description
   /// </summary>
   public string? Description { get; set; }

   /// <summary>
   /// Chatwoot label ID
   /// </summary>
   public int? ChatwootLabelId { get; set; }

   // Navigation
   public virtual ICollection<ContactTag> ContactTags { get; set; } = new List<ContactTag>();
   public virtual ICollection<ConversationTag> ConversationTags { get; set; } = new List<ConversationTag>();
}
