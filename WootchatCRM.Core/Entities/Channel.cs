using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WootchatCRM.Core.Enums;

namespace WootchatCRM.Core.Entities
{
   public class Channel : BaseEntity
   {
      public string Name { get; set; } = string.Empty;
      public ChannelType Type { get; set; }
      public bool IsActive { get; set; } = true;

      /// <summary>
      /// JSON configuration for channel-specific settings
      /// API keys, tokens, etc.
      /// </summary>
      public string? Configuration { get; set; }

      // Navigation
      public virtual ICollection<Inbox> Inboxes { get; set; } = new List<Inbox>();
   }
}
