using WootchatCRM.Core.Entities;
using WootchatCRM.Core.Interfaces.Services;
using WootchatCRM.Infrastructure.Chatwoot;
using WootchatCRM.Infrastructure.Chatwoot.DTOs;
using WootchatCRM.Infrastructure.Telegram;

namespace WootchatCRM.WinForms.Contacts
{
   public partial class NewConversationForm : Form
   {
      #region Fields

      private readonly Contact _contact;
      private readonly IChatwootApiClient _chatwootService;
      private readonly ISettingsService _settingsService;
      private readonly ITelegramClientService? _telegramClientService;
      private readonly IServiceProvider _serviceProvider;

      private List<ChatwootInbox> _inboxes = new();
      private int _accountId;
      private bool _inboxesLoaded = false;

      #endregion

      #region Constructor

      public NewConversationForm(
          Contact contact,
          IChatwootApiClient chatwootService,
          ISettingsService settingsService,
          IServiceProvider serviceProvider,
          ITelegramClientService? telegramClientService = null)
      {
         InitializeComponent();

         _contact = contact ?? throw new ArgumentNullException(nameof(contact));
         _chatwootService = chatwootService ?? throw new ArgumentNullException(nameof(chatwootService));
         _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
         _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
         _telegramClientService = telegramClientService;

         SetupForm();
         WireEvents();
      }

      #endregion

      #region Setup

      private void SetupForm()
      {
         // نمایش اطلاعات مخاطب - استفاده از Name (نه FirstName/LastName)
         lblContactName.Text = !string.IsNullOrWhiteSpace(_contact.Name)
             ? _contact.Name
             : "---";

         // استفاده از PhoneNumber (نه Mobile)
         lblContactPhone.Text = !string.IsNullOrWhiteSpace(_contact.PhoneNumber)
             ? _contact.PhoneNumber
             : "---";

         lblContactEmail.Text = !string.IsNullOrWhiteSpace(_contact.Email)
             ? _contact.Email
             : "---";

         // تنظیم RadioButton پیش‌فرض
         rdoChatwoot.Checked = true;

         // شمارنده کاراکتر
         UpdateCharCount();
      }

      private void WireEvents()
      {
         this.Load += NewConversationForm_Load;
         btnSend.Click += BtnSend_Click;
         btnCancel.Click += BtnCancel_Click;
         txtMessage.TextChanged += TxtMessage_TextChanged;
         rdoChatwoot.CheckedChanged += SendMethod_CheckedChanged;
         rdoTelegramDirect.CheckedChanged += SendMethod_CheckedChanged;
      }

      #endregion

      #region Load Data

      private async Task LoadInboxesAsync()
      {
         try
         {
            lblStatus.Text = "در حال بارگذاری کانال‌ها...";
            cmbInbox.Enabled = false;

            var result = await _chatwootService.GetInboxesAsync();

            if (result.IsSuccess && result.Data != null)
            {
               cmbInbox.DataSource = null;
               cmbInbox.DataSource = result.Data;
               cmbInbox.DisplayMember = "Name";
               cmbInbox.ValueMember = "Id";

               _inboxesLoaded = true;

               if (result.Data.Count > 0)
               {
                  cmbInbox.SelectedIndex = 0;
                  lblStatus.Text = $"{result.Data.Count} کانال بارگذاری شد";
               }
               else
               {
                  lblStatus.Text = "هیچ کانالی یافت نشد";
               }
            }
            else
            {
               lblStatus.Text = $"خطا: {result.ErrorMessage ?? "خطا در بارگذاری کانال‌ها"}";
            }
         }
         catch (Exception ex)
         {
            lblStatus.Text = $"خطا: {ex.Message}";
         }
         finally
         {
            cmbInbox.Enabled = rdoChatwoot.Checked;
         }
      }

      #endregion

      #region Form Load

      private async void NewConversationForm_Load(object? sender, EventArgs e)
      {
         await LoadDataAsync();
         if (rdoChatwoot.Checked)
         {
            await LoadInboxesAsync();
         }
      }

