using WootchatCRM.Core.Enums;

namespace WootchatCRM.Core.Entities;

public class Campaign : BaseEntity
{
   /// <summary>
   /// Campaign name
   /// </summary>
   public string Name { get; set; } = string.Empty;

   /// <summary>
   /// Campaign description
   /// </summary>
   public string? Description { get; set; }

   /// <summary>
   /// Channel type for this campaign
   /// </summary>
   public ChannelType ChannelType { get; set; }

   /// <summary>
   /// Inbox ID (optional - specific inbox)
   /// </summary>
   public int? InboxId { get; set; }

   /// <summary>
   /// Message template content
   /// </summary>
   public string MessageTemplate { get; set; } = string.Empty;

   /// <summary>
   /// Scheduled start time
   /// </summary>
   public DateTime? ScheduledAt { get; set; }

   /// <summary>
   /// Campaign status
   /// </summary>
   public CampaignStatus Status { get; set; } = CampaignStatus.Draft;

   /// <summary>
   /// Total recipients count
   /// </summary>
   public int TotalRecipients { get; set; } = 0;

   /// <summary>
   /// Successfully sent count
   /// </summary>
   public int SentCount { get; set; } = 0;

   /// <summary>
   /// Failed count
   /// </summary>
   public int FailedCount { get; set; } = 0;

   /// <summary>
   /// Delivered count
   /// </summary>
   public int DeliveredCount { get; set; } = 0;

   /// <summary>
   /// Read count
   /// </summary>
   public int ReadCount { get; set; } = 0;

   /// <summary>
   /// Campaign started at
   /// </summary>
   public DateTime? StartedAt { get; set; }

   /// <summary>
   /// Campaign completed at
   /// </summary>
   public DateTime? CompletedAt { get; set; }

   /// <summary>
   /// Creator user ID
   /// </summary>
   public int CreatedById { get; set; }

   /// <summary>
   /// Filter criteria JSON (contacts selection)
   /// </summary>
   public string? FilterCriteria { get; set; }

   // Navigation
   public virtual Inbox? Inbox { get; set; }
   public virtual User CreatedBy { get; set; } = null!;
   public virtual ICollection<CampaignRecipient> Recipients { get; set; } = new List<CampaignRecipient>();
}
