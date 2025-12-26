namespace WootchatCRM.Core.Entities;

/// <summary>
/// Many-to-Many relationship between Contact and Tag
/// </summary>
public class ContactTag
{
   public int ContactId { get; set; }
   public int TagId { get; set; }

   public DateTime AssignedAt { get; set; } = DateTime.UtcNow;

   // Navigation
   public virtual Contact Contact { get; set; } = null!;
   public virtual Tag Tag { get; set; } = null!;
}