      private async Task LoadDataAsync()
      {
         try
         {
            SetLoading(true, "در حال بارگذاری...");

            // دریافت تنظیمات Chatwoot
            var settings = await _settingsService.GetChatwootSettingsAsync();

            if (settings == null || settings.AccountId <= 0)
            {
               MessageBox.Show(
                   "تنظیمات Chatwoot یافت نشد. لطفاً ابتدا تنظیمات را کامل کنید.",
                   "خطا",
                   MessageBoxButtons.OK,
                   MessageBoxIcon.Error,
                   MessageBoxDefaultButton.Button1,
                   MessageBoxOptions.RtlReading);
               this.Close();
               return;
            }

            _accountId = settings.AccountId;

            // دریافت لیست Inbox ها - بدون پارامتر (طبق Interface شما)
            var inboxResult = await _chatwootService.GetInboxesAsync();

            if (inboxResult.IsSuccess && inboxResult.Data != null)
            {
               _inboxes = inboxResult.Data.ToList();

               cmbInbox.DisplayMember = "Name";
               cmbInbox.ValueMember = "Id";
               cmbInbox.DataSource = _inboxes;

               if (_inboxes.Count > 0)
               {
                  cmbInbox.SelectedIndex = 0;
               }
            }
            else
            {
               MessageBox.Show(
                   $"خطا در دریافت لیست کانال‌ها:\n{inboxResult.ErrorMessage ?? "خطای نامشخص"}",
                   "خطا",
                   MessageBoxButtons.OK,
                   MessageBoxIcon.Warning,
                   MessageBoxDefaultButton.Button1,
                   MessageBoxOptions.RtlReading);
            }

            // بررسی وضعیت Telegram Client
            UpdateTelegramStatus();

            SetLoading(false, "آماده ارسال");
         }
         catch (Exception ex)
         {
            SetLoading(false, "خطا در بارگذاری");
            MessageBox.Show(
                $"خطا در بارگذاری:\n{ex.Message}",
                "خطا",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error,
                MessageBoxDefaultButton.Button1,
                MessageBoxOptions.RtlReading);
         }
      }

      private void UpdateTelegramStatus()
      {
         if (_telegramClientService == null)
         {
            rdoTelegramDirect.Enabled = false;
            rdoTelegramDirect.Text = "ارسال مستقیم Telegram (غیرفعال)";
         }
         else
         {
            // استفاده از IsConnected (طبق Interface شما)
            bool isConnected = _telegramClientService.IsConnected;
            rdoTelegramDirect.Enabled = true;

            rdoTelegramDirect.Text = isConnected
                ? "ارسال مستقیم Telegram ✓"
                : "ارسال مستقیم Telegram (نیاز به ورود)";
         }
      }

      #endregion

      #region Event Handlers

      private void SendMethod_CheckedChanged(object? sender, EventArgs e)
      {
         // فعال/غیرفعال کردن انتخاب Inbox
         bool isChatwoot = rdoChatwoot.Checked;

         lblInbox.Enabled = isChatwoot;
         lblInbox.Visible = isChatwoot;
         cmbInbox.Enabled = isChatwoot;
         cmbInbox.Visible = isChatwoot;

         // بروزرسانی وضعیت
         lblStatus.Text = isChatwoot
             ? "ارسال از طریق Chatwoot"
             : "ارسال مستقیم با Telegram";

         // بارگذاری Inbox ها اگر نیاز باشد
         if (isChatwoot && !_inboxesLoaded)
         {
            _ = LoadInboxesAsync();
         }
      }


      private void TxtMessage_TextChanged(object? sender, EventArgs e)
      {
         UpdateCharCount();
      }

      private void UpdateCharCount()
      {
         int length = txtMessage.Text?.Length ?? 0;
         lblCharCount.Text = $"{length} / 4096";

         // تغییر رنگ اگر نزدیک حد مجاز
         lblCharCount.ForeColor = length > 3500 ? Color.OrangeRed : Color.Gray;
      }

