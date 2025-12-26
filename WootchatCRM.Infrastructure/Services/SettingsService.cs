using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using WootchatCRM.Core.Entities;
using WootchatCRM.Core.Interfaces;
using WootchatCRM.Core.Interfaces.Services;
using WootchatCRM.Infrastructure.Data;

namespace WootchatCRM.Infrastructure.Services;

public class SettingsService : ISettingsService
{
   private readonly AppDbContext _context;
   private readonly IRepository<Settings> _repository;

   // Cache ساده برای کاهش Query ها
   private static Dictionary<string, string?>? _cache;
   private static DateTime _cacheExpiry = DateTime.MinValue;
   private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(5);

   public SettingsService(AppDbContext context, IRepository<Settings> repository)
   {
      _context = context;
      _repository = repository;
   }

   // ═══════════════════════════════════════════════════════════════
   // Single Value Operations
   // ═══════════════════════════════════════════════════════════════

   public string? GetValue(string key)
   {
      return GetValueAsync(key).GetAwaiter().GetResult();
   }

   public async Task<string?> GetValueAsync(string key)
   {
      var setting = await _context.Settings
          .AsNoTracking()
          .FirstOrDefaultAsync(s => s.Key == key);

      if (setting == null) return null;

      return DecryptIfNeeded(setting);
   }

   public string? GetValue(string category, string key)
   {
      return GetValueAsync(category, key).GetAwaiter().GetResult();
   }

   public async Task<string?> GetValueAsync(string category, string key)
   {
      var setting = await _context.Settings
          .AsNoTracking()
          .FirstOrDefaultAsync(s => s.Category == category && s.Key == key);

      if (setting == null) return null;

      return DecryptIfNeeded(setting);
   }

   // ═══════════════════════════════════════════════════════════════
   // Typed Value Getters
   // ═══════════════════════════════════════════════════════════════

   public int GetInt(string key, int defaultValue = 0)
   {
      var value = GetValue(key);
      return int.TryParse(value, out var result) ? result : defaultValue;
   }

   public bool GetBool(string key, bool defaultValue = false)
   {
      var value = GetValue(key);
      if (string.IsNullOrEmpty(value)) return defaultValue;

      return value.ToLower() switch
      {
         "true" or "1" or "yes" => true,
         "false" or "0" or "no" => false,
         _ => defaultValue
      };
   }

   public T? GetJson<T>(string key) where T : class
   {
      var value = GetValue(key);
      if (string.IsNullOrEmpty(value)) return null;

      try
      {
         return JsonSerializer.Deserialize<T>(value, new JsonSerializerOptions
         {
            PropertyNameCaseInsensitive = true
         });
      }
      catch
      {
         return null;
      }
   }

   // ═══════════════════════════════════════════════════════════════
   // Entity Operations
   // ═══════════════════════════════════════════════════════════════

   public async Task<Settings?> GetByKeyAsync(string key)
   {
      return await _context.Settings
          .FirstOrDefaultAsync(s => s.Key == key);
   }

   public async Task<IEnumerable<Settings>> GetByCategoryAsync(string category)
   {
      return await _context.Settings
          .AsNoTracking()
          .Where(s => s.Category == category)
          .OrderBy(s => s.Key)
          .ToListAsync();
   }

   public async Task<Dictionary<string, string?>> GetCategoryAsDictionaryAsync(string category)
   {
      var settings = await GetByCategoryAsync(category);
      return settings.ToDictionary(
          s => s.Key,
          s => DecryptIfNeeded(s)
      );
   }

   // ═══════════════════════════════════════════════════════════════
   // Set Operations
   // ═══════════════════════════════════════════════════════════════

   public async Task SetValueAsync(string key, string? value, string category = "General",
       string valueType = "String", bool isEncrypted = false, string? description = null)
   {
      var existing = await _context.Settings
          .FirstOrDefaultAsync(s => s.Key == key);

      var finalValue = isEncrypted ? EncryptValue(value) : value;

      if (existing != null)
      {
         existing.Value = finalValue;
         existing.Category = category;
         existing.ValueType = valueType;
         existing.IsEncrypted = isEncrypted;
         existing.UpdatedAt = DateTime.UtcNow;

         if (description != null)
            existing.Description = description;

         _context.Settings.Update(existing);
      }
      else
      {
         var setting = new Settings
         {
            Key = key,
            Value = finalValue,
            Category = category,
            ValueType = valueType,
            IsEncrypted = isEncrypted,
            Description = description,
            IsSystem = false,
            CreatedAt = DateTime.UtcNow
         };

         await _context.Settings.AddAsync(setting);
      }

      await _context.SaveChangesAsync();
      InvalidateCache();
   }

