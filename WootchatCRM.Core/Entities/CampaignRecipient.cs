using WootchatCRM.Core.Enums;

namespace WootchatCRM.Core.Entities;

/// <summary>
/// Campaign recipients tracking
/// </summary>
public class CampaignRecipient : BaseEntity
{
   public int CampaignId { get; set; }
   public int ContactId { get; set; }

   /// <summary>
   /// Message status for this recipient
   /// </summary>
   public MessageStatus Status { get; set; } = MessageStatus.Pending;

   /// <summary>
   /// Sent message ID (if sent)
   /// </summary>
   public int? MessageId { get; set; }

   /// <summary>
   /// Sent at
   /// </summary>
   public DateTime? SentAt { get; set; }

   /// <summary>
   /// Delivered at
   /// </summary>
   public DateTime? DeliveredAt { get; set; }

   /// <summary>
   /// Read at
   /// </summary>
   public DateTime? ReadAt { get; set; }

   /// <summary>
   /// Error message if failed
   /// </summary>
   public string? ErrorMessage { get; set; }

   // Navigation
   public virtual Campaign Campaign { get; set; } = null!;
   public virtual Contact Contact { get; set; } = null!;
   public virtual Message? Message { get; set; }
}