      private async void BtnSend_Click(object? sender, EventArgs e)
      {
         await SendMessageAsync();
      }

      private void BtnCancel_Click(object? sender, EventArgs e)
      {
         this.DialogResult = DialogResult.Cancel;
         this.Close();
      }

      #endregion
      #region Send Logic

      private async Task SendMessageAsync()
      {
         // اعتبارسنجی پیام
         string message = txtMessage.Text?.Trim() ?? string.Empty;

         if (string.IsNullOrWhiteSpace(message))
         {
            MessageBox.Show(
                "لطفاً متن پیام را وارد کنید.",
                "هشدار",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button1,
                MessageBoxOptions.RtlReading);
            txtMessage.Focus();
            return;
         }

         try
         {
            SetLoading(true, "در حال ارسال...");

            bool success;

            if (rdoChatwoot.Checked)
            {
               success = await SendViaChatwootAsync(message);
            }
            else
            {
               success = await SendViaTelegramDirectAsync(message);
            }

            if (success)
            {
               SetLoading(false, "پیام ارسال شد ✓");

               MessageBox.Show(
                   "پیام با موفقیت ارسال شد.",
                   "موفق",
                   MessageBoxButtons.OK,
                   MessageBoxIcon.Information,
                   MessageBoxDefaultButton.Button1,
                   MessageBoxOptions.RtlReading);

               this.DialogResult = DialogResult.OK;
               this.Close();
            }
            else
            {
               SetLoading(false, "خطا در ارسال");
            }
         }
         catch (Exception ex)
         {
            SetLoading(false, "خطا در ارسال");
            MessageBox.Show(
                $"خطا در ارسال پیام:\n{ex.Message}",
                "خطا",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error,
                MessageBoxDefaultButton.Button1,
                MessageBoxOptions.RtlReading);
         }
      }

      //private async Task<bool> SendViaChatwootAsync(string message)
      //{
      //   System.Diagnostics.Debug.WriteLine("========== SendViaChatwoot START ==========");

      //   // بررسی انتخاب Inbox
      //   if (cmbInbox.SelectedItem is not ChatwootInbox selectedInbox)
      //   {
      //      MessageBox.Show(
      //          "لطفاً یک کانال انتخاب کنید.",
      //          "هشدار",
      //          MessageBoxButtons.OK,
      //          MessageBoxIcon.Warning,
      //          MessageBoxDefaultButton.Button1,
      //          MessageBoxOptions.RtlReading);
      //      return false;
      //   }

      //   System.Diagnostics.Debug.WriteLine($"[INFO] Selected Inbox: {selectedInbox.Name} (ID: {selectedInbox.Id})");

      //   // بررسی یا ایجاد Contact در Chatwoot
      //   int chatwootContactId;

      //   System.Diagnostics.Debug.WriteLine($"[INFO] _contact.ChatwootContactId: {_contact.ChatwootContactId}");
      //   System.Diagnostics.Debug.WriteLine($"[INFO] _contact.PhoneNumber: [{_contact.PhoneNumber}]");
      //   System.Diagnostics.Debug.WriteLine($"[INFO] _contact.Email: [{_contact.Email}]");
      //   System.Diagnostics.Debug.WriteLine($"[INFO] _contact.Name: [{_contact.Name}]");

      //   if (_contact.ChatwootContactId.HasValue && _contact.ChatwootContactId.Value > 0)
      //   {
      //      chatwootContactId = _contact.ChatwootContactId.Value;
      //      System.Diagnostics.Debug.WriteLine($"[STEP 1] ✅ Using existing ChatwootContactId from local DB: {chatwootContactId}");
      //   }
      //   else
      //   {
      //      System.Diagnostics.Debug.WriteLine("[STEP 1] No local ChatwootContactId, searching in Chatwoot...");

      //      // جستجوی Contact موجود در Chatwoot
      //      ChatwootContact? existingContact = null;

