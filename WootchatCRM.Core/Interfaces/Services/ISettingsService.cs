using WootchatCRM.Core.Entities;

namespace WootchatCRM.Core.Interfaces.Services;

public interface ISettingsService
{
   // ═══════════════════════════════════════════════
   // Single Value Operations
   // ═══════════════════════════════════════════════

   /// <summary>
   /// دریافت مقدار یک تنظیم بر اساس کلید
   /// </summary>
   string? GetValue(string key);

   /// <summary>
   /// دریافت مقدار یک تنظیم بر اساس کلید (Async)
   /// </summary>
   Task<string?> GetValueAsync(string key);

   /// <summary>
   /// دریافت مقدار یک تنظیم بر اساس دسته‌بندی و کلید
   /// </summary>
   string? GetValue(string category, string key);

   /// <summary>
   /// دریافت مقدار یک تنظیم بر اساس دسته‌بندی و کلید (Async)
   /// </summary>
   Task<string?> GetValueAsync(string category, string key);

   // ═══════════════════════════════════════════════
   // Typed Value Getters
   // ═══════════════════════════════════════════════

   /// <summary>
   /// دریافت مقدار به صورت Integer
   /// </summary>
   int GetInt(string key, int defaultValue = 0);

   /// <summary>
   /// دریافت مقدار به صورت Boolean
   /// </summary>
   bool GetBool(string key, bool defaultValue = false);

   /// <summary>
   /// دریافت مقدار به صورت JSON و Deserialize
   /// </summary>
   T? GetJson<T>(string key) where T : class;

   // ═══════════════════════════════════════════════
   // Entity Operations
   // ═══════════════════════════════════════════════

   /// <summary>
   /// دریافت Entity کامل تنظیم
   /// </summary>
   Task<Settings?> GetByKeyAsync(string key);

   /// <summary>
   /// دریافت تمام تنظیمات یک دسته‌بندی
   /// </summary>
   Task<IEnumerable<Settings>> GetByCategoryAsync(string category);

   /// <summary>
   /// دریافت تمام تنظیمات یک دسته به صورت Dictionary
   /// </summary>
   Task<Dictionary<string, string?>> GetCategoryAsDictionaryAsync(string category);

   // ═══════════════════════════════════════════════
   // Set Operations
   // ═══════════════════════════════════════════════

   /// <summary>
   /// ذخیره یا بروزرسانی یک تنظیم
   /// </summary>
   Task SetValueAsync(string key, string? value, string category = "General",
       string valueType = "String", bool isEncrypted = false, string? description = null);

   /// <summary>
   /// ذخیره مقدار JSON
   /// </summary>
   Task SetJsonAsync<T>(string key, T value, string category = "General") where T : class;

   /// <summary>
   /// ذخیره چندین تنظیم به صورت یکجا
   /// </summary>
   Task SetBulkAsync(Dictionary<string, string?> settings, string category = "General");

   // ═══════════════════════════════════════════════
   // Delete Operations
   // ═══════════════════════════════════════════════

   /// <summary>
   /// حذف یک تنظیم (فقط غیر سیستمی)
   /// </summary>
   Task<bool> DeleteAsync(string key);

   /// <summary>
   /// حذف تمام تنظیمات یک دسته (فقط غیر سیستمی)
   /// </summary>
   Task<int> DeleteCategoryAsync(string category);

   // ═══════════════════════════════════════════════
   // Utility
   // ═══════════════════════════════════════════════

   /// <summary>
   /// بررسی وجود یک تنظیم
   /// </summary>
   Task<bool> ExistsAsync(string key);

   /// <summary>
   /// دریافت تمام دسته‌بندی‌های موجود
   /// </summary>
   Task<IEnumerable<string>> GetCategoriesAsync();

   // ═══════════════════════════════════════════════
   // Chatwoot Specific Helpers
   // ═══════════════════════════════════════════════

   /// <summary>
   /// دریافت تنظیمات Chatwoot
   /// </summary>
   Task<ChatwootSettings> GetChatwootSettingsAsync();

   /// <summary>
   /// ذخیره تنظیمات Chatwoot
   /// </summary>
   Task SaveChatwootSettingsAsync(ChatwootSettings settings);
}

/// <summary>
/// DTO برای تنظیمات Chatwoot
/// </summary>
public class ChatwootSettings
{
   public string BaseUrl { get; set; } = string.Empty;
   public string ApiKey { get; set; } = string.Empty;
   public int AccountId { get; set; } = 1;
   public int? DefaultInboxId { get; set; }
   public bool IsConfigured => !string.IsNullOrEmpty(BaseUrl) && !string.IsNullOrEmpty(ApiKey);
}
