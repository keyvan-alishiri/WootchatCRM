namespace WootchatCRM.Core.Entities;

public class Contact : BaseEntity
{
   // ═══════════════ Properties ═══════════════

   public string Name { get; set; } = string.Empty;

   /// <summary>
   /// شماره تلفن - برای واتساپ/SMS
   /// </summary>
   public string? PhoneNumber { get; set; }

   public string? Email { get; set; }
   public string? Company { get; set; }
   public string? Avatar { get; set; }

   /// <summary>
   /// شناسه مخاطب در Chatwoot
   /// </summary>
   public int? ChatwootContactId { get; set; }

   /// <summary>
   /// فیلدهای سفارشی به صورت JSON
   /// </summary>
   public string? CustomFields { get; set; }

   // ═══════════════ Navigation Properties ═══════════════

   public virtual ICollection<Conversation> Conversations { get; set; } = new List<Conversation>();
   public virtual ICollection<Note> Notes { get; set; } = new List<Note>();
   public virtual ICollection<CrmTask> CrmTasks { get; set; } = new List<CrmTask>();
   public virtual ICollection<ContactTag> ContactTags { get; set; } = new List<ContactTag>();
}