      //      // جستجو با شماره تلفن
      //      if (!string.IsNullOrWhiteSpace(_contact.PhoneNumber))
      //      {
      //         var e164Phone = ConvertToE164(_contact.PhoneNumber);
      //         System.Diagnostics.Debug.WriteLine($"[STEP 2] Searching by phone: [{_contact.PhoneNumber}] -> E164: [{e164Phone}]");

      //         var searchResult = await _chatwootService.SearchContactByPhoneAsync(e164Phone);

      //         System.Diagnostics.Debug.WriteLine($"[STEP 2] Search Result - IsSuccess: {searchResult.IsSuccess}");
      //         System.Diagnostics.Debug.WriteLine($"[STEP 2] Search Result - Data is null: {searchResult.Data == null}");
      //         System.Diagnostics.Debug.WriteLine($"[STEP 2] Search Result - Error: {searchResult.ErrorMessage ?? "none"}");

      //         if (searchResult.IsSuccess && searchResult.Data != null)
      //         {
      //            existingContact = searchResult.Data;
      //            System.Diagnostics.Debug.WriteLine($"[STEP 2] ✅ FOUND by phone! Contact ID: {existingContact.Id}, Name: {existingContact.Name}");
      //         }
      //         else
      //         {
      //            System.Diagnostics.Debug.WriteLine("[STEP 2] ❌ Not found by phone");
      //         }
      //      }

      //      // اگر پیدا نشد، جستجو با ایمیل
      //      if (existingContact == null && !string.IsNullOrWhiteSpace(_contact.Email))
      //      {
      //         System.Diagnostics.Debug.WriteLine($"[STEP 3] Searching by email: [{_contact.Email}]");

      //         var searchResult = await _chatwootService.SearchContactByEmailAsync(_contact.Email);

      //         System.Diagnostics.Debug.WriteLine($"[STEP 3] Search Result - IsSuccess: {searchResult.IsSuccess}");
      //         System.Diagnostics.Debug.WriteLine($"[STEP 3] Search Result - Data is null: {searchResult.Data == null}");

      //         if (searchResult.IsSuccess && searchResult.Data != null)
      //         {
      //            existingContact = searchResult.Data;
      //            System.Diagnostics.Debug.WriteLine($"[STEP 3] ✅ FOUND by email! Contact ID: {existingContact.Id}");
      //         }
      //         else
      //         {
      //            System.Diagnostics.Debug.WriteLine("[STEP 3] ❌ Not found by email");
      //         }
      //      }

      //      if (existingContact != null)
      //      {
      //         chatwootContactId = existingContact.Id;
      //         System.Diagnostics.Debug.WriteLine($"[STEP 4] ✅ Using existing Chatwoot contact: {chatwootContactId}");
      //      }
      //      else
      //      {
      //         System.Diagnostics.Debug.WriteLine("[STEP 4] ❌ No existing contact found, CREATING NEW...");

      //         // ایجاد Contact جدید در Chatwoot
      //         var createRequest = new ChatwootContactCreateRequest
      //         {
      //            Name = _contact.Name ?? "Unknown",
      //            Email = _contact.Email,
      //            PhoneNumber = ConvertToE164(_contact.PhoneNumber)
      //            // ❌ حذف InboxId - باعث خطای 500 می‌شود!
      //         };

      //         System.Diagnostics.Debug.WriteLine($"[STEP 4] Create Request - Name: {createRequest.Name}");
      //         System.Diagnostics.Debug.WriteLine($"[STEP 4] Create Request - Phone: {createRequest.PhoneNumber}");
      //         System.Diagnostics.Debug.WriteLine($"[STEP 4] Create Request - Email: {createRequest.Email}");

      //         var createResult = await _chatwootService.CreateContactAsync(createRequest);

      //         System.Diagnostics.Debug.WriteLine($"[STEP 4] Create Result - IsSuccess: {createResult.IsSuccess}");
      //         System.Diagnostics.Debug.WriteLine($"[STEP 4] Create Result - Error: {createResult.ErrorMessage ?? "none"}");
      //         System.Diagnostics.Debug.WriteLine($"[STEP 4] Create Result - Data is null: {createResult.Data == null}");

