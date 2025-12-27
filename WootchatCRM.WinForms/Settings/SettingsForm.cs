using WootchatCRM.Core.Interfaces.Services;
using WootchatCRM.Infrastructure.Chatwoot;
using WootchatCRM.Infrastructure.Chatwoot.DTOs;

namespace WootchatCRM.UI.Forms;

public class SettingsForm : Form
{
   // ═══════════════════════════════════════════════════════════════
   // Dependencies
   // ═══════════════════════════════════════════════════════════════

   private readonly ISettingsService _settingsService;
   private readonly IChatwootApiClient _chatwootClient;

   // ═══════════════════════════════════════════════════════════════
   // State
   // ═══════════════════════════════════════════════════════════════

   private bool _isDirty = false;
   private int _pendingDefaultInboxId = 0;

   // ═══════════════════════════════════════════════════════════════
   // UI Controls - Chatwoot Tab
   // ═══════════════════════════════════════════════════════════════

   private TabControl tabControl = null!;
   private TextBox txtBaseUrl = null!;
   private TextBox txtApiKey = null!;
   private NumericUpDown numAccountId = null!;
   private Button btnTestConnection = null!;
   private Label lblConnectionStatus = null!;
   private ComboBox cmbInbox = null!;
   private Button btnLoadInboxes = null!;

   // ═══════════════════════════════════════════════════════════════
   // UI Controls - General Tab
   // ═══════════════════════════════════════════════════════════════

   private CheckBox chkStartMinimized = null!;
   private CheckBox chkAutoSync = null!;
   private NumericUpDown numSyncInterval = null!;

   // ═══════════════════════════════════════════════════════════════
   // UI Controls - Footer
   // ═══════════════════════════════════════════════════════════════

   private Button btnSave = null!;
   private Button btnCancel = null!;

   // ═══════════════════════════════════════════════════════════════
   // Constructor
   // ═══════════════════════════════════════════════════════════════

   public SettingsForm(ISettingsService settingsService, IChatwootApiClient chatwootClient)
   {
      _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
      _chatwootClient = chatwootClient ?? throw new ArgumentNullException(nameof(chatwootClient));

      InitializeComponents();
      SetupEventHandlers();
   }

   // ═══════════════════════════════════════════════════════════════
   // UI Initialization
   // ═══════════════════════════════════════════════════════════════

   private void InitializeComponents()
   {
      // Form Settings
      this.Text = "تنظیمات";
      this.Size = new Size(550, 480);
      this.StartPosition = FormStartPosition.CenterParent;
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.RightToLeft = RightToLeft.Yes;
      this.RightToLeftLayout = true;
      this.Font = new Font("Segoe UI", 9.5f);

      // Tab Control
      tabControl = new TabControl
      {
         Dock = DockStyle.Top,
         Height = 360,
         RightToLeftLayout = true
      };

      // Create Tabs
      var tabChatwoot = CreateChatwootTab();
      var tabGeneral = CreateGeneralTab();

      tabControl.TabPages.Add(tabChatwoot);
      tabControl.TabPages.Add(tabGeneral);
      this.Controls.Add(tabControl);

      // Footer Panel
      var footerPanel = CreateFooterPanel();
      this.Controls.Add(footerPanel);

      this.AcceptButton = btnSave;
      this.CancelButton = btnCancel;
   }

