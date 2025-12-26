using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WootchatCRM.Core.Entities
{
   public class Team : BaseEntity
   {
      public string Name { get; set; } = string.Empty;
      public string? Description { get; set; }

      /// <summary>
      /// Chatwoot team ID
      /// </summary>
      public int? ChatwootTeamId { get; set; }

      // Navigation
      public virtual ICollection<User> Members { get; set; } = new List<User>();
      public virtual ICollection<Conversation> Conversations { get; set; } = new List<Conversation>();
   }
}
