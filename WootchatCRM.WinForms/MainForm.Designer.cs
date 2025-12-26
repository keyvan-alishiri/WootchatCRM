namespace WootchatCRM.WinForms;

partial class MainForm
{
   private System.ComponentModel.IContainer components = null;

   protected override void Dispose(bool disposing)
   {
      if (disposing && (components != null))
      {
         components.Dispose();
      }
      base.Dispose(disposing);
   }

   private void InitializeComponent()
   {
      this.pnlSidebar = new Panel();
      this.btnSettings = new Button();
      this.btnTags = new Button();
      this.btnUsers = new Button();
      this.btnCampaigns = new Button();
      this.btnConversations = new Button();
      this.btnContacts = new Button();
      this.btnDashboard = new Button();
      this.pnlLogo = new Panel();
      this.lblLogo = new Label();
      this.pnlMain = new Panel();
      this.pnlContent = new Panel();
      this.pnlStatusBar = new Panel();
      this.lblSyncStatus = new Label();
      this.lblConnectionStatus = new Label();
      this.pnlHeader = new Panel();
      this.lblPageTitle = new Label();

      this.pnlSidebar.SuspendLayout();
      this.pnlLogo.SuspendLayout();
      this.pnlMain.SuspendLayout();
      this.pnlStatusBar.SuspendLayout();
      this.pnlHeader.SuspendLayout();
      this.SuspendLayout();

      // ═══════════════════════════════════════════════════════════
      // pnlSidebar - پنل منوی کناری
      // ═══════════════════════════════════════════════════════════
      this.pnlSidebar.BackColor = Color.FromArgb(45, 55, 72);
      this.pnlSidebar.Controls.Add(this.btnSettings);
      this.pnlSidebar.Controls.Add(this.btnTags);
      this.pnlSidebar.Controls.Add(this.btnUsers);
      this.pnlSidebar.Controls.Add(this.btnCampaigns);
      this.pnlSidebar.Controls.Add(this.btnConversations);
      this.pnlSidebar.Controls.Add(this.btnContacts);
      this.pnlSidebar.Controls.Add(this.btnDashboard);
      this.pnlSidebar.Controls.Add(this.pnlLogo);
      this.pnlSidebar.Dock = DockStyle.Right;
      this.pnlSidebar.Location = new Point(784, 0);
      this.pnlSidebar.Name = "pnlSidebar";
      this.pnlSidebar.Size = new Size(200, 561);
      this.pnlSidebar.TabIndex = 0;

      // ═══════════════════════════════════════════════════════════
      // pnlLogo - لوگو بالای منو
      // ═══════════════════════════════════════════════════════════
      this.pnlLogo.BackColor = Color.FromArgb(26, 32, 44);
      this.pnlLogo.Controls.Add(this.lblLogo);
      this.pnlLogo.Dock = DockStyle.Top;
      this.pnlLogo.Location = new Point(0, 0);
      this.pnlLogo.Name = "pnlLogo";
      this.pnlLogo.Size = new Size(200, 60);
      this.pnlLogo.TabIndex = 0;

      // lblLogo
      this.lblLogo.Dock = DockStyle.Fill;
      this.lblLogo.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
      this.lblLogo.ForeColor = Color.White;
      this.lblLogo.Location = new Point(0, 0);
      this.lblLogo.Name = "lblLogo";
      this.lblLogo.Size = new Size(200, 60);
      this.lblLogo.TabIndex = 0;
      this.lblLogo.Text = "🏠 WootchatCRM";
      this.lblLogo.TextAlign = ContentAlignment.MiddleCenter;

      // ═══════════════════════════════════════════════════════════
      // دکمه‌های منو - استایل مشترک
      // ═══════════════════════════════════════════════════════════

      // btnDashboard
      this.btnDashboard.Cursor = Cursors.Hand;
      this.btnDashboard.Dock = DockStyle.Top;
      this.btnDashboard.FlatAppearance.BorderSize = 0;
      this.btnDashboard.FlatAppearance.MouseOverBackColor = Color.FromArgb(74, 85, 104);
      this.btnDashboard.FlatStyle = FlatStyle.Flat;
      this.btnDashboard.Font = new Font("Segoe UI", 11F);
      this.btnDashboard.ForeColor = Color.White;
      this.btnDashboard.Location = new Point(0, 60);
      this.btnDashboard.Name = "btnDashboard";
      this.btnDashboard.Padding = new Padding(15, 0, 0, 0);
      this.btnDashboard.Size = new Size(200, 45);
      this.btnDashboard.TabIndex = 1;
      this.btnDashboard.Text = "📊  داشبورد";
      this.btnDashboard.TextAlign = ContentAlignment.MiddleRight;
      this.btnDashboard.UseVisualStyleBackColor = false;
      this.btnDashboard.BackColor = Color.FromArgb(74, 85, 104); // Active by default
      this.btnDashboard.Click += new EventHandler(this.btnDashboard_Click);

      // btnContacts
      this.btnContacts.Cursor = Cursors.Hand;
      this.btnContacts.Dock = DockStyle.Top;
      this.btnContacts.FlatAppearance.BorderSize = 0;
      this.btnContacts.FlatAppearance.MouseOverBackColor = Color.FromArgb(74, 85, 104);
      this.btnContacts.FlatStyle = FlatStyle.Flat;
      this.btnContacts.Font = new Font("Segoe UI", 11F);
      this.btnContacts.ForeColor = Color.White;
      this.btnContacts.Location = new Point(0, 105);
      this.btnContacts.Name = "btnContacts";
      this.btnContacts.Padding = new Padding(15, 0, 0, 0);
      this.btnContacts.Size = new Size(200, 45);
      this.btnContacts.TabIndex = 2;
      this.btnContacts.Text = "👥  مخاطبین";
      this.btnContacts.TextAlign = ContentAlignment.MiddleRight;
      this.btnContacts.UseVisualStyleBackColor = false;
      this.btnContacts.BackColor = Color.FromArgb(45, 55, 72);
      this.btnContacts.Click += new EventHandler(this.btnContacts_Click);

      // btnConversations
      this.btnConversations.Cursor = Cursors.Hand;
      this.btnConversations.Dock = DockStyle.Top;
      this.btnConversations.FlatAppearance.BorderSize = 0;
      this.btnConversations.FlatAppearance.MouseOverBackColor = Color.FromArgb(74, 85, 104);
      this.btnConversations.FlatStyle = FlatStyle.Flat;
      this.btnConversations.Font = new Font("Segoe UI", 11F);
      this.btnConversations.ForeColor = Color.White;
      this.btnConversations.Location = new Point(0, 150);
      this.btnConversations.Name = "btnConversations";
      this.btnConversations.Padding = new Padding(15, 0, 0, 0);
      this.btnConversations.Size = new Size(200, 45);
      this.btnConversations.TabIndex = 3;
      this.btnConversations.Text = "💬  مکالمات";
      this.btnConversations.TextAlign = ContentAlignment.MiddleRight;
      this.btnConversations.UseVisualStyleBackColor = false;
      this.btnConversations.BackColor = Color.FromArgb(45, 55, 72);
      this.btnConversations.Click += new EventHandler(this.btnConversations_Click);

      // btnCampaigns
      this.btnCampaigns.Cursor = Cursors.Hand;
      this.btnCampaigns.Dock = DockStyle.Top;
      this.btnCampaigns.FlatAppearance.BorderSize = 0;
      this.btnCampaigns.FlatAppearance.MouseOverBackColor = Color.FromArgb(74, 85, 104);
      this.btnCampaigns.FlatStyle = FlatStyle.Flat;
      this.btnCampaigns.Font = new Font("Segoe UI", 11F);
      this.btnCampaigns.ForeColor = Color.White;
      this.btnCampaigns.Location = new Point(0, 195);
      this.btnCampaigns.Name = "btnCampaigns";
      this.btnCampaigns.Padding = new Padding(15, 0, 0, 0);
      this.btnCampaigns.Size = new Size(200, 45);
      this.btnCampaigns.TabIndex = 4;
      this.btnCampaigns.Text = "📢  کمپین‌ها";
      this.btnCampaigns.TextAlign = ContentAlignment.MiddleRight;
      this.btnCampaigns.UseVisualStyleBackColor = false;
      this.btnCampaigns.BackColor = Color.FromArgb(45, 55, 72);
      this.btnCampaigns.Click += new EventHandler(this.btnCampaigns_Click);

      // btnUsers
      this.btnUsers.Cursor = Cursors.Hand;
      this.btnUsers.Dock = DockStyle.Top;
      this.btnUsers.FlatAppearance.BorderSize = 0;
      this.btnUsers.FlatAppearance.MouseOverBackColor = Color.FromArgb(74, 85, 104);
      this.btnUsers.FlatStyle = FlatStyle.Flat;
      this.btnUsers.Font = new Font("Segoe UI", 11F);
      this.btnUsers.ForeColor = Color.White;
      this.btnUsers.Location = new Point(0, 240);
      this.btnUsers.Name = "btnUsers";
      this.btnUsers.Padding = new Padding(15, 0, 0, 0);
      this.btnUsers.Size = new Size(200, 45);
      this.btnUsers.TabIndex = 5;
      this.btnUsers.Text = "👤  کاربران";
      this.btnUsers.TextAlign = ContentAlignment.MiddleRight;
      this.btnUsers.UseVisualStyleBackColor = false;
      this.btnUsers.BackColor = Color.FromArgb(45, 55, 72);
      this.btnUsers.Click += new EventHandler(this.btnUsers_Click);

      // btnTags
      this.btnTags.Cursor = Cursors.Hand;
      this.btnTags.Dock = DockStyle.Top;
      this.btnTags.FlatAppearance.BorderSize = 0;
      this.btnTags.FlatAppearance.MouseOverBackColor = Color.FromArgb(74, 85, 104);
      this.btnTags.FlatStyle = FlatStyle.Flat;
      this.btnTags.Font = new Font("Segoe UI", 11F);
      this.btnTags.ForeColor = Color.White;
      this.btnTags.Location = new Point(0, 285);
      this.btnTags.Name = "btnTags";
      this.btnTags.Padding = new Padding(15, 0, 0, 0);
      this.btnTags.Size = new Size(200, 45);
      this.btnTags.TabIndex = 6;
      this.btnTags.Text = "🏷️  تگ‌ها";
      this.btnTags.TextAlign = ContentAlignment.MiddleRight;
      this.btnTags.UseVisualStyleBackColor = false;
      this.btnTags.BackColor = Color.FromArgb(45, 55, 72);
      this.btnTags.Click += new EventHandler(this.btnTags_Click);

      // btnSettings
      this.btnSettings.Cursor = Cursors.Hand;
      this.btnSettings.Dock = DockStyle.Bottom;
      this.btnSettings.FlatAppearance.BorderSize = 0;
      this.btnSettings.FlatAppearance.MouseOverBackColor = Color.FromArgb(74, 85, 104);
      this.btnSettings.FlatStyle = FlatStyle.Flat;
      this.btnSettings.Font = new Font("Segoe UI", 11F);
      this.btnSettings.ForeColor = Color.White;
      this.btnSettings.Location = new Point(0, 516);
      this.btnSettings.Name = "btnSettings";
      this.btnSettings.Padding = new Padding(15, 0, 0, 0);
      this.btnSettings.Size = new Size(200, 45);
      this.btnSettings.TabIndex = 7;
      this.btnSettings.Text = "⚙️  تنظیمات";
      this.btnSettings.TextAlign = ContentAlignment.MiddleRight;
      this.btnSettings.UseVisualStyleBackColor = false;
      this.btnSettings.BackColor = Color.FromArgb(45, 55, 72);
      this.btnSettings.Click += new EventHandler(this.btnSettings_Click);

      // ═══════════════════════════════════════════════════════════
      // pnlMain - پنل اصلی محتوا
      // ═══════════════════════════════════════════════════════════
      this.pnlMain.BackColor = Color.FromArgb(247, 250, 252);
      this.pnlMain.Controls.Add(this.pnlContent);
      this.pnlMain.Controls.Add(this.pnlStatusBar);
      this.pnlMain.Controls.Add(this.pnlHeader);
      this.pnlMain.Dock = DockStyle.Fill;
      this.pnlMain.Location = new Point(0, 0);
      this.pnlMain.Name = "pnlMain";
      this.pnlMain.Size = new Size(784, 561);
      this.pnlMain.TabIndex = 1;

      
        // pnlHeader - هدر بالای محتوا
        // ═══════════════════════════════════════════════════════════
      this.pnlHeader.BackColor = Color.White;
      this.pnlHeader.Controls.Add(this.lblPageTitle);
      this.pnlHeader.Dock = DockStyle.Top;
      this.pnlHeader.Location = new Point(0, 0);
      this.pnlHeader.Name = "pnlHeader";
      this.pnlHeader.Size = new Size(784, 60);
      this.pnlHeader.TabIndex = 0;

      // lblPageTitle
      this.lblPageTitle.Dock = DockStyle.Fill;
      this.lblPageTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
      this.lblPageTitle.ForeColor = Color.FromArgb(45, 55, 72);
      this.lblPageTitle.Location = new Point(0, 0);
      this.lblPageTitle.Name = "lblPageTitle";
      this.lblPageTitle.Padding = new Padding(20, 0, 0, 0);
      this.lblPageTitle.Size = new Size(784, 60);
      this.lblPageTitle.TabIndex = 0;
      this.lblPageTitle.Text = "📊 داشبورد";
      this.lblPageTitle.TextAlign = ContentAlignment.MiddleLeft;

      // ═══════════════════════════════════════════════════════════
      // pnlStatusBar - نوار وضعیت پایین
      // ═══════════════════════════════════════════════════════════
      this.pnlStatusBar.BackColor = Color.White;
      this.pnlStatusBar.Controls.Add(this.lblSyncStatus);
      this.pnlStatusBar.Controls.Add(this.lblConnectionStatus);
      this.pnlStatusBar.Dock = DockStyle.Bottom;
      this.pnlStatusBar.Location = new Point(0, 531);
      this.pnlStatusBar.Name = "pnlStatusBar";
      this.pnlStatusBar.Size = new Size(784, 30);
      this.pnlStatusBar.TabIndex = 1;

      // lblConnectionStatus
      this.lblConnectionStatus.AutoSize = true;
      this.lblConnectionStatus.Font = new Font("Segoe UI", 9F);
      this.lblConnectionStatus.ForeColor = Color.FromArgb(56, 161, 105);
      this.lblConnectionStatus.Location = new Point(10, 7);
      this.lblConnectionStatus.Name = "lblConnectionStatus";
      this.lblConnectionStatus.Size = new Size(130, 15);
      this.lblConnectionStatus.TabIndex = 0;
      this.lblConnectionStatus.Text = "✅ متصل به Chatwoot";

      // lblSyncStatus
      this.lblSyncStatus.Anchor = AnchorStyles.Right;
      this.lblSyncStatus.AutoSize = true;
      this.lblSyncStatus.Font = new Font("Segoe UI", 9F);
      this.lblSyncStatus.ForeColor = Color.FromArgb(113, 128, 150);
      this.lblSyncStatus.Location = new Point(600, 7);
      this.lblSyncStatus.Name = "lblSyncStatus";
      this.lblSyncStatus.Size = new Size(170, 15);
      this.lblSyncStatus.TabIndex = 1;
      this.lblSyncStatus.Text = "آخرین همگام‌سازی: --:--";

      // ═══════════════════════════════════════════════════════════
      // pnlContent - محتوای داینامیک
      // ═══════════════════════════════════════════════════════════
      this.pnlContent.Dock = DockStyle.Fill;
      this.pnlContent.Location = new Point(0, 60);
      this.pnlContent.Name = "pnlContent";
      this.pnlContent.Padding = new Padding(20);
      this.pnlContent.Size = new Size(784, 471);
      this.pnlContent.TabIndex = 2;

      // ═══════════════════════════════════════════════════════════
      // MainForm
      // ═══════════════════════════════════════════════════════════
      this.AutoScaleDimensions = new SizeF(7F, 15F);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(984, 561);
      this.Controls.Add(this.pnlMain);
      this.Controls.Add(this.pnlSidebar);
      this.Font = new Font("Segoe UI", 9F);
      this.MinimumSize = new Size(1000, 600);
      this.Name = "MainForm";
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "WootchatCRM";
      this.Load += new EventHandler(this.MainForm_Load);

      this.pnlSidebar.ResumeLayout(false);
      this.pnlLogo.ResumeLayout(false);
      this.pnlMain.ResumeLayout(false);
      this.pnlStatusBar.ResumeLayout(false);
      this.pnlStatusBar.PerformLayout();
      this.pnlHeader.ResumeLayout(false);
      this.ResumeLayout(false);
   }

   private Panel pnlSidebar;
   private Panel pnlLogo;
   private Label lblLogo;
   private Button btnDashboard;
   private Button btnContacts;
   private Button btnConversations;
   private Button btnCampaigns;
   private Button btnUsers;
   private Button btnTags;
   private Button btnSettings;
   private Panel pnlMain;
   private Panel pnlHeader;
   private Label lblPageTitle;
   private Panel pnlContent;
   private Panel pnlStatusBar;
   private Label lblConnectionStatus;
   private Label lblSyncStatus;
}