      //         if (!createResult.IsSuccess || createResult.Data == null)
      //         {
      //            MessageBox.Show(
      //                $"خطا در ایجاد مخاطب در Chatwoot:\n{createResult.ErrorMessage ?? "خطای نامشخص"}",
      //                "خطا",
      //                MessageBoxButtons.OK,
      //                MessageBoxIcon.Error,
      //                MessageBoxDefaultButton.Button1,
      //                MessageBoxOptions.RtlReading);
      //            return false;
      //         }

      //         chatwootContactId = createResult.Data.Id;
      //         System.Diagnostics.Debug.WriteLine($"[STEP 4] ✅ Created new contact: {chatwootContactId}");
      //      }
      //   }

      //   // ایجاد Conversation با پیام اولیه
      //   System.Diagnostics.Debug.WriteLine($"[STEP 5] Creating conversation - ContactId: {chatwootContactId}, InboxId: {selectedInbox.Id}");

      //   var conversationRequest = new ChatwootConversationCreateRequest
      //   {
      //      InboxId = selectedInbox.Id,
      //      ContactId = chatwootContactId,
      //      Status = "open",
      //      Message = new ChatwootInitialMessage
      //      {
      //         Content = message
      //      }
      //   };

      //   var convResult = await _chatwootService.CreateConversationAsync(conversationRequest);

      //   System.Diagnostics.Debug.WriteLine($"[STEP 5] Conversation Result - IsSuccess: {convResult.IsSuccess}");
      //   System.Diagnostics.Debug.WriteLine($"[STEP 5] Conversation Result - Error: {convResult.ErrorMessage ?? "none"}");

      //   if (!convResult.IsSuccess || convResult.Data == null)
      //   {
      //      MessageBox.Show(
      //          $"خطا در ایجاد مکالمه:\n{convResult.ErrorMessage ?? "خطای نامشخص"}",
      //          "خطا",
      //          MessageBoxButtons.OK,
      //          MessageBoxIcon.Error,
      //          MessageBoxDefaultButton.Button1,
      //          MessageBoxOptions.RtlReading);
      //      return false;
      //   }

      //   System.Diagnostics.Debug.WriteLine("[STEP 5] ✅ SUCCESS! Conversation created.");
      //   return true;
      //}


