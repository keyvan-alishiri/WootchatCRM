namespace WootchatCRM.Core.Entities;

public class Attachment : BaseEntity
{
   // ═══════════════ Properties ═══════════════

   /// <summary>
   /// نام فایل اصلی
   /// </summary>
   public string FileName { get; set; } = string.Empty;

   /// <summary>
   /// نوع فایل: image/png, application/pdf, etc.
   /// </summary>
   public string? FileType { get; set; }

   /// <summary>
   /// آدرس فایل (محلی یا URL)
   /// </summary>
   public string Url { get; set; } = string.Empty;

   /// <summary>
   /// حجم فایل به بایت
   /// </summary>
   public long? FileSize { get; set; }

   // ═══════════════ Foreign Keys ═══════════════

   public int MessageId { get; set; }

   // ═══════════════ Navigation Properties ═══════════════

   public virtual Message Message { get; set; } = null!;
}
