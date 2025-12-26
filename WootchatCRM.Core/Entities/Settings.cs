namespace WootchatCRM.Core.Entities;

public class Settings : BaseEntity
{
   /// <summary>
   /// Setting key (unique)
   /// </summary>
   public string Key { get; set; } = string.Empty;

   /// <summary>
   /// Setting value (can be JSON for complex values)
   /// </summary>
   public string? Value { get; set; }

   /// <summary>
   /// Setting category for grouping
   /// </summary>
   public string Category { get; set; } = "General";

   /// <summary>
   /// Value type hint: String, Integer, Boolean, Json, Encrypted
   /// </summary>
   public string ValueType { get; set; } = "String";

   /// <summary>
   /// Setting description
   /// </summary>
   public string? Description { get; set; }

   /// <summary>
   /// Is this a system setting (non-deletable)?
   /// </summary>
   public bool IsSystem { get; set; } = false;

   /// <summary>
   /// Is value encrypted?
   /// </summary>
   public bool IsEncrypted { get; set; } = false;
}