      private async Task<bool> SendViaChatwootAsync(string message)
      {
         System.Diagnostics.Debug.WriteLine("========== SendViaChatwoot START ==========");

         // بررسی انتخاب Inbox
         if (cmbInbox.SelectedItem is not ChatwootInbox selectedInbox)
         {
            MessageBox.Show(
                "لطفاً یک کانال انتخاب کنید.",
                "هشدار",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button1,
                MessageBoxOptions.RtlReading);
            return false;
         }

         System.Diagnostics.Debug.WriteLine($"[INFO] Selected Inbox: {selectedInbox.Name} (ID: {selectedInbox.Id})");
         System.Diagnostics.Debug.WriteLine($"[INFO] Inbox ChannelType: {selectedInbox.ChannelType}");

         // بررسی یا ایجاد Contact در Chatwoot
         int chatwootContactId;

         System.Diagnostics.Debug.WriteLine($"[INFO] _contact.ChatwootContactId: {_contact.ChatwootContactId}");
         System.Diagnostics.Debug.WriteLine($"[INFO] _contact.PhoneNumber: [{_contact.PhoneNumber}]");
         System.Diagnostics.Debug.WriteLine($"[INFO] _contact.Email: [{_contact.Email}]");
         System.Diagnostics.Debug.WriteLine($"[INFO] _contact.Name: [{_contact.Name}]");

         if (_contact.ChatwootContactId.HasValue && _contact.ChatwootContactId.Value > 0)
         {
            chatwootContactId = _contact.ChatwootContactId.Value;
            System.Diagnostics.Debug.WriteLine($"[STEP 1] ✅ Using existing ChatwootContactId from local DB: {chatwootContactId}");
         }
         else
         {
            System.Diagnostics.Debug.WriteLine("[STEP 1] No local ChatwootContactId, searching in Chatwoot...");

            // جستجوی Contact موجود در Chatwoot
            ChatwootContact? existingContact = null;

            // جستجو با شماره تلفن
            if (!string.IsNullOrWhiteSpace(_contact.PhoneNumber))
            {
               var e164Phone = ConvertToE164(_contact.PhoneNumber);
               System.Diagnostics.Debug.WriteLine($"[STEP 2] Searching by phone: [{_contact.PhoneNumber}] -> E164: [{e164Phone}]");

               var searchResult = await _chatwootService.SearchContactByPhoneAsync(e164Phone);

               System.Diagnostics.Debug.WriteLine($"[STEP 2] Search Result - IsSuccess: {searchResult.IsSuccess}");
               System.Diagnostics.Debug.WriteLine($"[STEP 2] Search Result - Data is null: {searchResult.Data == null}");

               if (searchResult.IsSuccess && searchResult.Data != null)
               {
                  existingContact = searchResult.Data;
                  System.Diagnostics.Debug.WriteLine($"[STEP 2] ✅ FOUND by phone! Contact ID: {existingContact.Id}");
               }
            }

            // اگر پیدا نشد، جستجو با ایمیل
            if (existingContact == null && !string.IsNullOrWhiteSpace(_contact.Email))
            {
               System.Diagnostics.Debug.WriteLine($"[STEP 3] Searching by email: [{_contact.Email}]");

               var searchResult = await _chatwootService.SearchContactByEmailAsync(_contact.Email);

               if (searchResult.IsSuccess && searchResult.Data != null)
               {
                  existingContact = searchResult.Data;
                  System.Diagnostics.Debug.WriteLine($"[STEP 3] ✅ FOUND by email! Contact ID: {existingContact.Id}");
               }
            }

            if (existingContact != null)
            {
               chatwootContactId = existingContact.Id;
               System.Diagnostics.Debug.WriteLine($"[STEP 4] ✅ Using existing Chatwoot contact: {chatwootContactId}");
            }
            else
            {
               System.Diagnostics.Debug.WriteLine("[STEP 4] ❌ No existing contact found, CREATING NEW...");

               var createRequest = new ChatwootContactCreateRequest
               {
                  Name = _contact.Name ?? "Unknown",
                  Email = _contact.Email,
                  PhoneNumber = ConvertToE164(_contact.PhoneNumber)
               };

               var createResult = await _chatwootService.CreateContactAsync(createRequest);

               if (!createResult.IsSuccess || createResult.Data == null)
               {
                  MessageBox.Show(
                      $"خطا در ایجاد مخاطب در Chatwoot:\n{createResult.ErrorMessage ?? "خطای نامشخص"}",
                      "خطا",
                      MessageBoxButtons.OK,
                      MessageBoxIcon.Error,
                      MessageBoxDefaultButton.Button1,
                      MessageBoxOptions.RtlReading);
                  return false;
               }

               chatwootContactId = createResult.Data.Id;
               System.Diagnostics.Debug.WriteLine($"[STEP 4] ✅ Created new contact: {chatwootContactId}");
            }
         }

         // ═══════════════════════════════════════════════════════════════
         // 🆕 مرحله 4.5: اتصال Contact به Inbox (فقط برای Channel::Api)
         // ═══════════════════════════════════════════════════════════════
         string? sourceId = null;

         // بررسی نوع کانال
         bool isApiChannel = selectedInbox.ChannelType?.Equals("Channel::Api", StringComparison.OrdinalIgnoreCase) == true;

         System.Diagnostics.Debug.WriteLine($"[STEP 4.5] Channel Type: {selectedInbox.ChannelType}, IsApiChannel: {isApiChannel}");

         if (isApiChannel)
         {
            System.Diagnostics.Debug.WriteLine($"[STEP 4.5] 🔗 Connecting Contact {chatwootContactId} to Inbox {selectedInbox.Id}...");

            var contactInboxResult = await _chatwootService.CreateContactInboxAsync(chatwootContactId, selectedInbox.Id);

            if (contactInboxResult.IsSuccess && contactInboxResult.Data != null)
            {
               sourceId = contactInboxResult.Data.SourceId;
               System.Diagnostics.Debug.WriteLine($"[STEP 4.5] ✅ Contact connected. SourceId: {sourceId}");
            }
            else
            {
               System.Diagnostics.Debug.WriteLine($"[STEP 4.5] ⚠️ Could not create ContactInbox: {contactInboxResult.ErrorMessage}");
               // برای API Channel این خطا مهم است
               MessageBox.Show(
                   $"خطا در اتصال مخاطب به کانال:\n{contactInboxResult.ErrorMessage ?? "خطای نامشخص"}",
                   "خطا",
                   MessageBoxButtons.OK,
                   MessageBoxIcon.Error,
                   MessageBoxDefaultButton.Button1,
                   MessageBoxOptions.RtlReading);
               return false;
            }
         }
         else
         {
            System.Diagnostics.Debug.WriteLine($"[STEP 4.5] ⏭️ Skipping ContactInbox for channel type: {selectedInbox.ChannelType}");
            System.Diagnostics.Debug.WriteLine($"[STEP 4.5] ℹ️ For Telegram/WhatsApp, user must initiate contact first");
         }

         // ═══════════════════════════════════════════════════════════════
         // ایجاد Conversation با پیام اولیه
         // ═══════════════════════════════════════════════════════════════
         System.Diagnostics.Debug.WriteLine($"[STEP 5] Creating conversation...");
         System.Diagnostics.Debug.WriteLine($"[STEP 5] ContactId: {chatwootContactId}, InboxId: {selectedInbox.Id}, SourceId: {sourceId ?? "null"}");

         var conversationRequest = new ChatwootConversationCreateRequest
         {
            InboxId = selectedInbox.Id,
            ContactId = chatwootContactId,
            SourceId = sourceId,  // می‌تواند null باشد برای Telegram
            Status = "open",
            Message = new ChatwootInitialMessage
            {
               Content = message
            }
         };

         var convResult = await _chatwootService.CreateConversationAsync(conversationRequest);

         System.Diagnostics.Debug.WriteLine($"[STEP 5] Conversation Result - IsSuccess: {convResult.IsSuccess}");
         System.Diagnostics.Debug.WriteLine($"[STEP 5] Conversation Result - Error: {convResult.ErrorMessage ?? "none"}");

         if (!convResult.IsSuccess || convResult.Data == null)
         {
            MessageBox.Show(
                $"خطا در ایجاد مکالمه:\n{convResult.ErrorMessage ?? "خطای نامشخص"}",
                "خطا",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error,
                MessageBoxDefaultButton.Button1,
                MessageBoxOptions.RtlReading);
            return false;
         }

         System.Diagnostics.Debug.WriteLine($"[STEP 5] ✅ SUCCESS! Conversation created. ID: {convResult.Data.Id}");
         System.Diagnostics.Debug.WriteLine("========== SendViaChatwoot END ==========");
         return true;
      }