   public async Task SetJsonAsync<T>(string key, T value, string category = "General") where T : class
   {
      var json = JsonSerializer.Serialize(value, new JsonSerializerOptions
      {
         WriteIndented = false,
         PropertyNamingPolicy = JsonNamingPolicy.CamelCase
      });

      await SetValueAsync(key, json, category, "Json");
   }

   public async Task SetBulkAsync(Dictionary<string, string?> settings, string category = "General")
   {
      foreach (var kvp in settings)
      {
         await SetValueAsync(kvp.Key, kvp.Value, category);
      }
   }

   // ═══════════════════════════════════════════════════════════════
   // Delete Operations
   // ═══════════════════════════════════════════════════════════════

   public async Task<bool> DeleteAsync(string key)
   {
      var setting = await _context.Settings
          .FirstOrDefaultAsync(s => s.Key == key);

      if (setting == null) return false;

      // تنظیمات سیستمی قابل حذف نیستند
      if (setting.IsSystem) return false;

      _context.Settings.Remove(setting);
      await _context.SaveChangesAsync();
      InvalidateCache();

      return true;
   }

   public async Task<int> DeleteCategoryAsync(string category)
   {
      var settings = await _context.Settings
          .Where(s => s.Category == category && !s.IsSystem)
          .ToListAsync();

      if (!settings.Any()) return 0;

      _context.Settings.RemoveRange(settings);
      await _context.SaveChangesAsync();
      InvalidateCache();

      return settings.Count;
   }

   // ═══════════════════════════════════════════════════════════════
   // Utility
   // ═══════════════════════════════════════════════════════════════

   public async Task<bool> ExistsAsync(string key)
   {
      return await _context.Settings
          .AnyAsync(s => s.Key == key);
   }

   public async Task<IEnumerable<string>> GetCategoriesAsync()
   {
      return await _context.Settings
          .Select(s => s.Category)
          .Distinct()
          .OrderBy(c => c)
          .ToListAsync();
   }

   // ═══════════════════════════════════════════════════════════════
   // Chatwoot Specific Helpers
   // ═══════════════════════════════════════════════════════════════

   private const string ChatwootCategory = "Chatwoot";

   public async Task<ChatwootSettings> GetChatwootSettingsAsync()
   {
      var settings = await GetCategoryAsDictionaryAsync(ChatwootCategory);

      return new ChatwootSettings
      {
         BaseUrl = settings.GetValueOrDefault("BaseUrl") ?? string.Empty,
         ApiKey = settings.GetValueOrDefault("ApiKey") ?? string.Empty,
         AccountId = int.TryParse(settings.GetValueOrDefault("AccountId"), out var id) ? id : 1,
         DefaultInboxId = int.TryParse(settings.GetValueOrDefault("DefaultInboxId"), out var inbox) ? inbox : null
      };
   }

   public async Task SaveChatwootSettingsAsync(ChatwootSettings settings)
   {
      await SetValueAsync(
          key: "BaseUrl",
          value: settings.BaseUrl,
          category: ChatwootCategory,
          valueType: "String",
          isEncrypted: false,
          description: "Chatwoot server base URL"
      );

      await SetValueAsync(
          key: "ApiKey",
          value: settings.ApiKey,
          category: ChatwootCategory,
          valueType: "Encrypted",
          isEncrypted: true,
          description: "Chatwoot API access token"
      );

      await SetValueAsync(
          key: "AccountId",
          value: settings.AccountId.ToString(),
          category: ChatwootCategory,
          valueType: "Integer",
          isEncrypted: false,
          description: "Chatwoot account ID"
      );

      if (settings.DefaultInboxId.HasValue)
      {
         await SetValueAsync(
             key: "DefaultInboxId",
             value: settings.DefaultInboxId.Value.ToString(),
             category: ChatwootCategory,
             valueType: "Integer",
             isEncrypted: false,
             description: "Default inbox ID for Chatwoot"
         );
      }
   }

   // ═══════════════════════════════════════════════
   // Encryption Helpers (Placeholder)
   // ═══════════════════════════════════════════════

   private string? EncryptValue(string? value)
   {
      if (string.IsNullOrEmpty(value)) return value;

      // TODO: پیاده‌سازی واقعی (DPAPI / AES)
      return Convert.ToBase64String(
          System.Text.Encoding.UTF8.GetBytes(value)
      );
   }

   private string? DecryptIfNeeded(Settings setting)
   {
      if (!setting.IsEncrypted || string.IsNullOrEmpty(setting.Value))
         return setting.Value;

      try
      {
         var bytes = Convert.FromBase64String(setting.Value);
         return System.Text.Encoding.UTF8.GetString(bytes);
      }
      catch
      {
         return null;
      }
   }

   private void InvalidateCache()
   {
      _cache = null;
      _cacheExpiry = DateTime.MinValue;
   }
}
