using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TL;
using WTelegram;

namespace WootchatCRM.Infrastructure.Telegram;

/// <summary>
/// پیاده‌سازی سرویس Telegram با WTelegramClient
/// </summary>
public class TelegramClientService : ITelegramClientService
{
   private readonly ILogger<TelegramClientService> _logger;
   private Client? _client;
   private User? _currentUser;
   private string? _pendingPhoneNumber;
   private string? _verificationCodeHash;

   // تنظیمات Telegram API (از my.telegram.org)
   private readonly int _apiId;
   private readonly string _apiHash;

   public bool IsConnected => _currentUser != null;

   public TelegramClientService(
       ILogger<TelegramClientService> logger,
       int apiId,
       string apiHash)
   {
      _logger = logger;
      _apiId = apiId;
      _apiHash = apiHash;
   }

   /// <summary>
   /// اتصال به Telegram
   /// </summary>
   public async Task<TelegramLoginResult> ConnectAsync(string phoneNumber)
   {
      try
      {
         _pendingPhoneNumber = NormalizePhoneNumber(phoneNumber);

         // ایجاد کلاینت
         _client = new Client(Config);

         // شروع لاگین
         var result = await _client.Login(_pendingPhoneNumber);

         if (result == null)
         {
            // لاگین موفق (session قبلی)
            _currentUser = _client.User;
            _logger.LogInformation("Telegram: Logged in as {Name}", _currentUser.first_name);

            return new TelegramLoginResult
            {
               Success = true,
               State = TelegramLoginState.LoggedIn,
               PhoneNumber = _pendingPhoneNumber
            };
         }

         // نیاز به کد تأیید
         _logger.LogInformation("Telegram: Verification code sent to {Phone}", _pendingPhoneNumber);

         return new TelegramLoginResult
         {
            Success = true,
            State = TelegramLoginState.NeedVerificationCode,
            PhoneNumber = _pendingPhoneNumber
         };
      }
      catch (Exception ex)
      {
         _logger.LogError(ex, "Telegram: Connection failed");

         return new TelegramLoginResult
         {
            Success = false,
            State = TelegramLoginState.Error,
            ErrorMessage = ex.Message
         };
      }
   }

   /// <summary>
   /// تأیید کد ورود
   /// </summary>
   public async Task<TelegramLoginResult> VerifyCodeAsync(string verificationCode)
   {
      try
      {
         if (_client == null)
         {
            return new TelegramLoginResult
            {
               Success = false,
               State = TelegramLoginState.Error,
               ErrorMessage = "Client not initialized"
            };
         }

         var result = await _client.Login(verificationCode);

         if (result == null)
         {
            // لاگین موفق
            _currentUser = _client.User;
            _logger.LogInformation("Telegram: Logged in as {Name}", _currentUser.first_name);

            return new TelegramLoginResult
            {
               Success = true,
               State = TelegramLoginState.LoggedIn
            };
         }

         // نیاز به 2FA
         if (result == "2FA")
         {
            return new TelegramLoginResult
            {
               Success = true,
               State = TelegramLoginState.Need2FAPassword
            };
         }

         return new TelegramLoginResult
         {
            Success = false,
            State = TelegramLoginState.Error,
            ErrorMessage = "Unexpected login state"
         };
      }
      catch (Exception ex)
      {
         _logger.LogError(ex, "Telegram: Verification failed");

         return new TelegramLoginResult
         {
            Success = false,
            State = TelegramLoginState.Error,
            ErrorMessage = ex.Message
         };
      }
   }

   /// <summary>
   /// تأیید رمز 2FA
   /// </summary>
   public async Task<TelegramLoginResult> Verify2FAAsync(string password)
   {
      try
      {
         if (_client == null)
         {
            return new TelegramLoginResult
            {
               Success = false,
               State = TelegramLoginState.Error,
               ErrorMessage = "Client not initialized"
            };
         }

         var result = await _client.Login(password);

         if (result == null)
         {
            _currentUser = _client.User;
            _logger.LogInformation("Telegram: Logged in with 2FA as {Name}", _currentUser.first_name);

            return new TelegramLoginResult
            {
               Success = true,
               State = TelegramLoginState.LoggedIn
            };
         }

         return new TelegramLoginResult
         {
            Success = false,
            State = TelegramLoginState.Error,
            ErrorMessage = "2FA verification failed"
         };
      }
      catch (Exception ex)
      {
         _logger.LogError(ex, "Telegram: 2FA verification failed");

         return new TelegramLoginResult
         {
            Success = false,
            State = TelegramLoginState.Error,
            ErrorMessage = ex.Message
         };
      }
   }