      /// <summary>
      /// تبدیل شماره تلفن ایرانی به فرمت E.164
      /// </summary>
      private string ConvertToE164(string phoneNumber)
      {
         if (string.IsNullOrWhiteSpace(phoneNumber))
            return phoneNumber;

         // حذف فاصله، خط تیره و کاراکترهای اضافی
         var cleaned = phoneNumber
             .Replace(" ", "")
             .Replace("-", "")
             .Replace("(", "")
             .Replace(")", "")
             .Trim();

         // اگر از قبل با + شروع شده، برگردان
         if (cleaned.StartsWith("+"))
            return cleaned;

         // تبدیل شماره ایرانی
         if (cleaned.StartsWith("09") && cleaned.Length == 11)
         {
            // 09193670720 → +989193670720
            return "+98" + cleaned.Substring(1);
         }

         if (cleaned.StartsWith("9") && cleaned.Length == 10)
         {
            // 9193670720 → +989193670720
            return "+98" + cleaned;
         }

         if (cleaned.StartsWith("0098"))
         {
            // 00989193670720 → +989193670720
            return "+" + cleaned.Substring(2);
         }

         if (cleaned.StartsWith("98") && cleaned.Length == 12)
         {
            // 989193670720 → +989193670720
            return "+" + cleaned;
         }

         // اگر هیچکدام نبود، + اضافه کن
         return "+" + cleaned;
      }