   private TabPage CreateChatwootTab()
   {
      var tab = new TabPage("Chatwoot API") { Padding = new Padding(15) };

      var layout = new TableLayoutPanel
      {
         Dock = DockStyle.Fill,
         ColumnCount = 2,
         RowCount = 6,
         AutoSize = true
      };
      layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30));
      layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70));

      // Row 0: Base URL
      layout.Controls.Add(CreateLabel("آدرس سرور:"), 0, 0);
      txtBaseUrl = new TextBox
      {
         Dock = DockStyle.Fill,
         PlaceholderText = "https://chat.example.com"
      };
      layout.Controls.Add(txtBaseUrl, 1, 0);

      // Row 1: API Key
      layout.Controls.Add(CreateLabel("کلید API:"), 0, 1);
      txtApiKey = new TextBox
      {
         Dock = DockStyle.Fill,
         UseSystemPasswordChar = true,
         PlaceholderText = "API Access Token"
      };
      layout.Controls.Add(txtApiKey, 1, 1);

      // Row 2: Account ID
      layout.Controls.Add(CreateLabel("شناسه اکانت:"), 0, 2);
      numAccountId = new NumericUpDown
      {
         Minimum = 1,
         Maximum = 9999,
         Value = 1,
         Width = 100
      };
      layout.Controls.Add(numAccountId, 1, 2);

      // Row 3: Test Connection
      layout.Controls.Add(new Label(), 0, 3);

      var testPanel = new FlowLayoutPanel
      {
         Dock = DockStyle.Fill,
         FlowDirection = FlowDirection.LeftToRight,
         WrapContents = false,
         AutoSize = true
      };

      btnTestConnection = new Button
      {
         Text = "🔗 تست اتصال",
         Width = 120,
         Height = 32
      };

      lblConnectionStatus = new Label
      {
         Text = "",
         AutoSize = true,
         Padding = new Padding(10, 8, 0, 0)
      };

      testPanel.Controls.Add(btnTestConnection);
      testPanel.Controls.Add(lblConnectionStatus);
      layout.Controls.Add(testPanel, 1, 3);

      // Row 4: Separator
      var separator = new Label
      {
         Text = "────────────────────────────────────",
         Dock = DockStyle.Fill,
         ForeColor = Color.Gray,
         TextAlign = ContentAlignment.MiddleCenter
      };
      layout.Controls.Add(separator, 0, 4);
      layout.SetColumnSpan(separator, 2);

      // Row 5: Inbox Selection
      layout.Controls.Add(CreateLabel("صندوق پیش‌فرض:"), 0, 5);

      var inboxPanel = new FlowLayoutPanel
      {
         Dock = DockStyle.Fill,
         FlowDirection = FlowDirection.LeftToRight,
         WrapContents = false,
         AutoSize = true
      };

      cmbInbox = new ComboBox
      {
         Width = 250,
         DropDownStyle = ComboBoxStyle.DropDownList,
         DisplayMember = "Name",
         ValueMember = "Id"
      };

      btnLoadInboxes = new Button
      {
         Text = "🔄 بارگذاری",
         Width = 90,
         Height = 28,
         Margin = new Padding(5, 0, 0, 0)
      };

      inboxPanel.Controls.Add(cmbInbox);
      inboxPanel.Controls.Add(btnLoadInboxes);
      layout.Controls.Add(inboxPanel, 1, 5);

      tab.Controls.Add(layout);
      return tab;
   }

   private TabPage CreateGeneralTab()
   {
      var tab = new TabPage("عمومی") { Padding = new Padding(15) };

      var layout = new TableLayoutPanel
      {
         Dock = DockStyle.Fill,
         ColumnCount = 2,
         RowCount = 3,
         AutoSize = true
      };
      layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35));
      layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65));

      // Row 0: Start Minimized
      layout.Controls.Add(CreateLabel("شروع کوچک شده:"), 0, 0);
      chkStartMinimized = new CheckBox
      {
         Text = "در System Tray شروع شود",
         Dock = DockStyle.Fill
      };
      layout.Controls.Add(chkStartMinimized, 1, 0);

      // Row 1: Auto Sync
      layout.Controls.Add(CreateLabel("همگام‌سازی خودکار:"), 0, 1);
      chkAutoSync = new CheckBox
      {
         Text = "همگام‌سازی با Chatwoot فعال باشد",
         Dock = DockStyle.Fill
      };
      layout.Controls.Add(chkAutoSync, 1, 1);

      // Row 2: Sync Interval
      layout.Controls.Add(CreateLabel("فاصله همگام‌سازی (دقیقه):"), 0, 2);
      numSyncInterval = new NumericUpDown
      {
         Minimum = 1,
         Maximum = 60,
         Value = 5,
         Width = 80
      };
      layout.Controls.Add(numSyncInterval, 1, 2);

      tab.Controls.Add(layout);
      return tab;
   }

   private FlowLayoutPanel CreateFooterPanel()
   {
      var footer = new FlowLayoutPanel
      {
         Dock = DockStyle.Bottom,
         Height = 50,
         FlowDirection = FlowDirection.LeftToRight,
         Padding = new Padding(10),
         RightToLeft = RightToLeft.No
      };

      btnCancel = new Button
      {
         Text = "انصراف",
         Width = 90,
         Height = 35,
         DialogResult = DialogResult.Cancel
      };

      btnSave = new Button
      {
         Text = "💾 ذخیره",
         Width = 100,
         Height = 35,
         BackColor = Color.FromArgb(0, 120, 215),
         ForeColor = Color.White,
         FlatStyle = FlatStyle.Flat
      };

      footer.Controls.Add(btnSave);
      footer.Controls.Add(btnCancel);

      return footer;
   }

   private static Label CreateLabel(string text)
   {
      return new Label
      {
         Text = text,
         Dock = DockStyle.Fill,
         TextAlign = ContentAlignment.MiddleRight
      };
   }

   // ═══════════════════════════════════════════════════════════════
   // Event Handlers Setup
   // ═══════════════════════════════════════════════════════════════

   private void SetupEventHandlers()
   {
      this.Load += SettingsForm_Load;
      this.FormClosing += SettingsForm_FormClosing;

      btnSave.Click += BtnSave_Click;
      btnCancel.Click += (s, e) => this.Close();
      btnTestConnection.Click += BtnTestConnection_Click;
      btnLoadInboxes.Click += BtnLoadInboxes_Click;

      // Track dirty state
      txtBaseUrl.TextChanged += MarkDirty;
      txtApiKey.TextChanged += MarkDirty;
      numAccountId.ValueChanged += MarkDirty;
      cmbInbox.SelectedIndexChanged += MarkDirty;
      chkStartMinimized.CheckedChanged += MarkDirty;
      chkAutoSync.CheckedChanged += MarkDirty;
      numSyncInterval.ValueChanged += MarkDirty;
   }

   private void MarkDirty(object? sender, EventArgs e)
   {
      _isDirty = true;
   }

   // ═══════════════════════════════════════════════════════════════
   // Form Load - Load Settings
   // ═══════════════════════════════════════════════════════════════

   private async void SettingsForm_Load(object? sender, EventArgs e)
   {
      try
      {
         await LoadSettingsAsync();
         _isDirty = false;
      }
      catch (Exception ex)
      {
         MessageBox.Show(
             $"خطا در بارگذاری تنظیمات:\n{ex.Message}",
             "خطا",
             MessageBoxButtons.OK,
             MessageBoxIcon.Error
         );
      }
   }

   private async Task LoadSettingsAsync()
   {
      // Load Chatwoot Settings
      var chatwootSettings = await _settingsService.GetChatwootSettingsAsync();

      txtBaseUrl.Text = chatwootSettings.BaseUrl ?? string.Empty;
      txtApiKey.Text = chatwootSettings.ApiKey ?? string.Empty;
      numAccountId.Value = chatwootSettings.AccountId > 0 ? chatwootSettings.AccountId : 1;

      // Store pending inbox ID to select after loading inboxes
      _pendingDefaultInboxId = chatwootSettings.DefaultInboxId ?? 0;

      // Load General Settings
      chkStartMinimized.Checked = _settingsService.GetBool("StartMinimized", false);
      chkAutoSync.Checked = _settingsService.GetBool("AutoSyncEnabled", true);
      numSyncInterval.Value = _settingsService.GetInt("SyncIntervalMinutes", 5);
   }

   // ═══════════════════════════════════════════════════════════════
   // Save Settings
   // ═══════════════════════════════════════════════════════════════

   private async void BtnSave_Click(object? sender, EventArgs e)
   {
      try
      {
         btnSave.Enabled = false;
         btnSave.Text = "در حال ذخیره...";

         await SaveSettingsAsync();

         _isDirty = false;

         MessageBox.Show(
             "تنظیمات با موفقیت ذخیره شد ✅",
             "موفق",
             MessageBoxButtons.OK,
             MessageBoxIcon.Information
         );

         this.DialogResult = DialogResult.OK;
         this.Close();
      }
      
      catch (Exception ex)
      {
         MessageBox.Show(
             $"خطا در ذخیره تنظیمات:\n{ex.Message}",
             "خطا",
             MessageBoxButtons.OK,
             MessageBoxIcon.Error
         );
      }
      finally
      {
         btnSave.Enabled = true;
         btnSave.Text = "💾 ذخیره";
      }
   }

   private async Task SaveSettingsAsync()
   {
      // Save Chatwoot Settings
      var chatwootSettings = new ChatwootSettings
      {
         BaseUrl = txtBaseUrl.Text.Trim(),
         ApiKey = txtApiKey.Text.Trim(),
         AccountId = (int)numAccountId.Value,
         DefaultInboxId = cmbInbox.SelectedItem?.GetType().GetProperty("Id")?.GetValue(cmbInbox.SelectedItem) as int?
      };

      await _settingsService.SaveChatwootSettingsAsync(chatwootSettings);

      // Save General Settings (as string values)
      await _settingsService.SetValueAsync("StartMinimized", chkStartMinimized.Checked.ToString());
      await _settingsService.SetValueAsync("AutoSyncEnabled", chkAutoSync.Checked.ToString());
      await _settingsService.SetValueAsync("SyncIntervalMinutes", numSyncInterval.Value.ToString());
   }

   // ═══════════════════════════════════════════════════════════════
   // Chatwoot API Actions
   // ═══════════════════════════════════════════════════════════════

   private async void BtnTestConnection_Click(object? sender, EventArgs e)
   {
      try
      {
         btnTestConnection.Enabled = false;
         lblConnectionStatus.Text = "در حال تست...";
         lblConnectionStatus.ForeColor = Color.Black;

         _chatwootClient.ResetConfiguration();

         var result = await _chatwootClient.GetProfileAsync();

         if (result.IsSuccess && result.Data != null)
         {
            lblConnectionStatus.Text = "✅ اتصال موفق";
            lblConnectionStatus.ForeColor = Color.Green;
         }
         else
         {
            lblConnectionStatus.Text = "❌ اتصال ناموفق";
            lblConnectionStatus.ForeColor = Color.Red;
         }
      }
      catch (Exception ex)
      {
         lblConnectionStatus.Text = "❌ خطا در اتصال";
         lblConnectionStatus.ForeColor = Color.Red;

         MessageBox.Show(
             $"خطا در تست اتصال:\n{ex.Message}",
             "خطا",
             MessageBoxButtons.OK,
             MessageBoxIcon.Warning
         );
      }
      finally
      {
         btnTestConnection.Enabled = true;
      }
   }

   private async void BtnLoadInboxes_Click(object? sender, EventArgs e)
   {
      try
      {
         btnLoadInboxes.Enabled = false;
         cmbInbox.DataSource = null;

         var result = await _chatwootClient.GetInboxesAsync();

         if (!result.IsSuccess || result.Data == null)
         {
            MessageBox.Show("دریافت لیست Inboxها ناموفق بود.", "خطا",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
         }

         cmbInbox.DataSource = result.Data;

         // Select previously saved inbox
         if (_pendingDefaultInboxId > 0)
         {
            foreach (var item in result.Data)
            {
               var idProp = item?.GetType().GetProperty("Id");
               if ((int?)idProp?.GetValue(item)! == _pendingDefaultInboxId)
               {
                  cmbInbox.SelectedItem = item;
                  break;
               }
            }
         }
      }
      catch (Exception ex)
      {
         MessageBox.Show(
             $"خطا در بارگذاری Inboxها:\n{ex.Message}",
             "خطا",
             MessageBoxButtons.OK,
             MessageBoxIcon.Error
         );
      }
      finally
      {
         btnLoadInboxes.Enabled = true;
      }
   }

   // ═══════════════════════════════════════════════════════════════
   // Form Closing - Dirty Check
   // ═══════════════════════════════════════════════════════════════

   private void SettingsForm_FormClosing(object? sender, FormClosingEventArgs e)
   {
      if (!_isDirty) return;

      var result = MessageBox.Show(
          "تغییرات ذخیره نشده‌اند. آیا می‌خواهید خارج شوید؟",
          "تأیید خروج",
          MessageBoxButtons.YesNo,
          MessageBoxIcon.Question
      );

      if (result == DialogResult.No)
      {
         e.Cancel = true;
      }
   }
}