   /// <summary>
   /// ارسال پیام به شماره تلفن
   /// </summary>
   public async Task<TelegramSendResult> SendMessageAsync(string phoneNumber, string message)
   {
      try
      {
         if (_client == null || !IsConnected)
         {
            return new TelegramSendResult
            {
               Success = false,
               ErrorMessage = "Not connected to Telegram"
            };
         }

         var normalizedPhone = NormalizePhoneNumber(phoneNumber);

         // جستجوی کاربر با شماره تلفن
         var contacts = await _client.Contacts_ImportContacts(new[]
         {
                new InputPhoneContact
                {
                    phone = normalizedPhone,
                    first_name = "Contact",
                    last_name = ""
                }
            });

         if (contacts.users.Count == 0)
         {
            return new TelegramSendResult
            {
               Success = false,
               ErrorMessage = $"User with phone {phoneNumber} not found on Telegram"
            };
         }

         var user = contacts.users.Values.First();

         // ارسال پیام
         var sentMessage = await _client.SendMessageAsync(user, message);

         _logger.LogInformation("Telegram: Message sent to {Phone}, MessageId: {Id}",
             phoneNumber, sentMessage.ID);

         return new TelegramSendResult
         {
            Success = true,
            MessageId = sentMessage.ID,
            SentAt = DateTime.Now
         };
      }
      catch (Exception ex)
      {
         _logger.LogError(ex, "Telegram: Failed to send message to {Phone}", phoneNumber);

         return new TelegramSendResult
         {
            Success = false,
            ErrorMessage = ex.Message
         };
      }
   }

   /// <summary>
   /// ارسال پیام به یوزرنیم
   /// </summary>
   public async Task<TelegramSendResult> SendMessageByUsernameAsync(string username, string message)
   {
      try
      {
         if (_client == null || !IsConnected)
         {
            return new TelegramSendResult
            {
               Success = false,
               ErrorMessage = "Not connected to Telegram"
            };
         }

         // حذف @ از ابتدا
         username = username.TrimStart('@');

         // پیدا کردن کاربر
         var resolved = await _client.Contacts_ResolveUsername(username);

         if (resolved.User == null)
         {
            return new TelegramSendResult
            {
               Success = false,
               ErrorMessage = $"Username @{username} not found"
            };
         }

         // ارسال پیام
         var sentMessage = await _client.SendMessageAsync(resolved.User, message);

         _logger.LogInformation("Telegram: Message sent to @{Username}, MessageId: {Id}",
             username, sentMessage.ID);

         return new TelegramSendResult
         {
            Success = true,
            MessageId = sentMessage.ID,
            SentAt = DateTime.Now
         };
      }
      catch (Exception ex)
      {
         _logger.LogError(ex, "Telegram: Failed to send message to @{Username}", username);

         return new TelegramSendResult
         {
            Success = false,
            ErrorMessage = ex.Message
         };
      }
   }

   /// <summary>
   /// قطع اتصال
   /// </summary>
   public async Task DisconnectAsync()
   {
      if (_client != null)
      {
         await _client.Auth_LogOut();
         _client.Dispose();
         _client = null;
         _currentUser = null;
         _logger.LogInformation("Telegram: Disconnected");
      }
   }

   /// <summary>
   /// پاکسازی منابع
   /// </summary>
   public async ValueTask DisposeAsync()
   {
      if (_client != null)
      {
         _client.Dispose();
         _client = null;
      }
   }

   #region Private Methods

   /// <summary>
   /// تنظیمات WTelegramClient
   /// </summary>
   private string? Config(string what)
   {
      return what switch
      {
         "api_id" => _apiId.ToString(),
         "api_hash" => _apiHash,
         "phone_number" => _pendingPhoneNumber,
         "session_pathname" => Path.Combine(
             Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
             "WootchatCRM",
             "telegram.session"),
         _ => null
      };
   }

   /// <summary>
   /// نرمال‌سازی شماره تلفن
   /// </summary>
   private static string NormalizePhoneNumber(string phone)
   {
      // حذف کاراکترهای اضافی
      phone = new string(phone.Where(c => char.IsDigit(c) || c == '+').ToArray());

      // اضافه کردن + اگر نداشت
      if (!phone.StartsWith("+"))
      {
         // فرض: شماره ایرانی
         if (phone.StartsWith("0"))
            phone = "+98" + phone.Substring(1);
         else if (phone.StartsWith("98"))
            phone = "+" + phone;
         else
            phone = "+98" + phone;
      }

      return phone;
   }

   #endregion
}
