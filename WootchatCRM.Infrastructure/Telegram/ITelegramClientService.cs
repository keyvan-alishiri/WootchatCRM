using System;
using System.Threading.Tasks;

namespace WootchatCRM.Infrastructure.Telegram;

/// <summary>
/// سرویس ارسال پیام مستقیم از طریق Telegram Client API
/// با استفاده از WTelegramClient
/// </summary>
public interface ITelegramClientService : IAsyncDisposable
{
   /// <summary>
   /// وضعیت اتصال
   /// </summary>
   bool IsConnected { get; }

   /// <summary>
   /// اتصال به Telegram با شماره تلفن
   /// </summary>
   /// <param name="phoneNumber">شماره تلفن با کد کشور (مثال: +989123456789)</param>
   Task<TelegramLoginResult> ConnectAsync(string phoneNumber);

   /// <summary>
   /// تأیید کد ورود
   /// </summary>
   /// <param name="verificationCode">کد دریافتی از Telegram</param>
   Task<TelegramLoginResult> VerifyCodeAsync(string verificationCode);

   /// <summary>
   /// تأیید رمز دو مرحله‌ای (در صورت فعال بودن)
   /// </summary>
   /// <param name="password">رمز عبور 2FA</param>
   Task<TelegramLoginResult> Verify2FAAsync(string password);

   /// <summary>
   /// ارسال پیام به شماره تلفن
   /// </summary>
   /// <param name="phoneNumber">شماره تلفن مقصد</param>
   /// <param name="message">متن پیام</param>
   Task<TelegramSendResult> SendMessageAsync(string phoneNumber, string message);

   /// <summary>
   /// ارسال پیام به یوزرنیم
   /// </summary>
   /// <param name="username">یوزرنیم بدون @</param>
   /// <param name="message">متن پیام</param>
   Task<TelegramSendResult> SendMessageByUsernameAsync(string username, string message);

   /// <summary>
   /// قطع اتصال
   /// </summary>
   Task DisconnectAsync();
}

#region Result Models

public class TelegramLoginResult
{
   public bool Success { get; set; }
   public TelegramLoginState State { get; set; }
   public string? ErrorMessage { get; set; }
   public string? PhoneNumber { get; set; }
}

public enum TelegramLoginState
{
   /// <summary>
   /// نیاز به شماره تلفن
   /// </summary>
   NeedPhoneNumber,

   /// <summary>
   /// نیاز به کد تأیید
   /// </summary>
   NeedVerificationCode,

   /// <summary>
   /// نیاز به رمز 2FA
   /// </summary>
   Need2FAPassword,

   /// <summary>
   /// ورود موفق
   /// </summary>
   LoggedIn,

   /// <summary>
   /// خطا
   /// </summary>
   Error
}

public class TelegramSendResult
{
   public bool Success { get; set; }
   public int? MessageId { get; set; }
   public string? ErrorMessage { get; set; }
   public DateTime? SentAt { get; set; }
}

#endregion