      #endregion

      #region Telegram Direct Send

      private async Task<bool> SendViaTelegramDirectAsync(string message)
      {
         // بررسی سرویس Telegram
         if (_telegramClientService == null)
         {
            MessageBox.Show(
                "سرویس Telegram در دسترس نیست.",
                "خطا",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error,
                MessageBoxDefaultButton.Button1,
                MessageBoxOptions.RtlReading);
            return false;
         }

         // بررسی شماره تلفن مخاطب
         if (string.IsNullOrWhiteSpace(_contact.PhoneNumber))
         {
            MessageBox.Show(
                "شماره تلفن مخاطب مشخص نیست.\nبرای ارسال مستقیم از Telegram، شماره تلفن الزامی است.",
                "خطا",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button1,
                MessageBoxOptions.RtlReading);
            return false;
         }

         // بررسی اتصال
         if (!_telegramClientService.IsConnected)
         {
            var loginResult = MessageBox.Show(
                "اتصال به Telegram برقرار نیست.\nآیا می‌خواهید وارد شوید؟",
                "نیاز به ورود",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button1,
                MessageBoxOptions.RtlReading);

            if (loginResult == DialogResult.Yes)
            {
               // TODO: نمایش فرم ورود Telegram
               // var loginForm = new TelegramLoginForm(_telegramClientService);
               // if (loginForm.ShowDialog() != DialogResult.OK)
               // {
               //     return false;
               // }

               MessageBox.Show(
                   "فرم ورود Telegram هنوز پیاده‌سازی نشده است.\nلطفاً از روش Chatwoot استفاده کنید.",
                   "در دست توسعه",
                   MessageBoxButtons.OK,
                   MessageBoxIcon.Information,
                   MessageBoxDefaultButton.Button1,
                   MessageBoxOptions.RtlReading);
               return false;
            }
            else
            {
               return false;
            }
         }

         // ارسال پیام از طریق Telegram
         var result = await _telegramClientService.SendMessageAsync(_contact.PhoneNumber, message);

         // استفاده از Success (نه IsSuccess) طبق ساختار TelegramSendResult
         if (!result.Success)
         {
            MessageBox.Show(
                $"خطا در ارسال پیام از طریق Telegram:\n{result.ErrorMessage ?? "خطای نامشخص"}",
                "خطا",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error,
                MessageBoxDefaultButton.Button1,
                MessageBoxOptions.RtlReading);
            return false;
         }

         return true;
      }

      #endregion

      #region Helper Methods

      private void SetLoading(bool isLoading, string statusText)
      {
         // بروزرسانی UI در Thread اصلی
         if (this.InvokeRequired)
         {
            this.Invoke(new Action(() => SetLoading(isLoading, statusText)));
            return;
         }

         btnSend.Enabled = !isLoading;
         btnCancel.Enabled = !isLoading;
         txtMessage.Enabled = !isLoading;
         cmbInbox.Enabled = !isLoading;
         rdoChatwoot.Enabled = !isLoading;
         rdoTelegramDirect.Enabled = !isLoading && _telegramClientService != null;

         lblStatus.Text = statusText;

         if (isLoading)
         {
            lblStatus.ForeColor = Color.DarkOrange;
            this.Cursor = Cursors.WaitCursor;
         }
         else
         {
            lblStatus.ForeColor = Color.FromArgb(52, 73, 94);
            this.Cursor = Cursors.Default;
         }

         Application.DoEvents();
      }

      #endregion
   }
}
