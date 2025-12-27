namespace WootchatCRM.WinForms.Contacts
{
   partial class NewConversationForm
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

      #region Windows Form Designer generated code

      private void InitializeComponent()
      {
         this.panelHeader = new System.Windows.Forms.Panel();
         this.lblHeader = new System.Windows.Forms.Label();

         this.panelContactInfo = new System.Windows.Forms.Panel();
         this.lblContactNameTitle = new System.Windows.Forms.Label();
         this.lblContactName = new System.Windows.Forms.Label();
         this.lblContactPhoneTitle = new System.Windows.Forms.Label();
         this.lblContactPhone = new System.Windows.Forms.Label();
         this.lblContactEmailTitle = new System.Windows.Forms.Label();
         this.lblContactEmail = new System.Windows.Forms.Label();

         this.panelChannelSelection = new System.Windows.Forms.Panel();
         this.grpChatwoot = new System.Windows.Forms.GroupBox();
         this.rdoChatwoot = new System.Windows.Forms.RadioButton();
         this.panelInbox = new System.Windows.Forms.Panel();
         this.lblInbox = new System.Windows.Forms.Label();
         this.cmbInbox = new System.Windows.Forms.ComboBox();

         this.grpTelegram = new System.Windows.Forms.GroupBox();
         this.rdoTelegramDirect = new System.Windows.Forms.RadioButton();

         this.panelMessage = new System.Windows.Forms.Panel();
         this.lblMessage = new System.Windows.Forms.Label();
         this.txtMessage = new System.Windows.Forms.TextBox();
         this.lblCharCount = new System.Windows.Forms.Label();

         this.panelButtons = new System.Windows.Forms.Panel();
         this.btnSend = new System.Windows.Forms.Button();
         this.btnCancel = new System.Windows.Forms.Button();
         this.lblStatus = new System.Windows.Forms.Label();
         this.progressBar = new System.Windows.Forms.ProgressBar();

         // 
         // panelHeader
         // 
         this.panelHeader.BackColor = System.Drawing.Color.FromArgb(37, 99, 235);
         this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
         this.panelHeader.Location = new System.Drawing.Point(0, 0);
         this.panelHeader.Name = "panelHeader";
         this.panelHeader.Size = new System.Drawing.Size(500, 50);
         this.panelHeader.TabIndex = 0;
         this.panelHeader.Controls.Add(this.lblHeader);

         // 
         // lblHeader
         // 
         this.lblHeader.Dock = System.Windows.Forms.DockStyle.Fill;
         this.lblHeader.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
         this.lblHeader.ForeColor = System.Drawing.Color.White;
         this.lblHeader.Location = new System.Drawing.Point(0, 0);
         this.lblHeader.Name = "lblHeader";
         this.lblHeader.Size = new System.Drawing.Size(500, 50);
         this.lblHeader.TabIndex = 0;
         this.lblHeader.Text = "ارسال پیام جدید";
         this.lblHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

         // 
         // panelContactInfo
         // 
         this.panelContactInfo.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
         this.panelContactInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.panelContactInfo.Location = new System.Drawing.Point(12, 60);
         this.panelContactInfo.Name = "panelContactInfo";
         this.panelContactInfo.Padding = new System.Windows.Forms.Padding(10);
         this.panelContactInfo.Size = new System.Drawing.Size(476, 80);
         this.panelContactInfo.TabIndex = 1;
         this.panelContactInfo.Controls.Add(this.lblContactNameTitle);
         this.panelContactInfo.Controls.Add(this.lblContactName);
         this.panelContactInfo.Controls.Add(this.lblContactPhoneTitle);
         this.panelContactInfo.Controls.Add(this.lblContactPhone);
         this.panelContactInfo.Controls.Add(this.lblContactEmailTitle);
         this.panelContactInfo.Controls.Add(this.lblContactEmail);

         // 
         // lblContactNameTitle
         // 
         this.lblContactNameTitle.AutoSize = true;
         this.lblContactNameTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
         this.lblContactNameTitle.Location = new System.Drawing.Point(380, 10);
         this.lblContactNameTitle.Name = "lblContactNameTitle";
         this.lblContactNameTitle.Size = new System.Drawing.Size(35, 15);
         this.lblContactNameTitle.TabIndex = 0;
         this.lblContactNameTitle.Text = "نام:";

         // 
         // lblContactName
         // 
         this.lblContactName.AutoSize = true;
         this.lblContactName.Location = new System.Drawing.Point(200, 10);
         this.lblContactName.Name = "lblContactName";
         this.lblContactName.Size = new System.Drawing.Size(170, 15);
         this.lblContactName.TabIndex = 1;
         this.lblContactName.Text = "---";

         // 
         // lblContactPhoneTitle
         // 
         this.lblContactPhoneTitle.AutoSize = true;
         this.lblContactPhoneTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
         this.lblContactPhoneTitle.Location = new System.Drawing.Point(380, 32);
         this.lblContactPhoneTitle.Name = "lblContactPhoneTitle";
         this.lblContactPhoneTitle.Size = new System.Drawing.Size(50, 15);
         this.lblContactPhoneTitle.TabIndex = 2;
         this.lblContactPhoneTitle.Text = "موبایل:";

         // 
         // lblContactPhone
         // 
         this.lblContactPhone.AutoSize = true;
         this.lblContactPhone.Location = new System.Drawing.Point(200, 32);
         this.lblContactPhone.Name = "lblContactPhone";
         this.lblContactPhone.Size = new System.Drawing.Size(170, 15);
         this.lblContactPhone.TabIndex = 3;
         this.lblContactPhone.Text = "---";

         // 
         // lblContactEmailTitle
         // 
         this.lblContactEmailTitle.AutoSize = true;
         this.lblContactEmailTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
         this.lblContactEmailTitle.Location = new System.Drawing.Point(380, 54);
         this.lblContactEmailTitle.Name = "lblContactEmailTitle";
         this.lblContactEmailTitle.Size = new System.Drawing.Size(45, 15);
         this.lblContactEmailTitle.TabIndex = 4;
         this.lblContactEmailTitle.Text = "ایمیل:";

         // 
         // lblContactEmail
         // 
         this.lblContactEmail.AutoSize = true;
         this.lblContactEmail.Location = new System.Drawing.Point(200, 54);
         this.lblContactEmail.Name = "lblContactEmail";
         this.lblContactEmail.Size = new System.Drawing.Size(170, 15);
         this.lblContactEmail.TabIndex = 5;
         this.lblContactEmail.Text = "---";

         // 
         // panelChannelSelection
         // 
         this.panelChannelSelection.Location = new System.Drawing.Point(12, 150);
         this.panelChannelSelection.Name = "panelChannelSelection";
         this.panelChannelSelection.Size = new System.Drawing.Size(476, 130);
         this.panelChannelSelection.TabIndex = 2;
         this.panelChannelSelection.Controls.Add(this.grpChatwoot);
         this.panelChannelSelection.Controls.Add(this.grpTelegram);

         // 
         // grpChatwoot
         // 
         this.grpChatwoot.Location = new System.Drawing.Point(240, 0);
         this.grpChatwoot.Name = "grpChatwoot";
         this.grpChatwoot.Size = new System.Drawing.Size(230, 125);
         this.grpChatwoot.TabIndex = 0;
         this.grpChatwoot.TabStop = false;
         this.grpChatwoot.Text = "Chatwoot";
         this.grpChatwoot.Controls.Add(this.rdoChatwoot);
         this.grpChatwoot.Controls.Add(this.panelInbox);

         // 
         // rdoChatwoot
         // 
         this.rdoChatwoot.AutoSize = true;
         this.rdoChatwoot.Checked = true;
         this.rdoChatwoot.Location = new System.Drawing.Point(130, 22);
         this.rdoChatwoot.Name = "rdoChatwoot";
         this.rdoChatwoot.Size = new System.Drawing.Size(90, 19);
         this.rdoChatwoot.TabIndex = 0;
         this.rdoChatwoot.TabStop = true;
         this.rdoChatwoot.Text = "از Chatwoot";
         this.rdoChatwoot.UseVisualStyleBackColor = true;

         // 
         // panelInbox
         // 
         this.panelInbox.Location = new System.Drawing.Point(10, 50);
         this.panelInbox.Name = "panelInbox";
         this.panelInbox.Size = new System.Drawing.Size(210, 65);
         this.panelInbox.TabIndex = 1;
         this.panelInbox.Controls.Add(this.lblInbox);
         this.panelInbox.Controls.Add(this.cmbInbox);

         // 
         // lblInbox
         // 
         this.lblInbox.AutoSize = true;
         this.lblInbox.Location = new System.Drawing.Point(160, 8);
         this.lblInbox.Name = "lblInbox";
         this.lblInbox.Size = new System.Drawing.Size(40, 15);
         this.lblInbox.TabIndex = 0;
         this.lblInbox.Text = "کانال:";

         // 
         // cmbInbox
         // 
         this.cmbInbox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.cmbInbox.FormattingEnabled = true;
         this.cmbInbox.Location = new System.Drawing.Point(5, 30);
         this.cmbInbox.Name = "cmbInbox";
         this.cmbInbox.Size = new System.Drawing.Size(200, 23);
         this.cmbInbox.TabIndex = 1;

         // 
         // grpTelegram
         // 
         this.grpTelegram.Location = new System.Drawing.Point(0, 0);
         this.grpTelegram.Name = "grpTelegram";
         this.grpTelegram.Size = new System.Drawing.Size(230, 125);
         this.grpTelegram.TabIndex = 1;
         this.grpTelegram.TabStop = false;
         this.grpTelegram.Text = "Telegram Direct";
         this.grpTelegram.Controls.Add(this.rdoTelegramDirect);

         // 
         // rdoTelegramDirect
         // 
         this.rdoTelegramDirect.AutoSize = true;
         this.rdoTelegramDirect.Location = new System.Drawing.Point(80, 50);
         this.rdoTelegramDirect.Name = "rdoTelegramDirect";
         this.rdoTelegramDirect.Size = new System.Drawing.Size(130, 19);
         this.rdoTelegramDirect.TabIndex = 0;
         this.rdoTelegramDirect.Text = "ارسال مستقیم Telegram";
         this.rdoTelegramDirect.UseVisualStyleBackColor = true;

         // 
         // panelMessage
         // 
         this.panelMessage.Location = new System.Drawing.Point(12, 290);
         this.panelMessage.Name = "panelMessage";
         this.panelMessage.Size = new System.Drawing.Size(476, 180);
         this.panelMessage.TabIndex = 3;
         this.panelMessage.Controls.Add(this.lblMessage);
         this.panelMessage.Controls.Add(this.txtMessage);
         this.panelMessage.Controls.Add(this.lblCharCount);

         // 
         // lblMessage
         // 
         this.lblMessage.AutoSize = true;
         this.lblMessage.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
         this.lblMessage.Location = new System.Drawing.Point(410, 5);
         this.lblMessage.Name = "lblMessage";
         this.lblMessage.Size = new System.Drawing.Size(60, 15);
         this.lblMessage.TabIndex = 0;
         this.lblMessage.Text = "متن پیام:";

         // 
         // txtMessage
         // 
         this.txtMessage.Location = new System.Drawing.Point(0, 25);
         this.txtMessage.MaxLength = 4096;
         this.txtMessage.Multiline = true;
         this.txtMessage.Name = "txtMessage";
         this.txtMessage.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
         this.txtMessage.Size = new System.Drawing.Size(476, 130);
         this.txtMessage.TabIndex = 1;

         // 
         // lblCharCount
         // 
         this.lblCharCount.AutoSize = true;
         this.lblCharCount.ForeColor = System.Drawing.Color.Gray;
         this.lblCharCount.Location = new System.Drawing.Point(0, 160);
         this.lblCharCount.Name = "lblCharCount";
         this.lblCharCount.Size = new System.Drawing.Size(60, 15);
         this.lblCharCount.TabIndex = 2;
         this.lblCharCount.Text = "0 / 4096";

         // 
         // panelButtons
         // 
         this.panelButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
         this.panelButtons.Location = new System.Drawing.Point(0, 480);
         this.panelButtons.Name = "panelButtons";
         this.panelButtons.Size = new System.Drawing.Size(500, 70);
         this.panelButtons.TabIndex = 4;
         this.panelButtons.Controls.Add(this.btnSend);
         this.panelButtons.Controls.Add(this.btnCancel);
         this.panelButtons.Controls.Add(this.lblStatus);
         this.panelButtons.Controls.Add(this.progressBar);

         // 
         // btnSend
         // 
         this.btnSend.BackColor = System.Drawing.Color.FromArgb(34, 197, 94);
         this.btnSend.FlatAppearance.BorderSize = 0;
         this.btnSend.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
         this.btnSend.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
         this.btnSend.ForeColor = System.Drawing.Color.White;
         this.btnSend.Location = new System.Drawing.Point(280, 35);
         this.btnSend.Name = "btnSend";
         this.btnSend.Size = new System.Drawing.Size(100, 30);
         this.btnSend.TabIndex = 0;
         this.btnSend.Text = "ارسال";
         this.btnSend.UseVisualStyleBackColor = false;

         // 
         // btnCancel
         // 
         this.btnCancel.BackColor = System.Drawing.Color.FromArgb(107, 114, 128);
         this.btnCancel.FlatAppearance.BorderSize = 0;
         this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
         this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 10F);
         this.btnCancel.ForeColor = System.Drawing.Color.White;
         this.btnCancel.Location = new System.Drawing.Point(390, 35);
         this.btnCancel.Name = "btnCancel";
         this.btnCancel.Size = new System.Drawing.Size(100, 30);
         this.btnCancel.TabIndex = 1;
         this.btnCancel.Text = "انصراف";
         this.btnCancel.UseVisualStyleBackColor = false;

         // 
         // lblStatus
         // 
         this.lblStatus.AutoSize = true;
         this.lblStatus.Location = new System.Drawing.Point(12, 10);
         this.lblStatus.Name = "lblStatus";
         this.lblStatus.Size = new System.Drawing.Size(50, 15);
         this.lblStatus.TabIndex = 2;
         this.lblStatus.Text = "آماده";

         // 
         // progressBar
         // 
         this.progressBar.Location = new System.Drawing.Point(12, 40);
         this.progressBar.MarqueeAnimationSpeed = 30;
         this.progressBar.Name = "progressBar";
         this.progressBar.Size = new System.Drawing.Size(150, 20);
         this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
         this.progressBar.TabIndex = 3;
         this.progressBar.Visible = false;

         // 
         // NewConversationForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackColor = System.Drawing.Color.White;
         this.ClientSize = new System.Drawing.Size(500, 550);
         this.Controls.Add(this.panelHeader);
         this.Controls.Add(this.panelContactInfo);
         this.Controls.Add(this.panelChannelSelection);
         this.Controls.Add(this.panelMessage);
         this.Controls.Add(this.panelButtons);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
         this.MaximizeBox = false;
         this.MinimizeBox = false;
         this.Name = "NewConversationForm";
         this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
         this.RightToLeftLayout = true;
         this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
         this.Text = "ارسال پیام جدید";

         this.panelHeader.ResumeLayout(false);
         this.panelContactInfo.ResumeLayout(false);
         this.panelContactInfo.PerformLayout();
         this.panelChannelSelection.ResumeLayout(false);
         this.grpChatwoot.ResumeLayout(false);
         this.grpChatwoot.PerformLayout();
         this.panelInbox.ResumeLayout(false);
         this.panelInbox.PerformLayout();
         this.grpTelegram.ResumeLayout(false);
         this.grpTelegram.PerformLayout();
         this.panelMessage.ResumeLayout(false);
         this.panelMessage.PerformLayout();
         this.panelButtons.ResumeLayout(false);
         this.panelButtons.PerformLayout();
         this.ResumeLayout(false);
      }

      #endregion

      private System.Windows.Forms.Panel panelHeader;
      private System.Windows.Forms.Label lblHeader;

      private System.Windows.Forms.Panel panelContactInfo;
      private System.Windows.Forms.Label lblContactNameTitle;
      private System.Windows.Forms.Label lblContactName;
      private System.Windows.Forms.Label lblContactPhoneTitle;
      private System.Windows.Forms.Label lblContactPhone;
      private System.Windows.Forms.Label lblContactEmailTitle;
      private System.Windows.Forms.Label lblContactEmail;

      private System.Windows.Forms.Panel panelChannelSelection;
      private System.Windows.Forms.GroupBox grpChatwoot;
      private System.Windows.Forms.RadioButton rdoChatwoot;
      private System.Windows.Forms.Panel panelInbox;
      private System.Windows.Forms.Label lblInbox;
      private System.Windows.Forms.ComboBox cmbInbox;

      private System.Windows.Forms.GroupBox grpTelegram;
      private System.Windows.Forms.RadioButton rdoTelegramDirect;

      private System.Windows.Forms.Panel panelMessage;
      private System.Windows.Forms.Label lblMessage;
      private System.Windows.Forms.TextBox txtMessage;
      private System.Windows.Forms.Label lblCharCount;

      private System.Windows.Forms.Panel panelButtons;
      private System.Windows.Forms.Button btnSend;
      private System.Windows.Forms.Button btnCancel;
      private System.Windows.Forms.Label lblStatus;
      private System.Windows.Forms.ProgressBar progressBar;
   }
}
