using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using WootchatCRM.Infrastructure.Chatwoot;
using WootchatCRM.Infrastructure.Chatwoot.DTOs;

namespace WootchatCRM.UI.Forms;

public partial class ConversationsForm : UserControl
{
   // ═══════════════════════════════════════════════════════════════
   // Dependencies & State
   // ═══════════════════════════════════════════════════════════════

   private readonly IChatwootApiClient _apiClient;
  
   private List<ChatwootInbox> _inboxes = new();
   private List<ChatwootConversation> _conversations = new();
   private ChatwootConversation? _selectedConversation;
   private List<ChatwootMessage> _currentMessages = new();

   private int _currentPage = 1;
   private int _totalPages = 1;
   private bool _isLoading = false;

   // ✅ اضافه شده: Timers برای Auto-Sync
   private System.Windows.Forms.Timer? _conversationsRefreshTimer;
   private System.Windows.Forms.Timer? _messagesRefreshTimer;
   private const int CONVERSATIONS_REFRESH_INTERVAL = 30000; // 30 ثانیه
   private const int MESSAGES_REFRESH_INTERVAL = 10000;      // 10 ثانیه

   // ═══════════════════════════════════════════════════════════════
   // Constructor
   // ═══════════════════════════════════════════════════════════════

   public ConversationsForm(IChatwootApiClient apiClient)
   {
      InitializeComponent();
      _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));

      SetupForm();
      SetupEventHandlers();
      SetupTimers();

   }
  
   // ═══════════════════════════════════════════════════════════════
   // Setup Methods
   // ═══════════════════════════════════════════════════════════════

   private void SetupForm()
   {
      this.Text = "مدیریت مکالمات";
     
      this.MinimumSize = new Size(1000, 700);

      // Setup Status Filter ComboBox
      cmbStatusFilter.Items.Clear();
      cmbStatusFilter.Items.Add(new ComboBoxItem("همه", "all"));
      cmbStatusFilter.Items.Add(new ComboBoxItem("باز", "open"));
      cmbStatusFilter.Items.Add(new ComboBoxItem("در انتظار", "pending"));
      cmbStatusFilter.Items.Add(new ComboBoxItem("حل شده", "resolved"));
      cmbStatusFilter.Items.Add(new ComboBoxItem("به تعویق افتاده", "snoozed"));
      cmbStatusFilter.SelectedIndex = 1; // Default: open

      // Setup ListView columns
      listViewConversations.Columns.Clear();
      listViewConversations.Columns.Add("شناسه", 60, HorizontalAlignment.Center);
      listViewConversations.Columns.Add("مخاطب", 150, HorizontalAlignment.Right);
      listViewConversations.Columns.Add("وضعیت", 80, HorizontalAlignment.Center);
      listViewConversations.Columns.Add("آخرین پیام", 250, HorizontalAlignment.Right);
      listViewConversations.Columns.Add("تاریخ", 120, HorizontalAlignment.Center);
      listViewConversations.View = View.Details;
      listViewConversations.FullRowSelect = true;
      listViewConversations.RightToLeftLayout = true;

      // Disable message input until a conversation is selected
      SetMessageInputEnabled(false);
   }

   private void SetupEventHandlers()
   {
      this.Load += ConversationsForm_Load;

      // Filters
      cmbStatusFilter.SelectedIndexChanged += CmbStatusFilter_SelectedIndexChanged;
      cmbInboxFilter.SelectedIndexChanged += CmbInboxFilter_SelectedIndexChanged;

      // Search
      txtSearch.KeyDown += TxtSearch_KeyDown;
      btnSearch.Click += BtnSearch_Click;

      // Pagination
      btnPreviousPage.Click += BtnPreviousPage_Click;
      btnNextPage.Click += BtnNextPage_Click;

      // Conversation List
      listViewConversations.SelectedIndexChanged += ListViewConversations_SelectedIndexChanged;

      // Message Actions
      btnSendMessage.Click += BtnSendMessage_Click;
      txtMessageInput.KeyDown += TxtMessageInput_KeyDown;

      // Status Actions
      btnChangeStatus.Click += BtnChangeStatus_Click;
      btnRefresh.Click += BtnRefresh_Click;
   }

   // ═══════════════════════════════════════════════════════════════
   // Form Events
   // ═══════════════════════════════════════════════════════════════

   private async void ConversationsForm_Load(object? sender, EventArgs e)
   {
      await LoadInitialDataAsync();
      StartTimers();
   }


   // ═══════════════════════════════════════════════════════════════
   // Data Loading
   // ═══════════════════════════════════════════════════════════════

   private async Task LoadInitialDataAsync()
   {
      await LoadInboxesAsync();
      await LoadConversationsAsync();
   }

   private async Task LoadInboxesAsync()
   {
      try
      {
         SetLoading(true, "در حال بارگذاری صندوق‌ها...");

         var result = await _apiClient.GetInboxesAsync();

         if (result.IsSuccess && result.Data != null)
         {
            _inboxes = result.Data;

            cmbInboxFilter.Items.Clear();
            cmbInboxFilter.Items.Add(new ComboBoxItem("همه صندوق‌ها", null));

            foreach (var inbox in _inboxes)
            {
               cmbInboxFilter.Items.Add(new ComboBoxItem(inbox.Name, inbox.Id));
            }

            cmbInboxFilter.SelectedIndex = 0;
         }
         else
         {
            ShowError($"خطا در بارگذاری صندوق‌ها: {result.ErrorMessage}");
         }
      }
      catch (Exception ex)
      {
         ShowError($"خطا: {ex.Message}");
      }
      finally
      {
         SetLoading(false);
      }
   }

   private async Task LoadConversationsAsync()
   {
      if (_isLoading) return;

      try
      {
         SetLoading(true, "در حال بارگذاری مکالمات...");

         // Get filter values
         string status = "open";
         if (cmbStatusFilter.SelectedItem is ComboBoxItem statusItem && statusItem.Value != null)
         {
            status = statusItem.Value.ToString()!;
            if (status == "all") status = "open"; // API default
         }

         int? inboxId = null;
         if (cmbInboxFilter.SelectedItem is ComboBoxItem inboxItem && inboxItem.Value is int id)
         {
            inboxId = id;
         }

         var result = await _apiClient.GetConversationsAsync(
             status: status,
             page: _currentPage,
             inboxId: inboxId,
             assigneeId: null
         );

         if (result.IsSuccess && result.Data != null)
         {
            _conversations = result.Data.Data?.Payload ?? new List<ChatwootConversation>();

            // Calculate total pages
            var meta = result.Data.Meta;
            if (meta?.Count != null && meta.Count > 0)
            {
               int totalItems = meta.AllCount ?? meta.Count.Value;
               _totalPages = Math.Max(1, (int)Math.Ceiling(totalItems / 25.0));
            }
            else
            {
               _totalPages = 1;
            }

            PopulateConversationList();
            UpdatePaginationUI();
            UpdateStatusBar($"تعداد: {_conversations.Count} مکالمه");
         }
         else
         {
            ShowError($"خطا در بارگذاری: {result.ErrorMessage}");
            _conversations.Clear();
            PopulateConversationList();
         }
      }
      catch (Exception ex)
      {
         ShowError($"خطا: {ex.Message}");
      }
      finally
      {
         SetLoading(false);
      }
   }

   private async Task LoadMessagesAsync(int conversationId)
   {
      try
      {
         SetLoading(true, "در حال بارگذاری پیام‌ها...");

         var result = await _apiClient.GetMessagesAsync(conversationId);

         if (result.IsSuccess && result.Data != null)
         {
            _currentMessages = result.Data;
            DisplayMessages();
         }
         else
         {
            ShowError($"خطا در بارگذاری پیام‌ها: {result.ErrorMessage}");
            _currentMessages.Clear();
            DisplayMessages();
         }
      }
      catch (Exception ex)
      {
         ShowError($"خطا: {ex.Message}");
      }
      finally
      {
         SetLoading(false);
      }
   }

   // ═══════════════════════════════════════════════════════════════
   // UI Population
   // ═══════════════════════════════════════════════════════════════

   private void PopulateConversationList()
   {
      listViewConversations.Items.Clear();

      foreach (var conv in _conversations)
      {
         string contactName = conv.Meta?.Sender?.Name ?? "نامشخص";
         string status = GetPersianStatus(conv.Status);
         string lastMessage = conv.LastMessage?.Content ?? "(بدون پیام)";
         if (lastMessage.Length > 50)
            lastMessage = lastMessage.Substring(0, 47) + "...";

         string dateStr = FormatUnixTimestamp(conv.CreatedAt);

         var item = new ListViewItem(new[]
         {
                conv.Id.ToString(),
                contactName,
                status,
                lastMessage,
                dateStr
            });

         item.Tag = conv;

         // Color coding based on status
         item.BackColor = conv.Status.ToLower() switch
         {
            "open" => Color.FromArgb(255, 250, 230),      // Light yellow
            "pending" => Color.FromArgb(255, 240, 220),   // Light orange
            "resolved" => Color.FromArgb(230, 255, 230),  // Light green
            "snoozed" => Color.FromArgb(240, 240, 240),   // Light gray
            _ => Color.White
         };

         // Highlight unread
         if (conv.UnreadCount > 0)
         {
            item.Font = new Font(listViewConversations.Font, FontStyle.Bold);
         }

         listViewConversations.Items.Add(item);
      }
   }

   private void DisplayMessages()
   {
      rtbMessages.Clear();

      if (_currentMessages.Count == 0)
      {
         rtbMessages.Text = "هیچ پیامی وجود ندارد.";
         return;
      }

      // Sort by creation time
      var sortedMessages = _currentMessages.OrderBy(m => m.CreatedAt).ToList();

      foreach (var msg in sortedMessages)
      {
         AppendMessage(msg);
      }

      // Scroll to end
      rtbMessages.SelectionStart = rtbMessages.TextLength;
      rtbMessages.ScrollToCaret();
   }

   private void AppendMessage(ChatwootMessage msg)
   {
      // Skip activity messages (type 2)
      if (msg.MessageType == 2) return;

      string senderName;
      Color senderColor;
      bool isOutgoing = msg.MessageType == 1;

      if (msg.Private)
      {
         senderName = $"🔒 {msg.Sender?.Name ?? "یادداشت داخلی"}";
         senderColor = Color.Purple;
      }
      else if (isOutgoing)
      {
         senderName = $"✉️ {msg.Sender?.Name ?? "شما"}";
         senderColor = Color.DarkBlue;
      }
      else
      {
         senderName = $"📩 {msg.Sender?.Name ?? "مخاطب"}";
         senderColor = Color.DarkGreen;
      }

      string timeStr = FormatUnixTimestamp(msg.CreatedAt);

      // Sender header
      rtbMessages.SelectionFont = new Font(rtbMessages.Font.FontFamily, 9, FontStyle.Bold);
      rtbMessages.SelectionColor = senderColor;
      rtbMessages.AppendText($"{senderName} - {timeStr}\n");

      // Message content
      rtbMessages.SelectionFont = new Font(rtbMessages.Font.FontFamily, 10, FontStyle.Regular);
      rtbMessages.SelectionColor = Color.Black;

      if (msg.Private)
      {
         rtbMessages.SelectionBackColor = Color.FromArgb(245, 230, 255);
      }
      else if (isOutgoing)
      {
         rtbMessages.SelectionBackColor = Color.FromArgb(230, 240, 255);
      }
      else
      {
         rtbMessages.SelectionBackColor = Color.FromArgb(240, 255, 240);
      }

      rtbMessages.AppendText($"{msg.Content ?? "(بدون محتوا)"}\n");

      // Attachments
      if (msg.Attachments != null && msg.Attachments.Count > 0)
      {
         rtbMessages.SelectionFont = new Font(rtbMessages.Font.FontFamily, 8, FontStyle.Italic);
         rtbMessages.SelectionColor = Color.Gray;
         rtbMessages.AppendText($"📎 {msg.Attachments.Count} فایل پیوست\n");
      }

      rtbMessages.SelectionBackColor = Color.White;
      rtbMessages.AppendText("\n");
   }

   private void UpdatePaginationUI()
   {
      lblPageInfo.Text = $"صفحه {_currentPage} از {_totalPages}";
      btnPreviousPage.Enabled = _currentPage > 1;
      btnNextPage.Enabled = _currentPage < _totalPages;
   }

   // ═══════════════════════════════════════════════════════════════
   // Event Handlers - Filters
   // ═══════════════════════════════════════════════════════════════

   private async void CmbStatusFilter_SelectedIndexChanged(object? sender, EventArgs e)
   {
      _currentPage = 1;
      await LoadConversationsAsync();
   }

   private async void CmbInboxFilter_SelectedIndexChanged(object? sender, EventArgs e)
   {
      _currentPage = 1;
      await LoadConversationsAsync();
   }

   // ═══════════════════════════════════════════════════════════════
   // Event Handlers - Search
   // ═══════════════════════════════════════════════════════════════

   private void TxtSearch_KeyDown(object? sender, KeyEventArgs e)
   {
      if (e.KeyCode == Keys.Enter)
      {
         e.SuppressKeyPress = true;
         PerformSearch();
      }
   }

   private void BtnSearch_Click(object? sender, EventArgs e)
   {
      PerformSearch();
   }

   private void PerformSearch()
   {
      string searchText = txtSearch.Text.Trim().ToLower();

      if (string.IsNullOrEmpty(searchText))
      {
         PopulateConversationList();
         return;
      }

      var filtered = _conversations.Where(c =>
          (c.Meta?.Sender?.Name?.ToLower().Contains(searchText) ?? false) ||
          (c.LastMessage?.Content?.ToLower().Contains(searchText) ?? false) ||
          c.Id.ToString().Contains(searchText)
      ).ToList();

      listViewConversations.Items.Clear();

      foreach (var conv in filtered)
      {
         string contactName = conv.Meta?.Sender?.Name ?? "نامشخص";
         string status = GetPersianStatus(conv.Status);
         string lastMessage = conv.LastMessage?.Content ?? "(بدون پیام)";
         if (lastMessage.Length > 50)
            lastMessage = lastMessage.Substring(0, 47) + "...";

         string dateStr = FormatUnixTimestamp(conv.CreatedAt);

         var item = new ListViewItem(new[]
         {
                conv.Id.ToString(),
                contactName,
                status,
                lastMessage,
                dateStr
            });

         item.Tag = conv;
         listViewConversations.Items.Add(item);
      }
   }

   // ═══════════════════════════════════════════════════════════════
   // Event Handlers - Pagination
   // ═══════════════════════════════════════════════════════════════

   private async void BtnPreviousPage_Click(object? sender, EventArgs e)
   {
      if (_currentPage <= 1) return;
      _currentPage--;
      await LoadConversationsAsync();
   }

   private async void BtnNextPage_Click(object? sender, EventArgs e)
   {
      if (_currentPage >= _totalPages) return;
      _currentPage++;
      await LoadConversationsAsync();
   }

   // ═══════════════════════════════════════════════════════════════
   // Event Handlers - Conversation Selection
   // ═══════════════════════════════════════════════════════════════

   private async void ListViewConversations_SelectedIndexChanged(object? sender, EventArgs e)
   {
      if (listViewConversations.SelectedItems.Count == 0)
      {
         _selectedConversation = null;
         SetMessageInputEnabled(false);
         rtbMessages.Clear();
         return;
      }

      var item = listViewConversations.SelectedItems[0];
      _selectedConversation = item.Tag as ChatwootConversation;

      if (_selectedConversation == null) return;

      SetMessageInputEnabled(_selectedConversation.CanReply);
      await LoadMessagesAsync(_selectedConversation.Id);
   }

   // ═══════════════════════════════════════════════════════════════
   // Event Handlers - Message Actions
   // ═══════════════════════════════════════════════════════════════

   private async void BtnSendMessage_Click(object? sender, EventArgs e)
   {
      if (_selectedConversation == null) return;

      string content = txtMessageInput.Text.Trim();
      if (string.IsNullOrEmpty(content)) return;

      try
      {
         SetLoading(true, "در حال ارسال پیام...");

         var result = await _apiClient.SendTextMessageAsync(
             _selectedConversation.Id,
             content,
             isPrivate: chkPrivateNote.Checked
         );

         if (result.IsSuccess && result.Data != null)
         {
            txtMessageInput.Clear();
            chkPrivateNote.Checked = false;
            await LoadMessagesAsync(_selectedConversation.Id);
            await LoadConversationsAsync();
         }
         else
         {
            ShowError($"خطا در ارسال پیام: {result.ErrorMessage}");
         }
      }
      catch (Exception ex)
      {
         ShowError($"خطا: {ex.Message}");
      }
      finally
      {
         SetLoading(false);
      }
   }

   private void TxtMessageInput_KeyDown(object? sender, KeyEventArgs e)
   {
      if (e.KeyCode == Keys.Enter && e.Control)
      {
         e.SuppressKeyPress = true;
         BtnSendMessage_Click(sender, EventArgs.Empty);
      }
   }

   // ═══════════════════════════════════════════════════════════════
   // Event Handlers - Status Actions
   // ═══════════════════════════════════════════════════════════════

   private async void BtnChangeStatus_Click(object? sender, EventArgs e)
   {
      if (_selectedConversation == null) return;

      var statusItem = cmbConversationStatus.SelectedItem as ComboBoxItem;
      if (statusItem?.Value == null) return;

      string newStatus = statusItem.Value.ToString()!;

      try
      {
         SetLoading(true, "در حال تغییر وضعیت...");

         var result = await _apiClient.UpdateConversationStatusAsync(
             _selectedConversation.Id,
             newStatus
         );

         if (result.IsSuccess)
         {
            await LoadConversationsAsync();
         }
         else
         {
            ShowError($"خطا در تغییر وضعیت: {result.ErrorMessage}");
         }
      }
      catch (Exception ex)
      {
         ShowError($"خطا: {ex.Message}");
      }
      finally
      {
         SetLoading(false);
      }
   }

   private async void BtnRefresh_Click(object? sender, EventArgs e)
   {
      await LoadConversationsAsync();
   }


   // ═══════════════════════════════════════════════════════════════
   // Auto-Sync (Timers)
   // ═══════════════════════════════════════════════════════════════

   private void SetupTimers()
   {
      // Timer برای refresh لیست مکالمات
      _conversationsRefreshTimer = new System.Windows.Forms.Timer();
      _conversationsRefreshTimer.Interval = CONVERSATIONS_REFRESH_INTERVAL;
      _conversationsRefreshTimer.Tick += ConversationsRefreshTimer_Tick;

      // Timer برای refresh پیام‌های مکالمه انتخاب‌شده
      _messagesRefreshTimer = new System.Windows.Forms.Timer();
      _messagesRefreshTimer.Interval = MESSAGES_REFRESH_INTERVAL;
      _messagesRefreshTimer.Tick += MessagesRefreshTimer_Tick;
   }

   private void StartTimers()
   {
      _conversationsRefreshTimer?.Start();
      _messagesRefreshTimer?.Start();
   }

   // ═══════════════════════════════════════════════════════════════
   // Cleanup
   // ═══════════════════════════════════════════════════════════════

   private void StopTimers()
   {
      _conversationsRefreshTimer?.Stop();
      _conversationsRefreshTimer?.Dispose();
      _conversationsRefreshTimer = null;

      _messagesRefreshTimer?.Stop();
      _messagesRefreshTimer?.Dispose();
      _messagesRefreshTimer = null;
   }


   private async void ConversationsRefreshTimer_Tick(object? sender, EventArgs e)
   {
      await RefreshConversationsAsync();
   }

   private async void MessagesRefreshTimer_Tick(object? sender, EventArgs e)
   {
      await RefreshMessagesAsync();
   }

   /// <summary>
   /// بروزرسانی خودکار لیست مکالمات (بدون نمایش loading)
   /// </summary>
   private async Task RefreshConversationsAsync()
   {
      // اگر در حال لود دستی هستیم، skip کن
      if (_isLoading) return;

      try
      {
         // Get filter values
         string status = "open";
         if (cmbStatusFilter.SelectedItem is ComboBoxItem statusItem && statusItem.Value != null)
         {
            status = statusItem.Value.ToString()!;
            if (status == "all") status = "open";
         }

         int? inboxId = null;
         if (cmbInboxFilter.SelectedItem is ComboBoxItem inboxItem && inboxItem.Value is int id)
         {
            inboxId = id;
         }

         var result = await _apiClient.GetConversationsAsync(
             status: status,
             page: _currentPage,
             inboxId: inboxId,
             assigneeId: null
         );

         if (result.IsSuccess && result.Data != null)
         {
            // ذخیره ID مکالمه انتخاب‌شده فعلی
            int? selectedConvId = _selectedConversation?.Id;

            _conversations = result.Data.Data?.Payload ?? new List<ChatwootConversation>();

            // محاسبه تعداد صفحات
            var meta = result.Data.Meta;
            if (meta?.Count != null && meta.Count > 0)
            {
               int totalItems = meta.AllCount ?? meta.Count.Value;
               _totalPages = Math.Max(1, (int)Math.Ceiling(totalItems / 25.0));
            }
            else
            {
               _totalPages = 1;
            }

            PopulateConversationList();
            UpdatePaginationUI();

            // بازیابی انتخاب قبلی
            if (selectedConvId.HasValue)
            {
               RestoreConversationSelection(selectedConvId.Value);
            }

            UpdateStatusBar($"آخرین بروزرسانی: {DateTime.Now:HH:mm:ss} | تعداد: {_conversations.Count}");
         }
      }
      catch (Exception ex)
      {
         // در refresh خودکار خطا را نمایش نمی‌دهیم (فقط log)
         System.Diagnostics.Debug.WriteLine($"Auto-refresh conversations error: {ex.Message}");
      }
   }

   /// <summary>
   /// بروزرسانی خودکار پیام‌های مکالمه انتخاب‌شده
   /// </summary>
   private async Task RefreshMessagesAsync()
   {
      // فقط اگر مکالمه‌ای انتخاب شده باشد
      if (_selectedConversation == null) return;
      if (_isLoading) return;

      try
      {
         var result = await _apiClient.GetMessagesAsync(_selectedConversation.Id);

         if (result.IsSuccess && result.Data != null)
         {
            // فقط اگر تعداد پیام‌ها تغییر کرده باشد، بروزرسانی کن
            if (result.Data.Count != _currentMessages.Count)
            {
               _currentMessages = result.Data;
               DisplayMessages();
            }
         }
      }
      catch (Exception ex)
      {
         System.Diagnostics.Debug.WriteLine($"Auto-refresh messages error: {ex.Message}");
      }
   }

   /// <summary>
   /// بازیابی انتخاب مکالمه پس از refresh
   /// </summary>
   private void RestoreConversationSelection(int conversationId)
   {
      foreach (ListViewItem item in listViewConversations.Items)
      {
         if (item.Tag is ChatwootConversation conv && conv.Id == conversationId)
         {
            item.Selected = true;
            item.Focused = true;
            item.EnsureVisible();
            _selectedConversation = conv;
            break;
         }
      }
   }

   // ═══════════════════════════════════════════════════════════════
   // Helper Methods
   // ═══════════════════════════════════════════════════════════════

   private void SetLoading(bool isLoading, string? statusText = null)
   {
      _isLoading = isLoading;

      // ✅ اصلاح شده برای ToolStripProgressBar
      progressBar.Visible = isLoading;
      if (isLoading)
      {
         progressBar.Style = ProgressBarStyle.Marquee;
         progressBar.MarqueeAnimationSpeed = 30;
      }

      // ✅ آپدیت StatusLabel
      if (!string.IsNullOrEmpty(statusText))
      {
         lblStatus.Text = statusText;
      }
      else if (!isLoading)
      {
         lblStatus.Text = "آماده";
      }

      // غیرفعال کردن کنترل‌ها
      cmbInboxFilter.Enabled = !isLoading;
      cmbStatusFilter.Enabled = !isLoading;
      btnRefresh.Enabled = !isLoading;
      btnSendMessage.Enabled = !isLoading && _selectedConversation != null;

      this.UseWaitCursor = isLoading;
   }

   private void UpdateStatusBar(string text)
   {
      if (InvokeRequired)
      {
         Invoke(new Action(() => UpdateStatusBar(text)));
         return;
      }

      lblStatus.Text = text;
   }

   private void SetMessageInputEnabled(bool enabled)
   {
      txtMessageInput.Enabled = enabled;
      btnSendMessage.Enabled = enabled;
      chkPrivateNote.Enabled = enabled;
   }

   private void ShowError(string message)
   {
      MessageBox.Show(message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
      UpdateStatusBar($"❌ {message}");
   }

   private void ShowSuccess(string message)
   {
      UpdateStatusBar($"✅ {message}");
   }

   private static string FormatUnixTimestamp(long unix)
   {
      var date = DateTimeOffset.FromUnixTimeSeconds(unix).LocalDateTime;
      return date.ToString("yyyy/MM/dd HH:mm");
   }

   private static string GetPersianStatus(string status) => status.ToLower() switch
   {
      "open" => "باز",
      "pending" => "در انتظار",
      "resolved" => "حل شده",
      "snoozed" => "تعویق",
      _ => status
   };


   // ═══════════════════════════════════════════════════════════════
   // ComboBox Item Helper
   // ═══════════════════════════════════════════════════════════════

   private class ComboBoxItem
   {
      public string Text { get; }
      public object? Value { get; }

      public ComboBoxItem(string text, object? value)
      {
         Text = text;
         Value = value;
      }

      public override string ToString() => Text;
   }



}
