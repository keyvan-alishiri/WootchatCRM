using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WootchatCRM.Core.Entities
{
   public class Inbox : BaseEntity
   {
      public int ChannelId { get; set; }

      /// <summary>
      /// Chatwoot Inbox ID
      /// </summary>
      public int? ChatwootInboxId { get; set; }

      public string Name { get; set; } = string.Empty;

      /// <summary>
      /// Phone number, email, bot username, etc.
      /// </summary>
      public string? Identifier { get; set; }

      public bool IsActive { get; set; } = true;

      /// <summary>
      /// Webhook URL for this inbox
      /// </summary>
      public string? WebhookUrl { get; set; }

      // Navigation
      public virtual Channel Channel { get; set; } = null!;
      public virtual ICollection<Conversation> Conversations { get; set; } = new List<Conversation>();
   }
}
