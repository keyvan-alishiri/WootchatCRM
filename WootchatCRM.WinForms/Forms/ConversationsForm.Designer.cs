namespace WootchatCRM.UI.Forms;

partial class ConversationsForm : UserControl
{
   private System.ComponentModel.IContainer components = null;

   protected override void Dispose(bool disposing)
   {
      if (disposing && (components != null))
      {
		 StopTimers();
		 components.Dispose();
      }
      base.Dispose(disposing);
   }

   #region Windows Form Designer generated code

   private void InitializeComponent()
   {
		 this.splitContainerMain = new System.Windows.Forms.SplitContainer();
		 this.listViewConversations = new System.Windows.Forms.ListView();
		 this.panelPagination = new System.Windows.Forms.Panel();
		 this.btnNextPage = new System.Windows.Forms.Button();
		 this.lblPageInfo = new System.Windows.Forms.Label();
		 this.btnPreviousPage = new System.Windows.Forms.Button();
		 this.panelListHeader = new System.Windows.Forms.Panel();
		 this.panelFilters = new System.Windows.Forms.Panel();
		 this.btnRefresh = new System.Windows.Forms.Button();
		 this.btnSearch = new System.Windows.Forms.Button();
		 this.txtSearch = new System.Windows.Forms.TextBox();
		 this.cmbInboxFilter = new System.Windows.Forms.ComboBox();
		 this.lblInboxFilter = new System.Windows.Forms.Label();
		 this.cmbStatusFilter = new System.Windows.Forms.ComboBox();
		 this.lblStatusFilter = new System.Windows.Forms.Label();
		 this.rtbMessages = new System.Windows.Forms.RichTextBox();
		 this.panelMessageInput = new System.Windows.Forms.Panel();
		 this.btnSendMessage = new System.Windows.Forms.Button();
		 this.chkPrivateNote = new System.Windows.Forms.CheckBox();
		 this.txtMessageInput = new System.Windows.Forms.TextBox();
		 this.panelMessageHeader = new System.Windows.Forms.Panel();
		 this.btnChangeStatus = new System.Windows.Forms.Button();
		 this.cmbConversationStatus = new System.Windows.Forms.ComboBox();
		 this.lblConversationTitle = new System.Windows.Forms.Label();
		 this.statusStrip = new System.Windows.Forms.StatusStrip();
		 this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
		 this.progressBar = new System.Windows.Forms.ToolStripProgressBar();
		 ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).BeginInit();
		 this.splitContainerMain.Panel1.SuspendLayout();
		 this.splitContainerMain.Panel2.SuspendLayout();
		 this.splitContainerMain.SuspendLayout();
		 this.panelPagination.SuspendLayout();
		 this.panelListHeader.SuspendLayout();
		 this.panelFilters.SuspendLayout();
		 this.panelMessageInput.SuspendLayout();
		 this.panelMessageHeader.SuspendLayout();
		 this.statusStrip.SuspendLayout();
		 this.SuspendLayout();
		 // 
		 // splitContainerMain
		 // 
		 this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
		 this.splitContainerMain.Location = new System.Drawing.Point(0, 0);
		 this.splitContainerMain.Name = "splitContainerMain";
		 // 
		 // splitContainerMain.Panel1
		 // 
		 this.splitContainerMain.Panel1.Controls.Add(this.listViewConversations);
		 this.splitContainerMain.Panel1.Controls.Add(this.panelPagination);
		 this.splitContainerMain.Panel1.Controls.Add(this.panelListHeader);
		 this.splitContainerMain.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
		 // 
		 // splitContainerMain.Panel2
		 // 
		 this.splitContainerMain.Panel2.Controls.Add(this.rtbMessages);
		 this.splitContainerMain.Panel2.Controls.Add(this.panelMessageInput);
		 this.splitContainerMain.Panel2.Controls.Add(this.panelMessageHeader);
		 this.splitContainerMain.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
		 this.splitContainerMain.Size = new System.Drawing.Size(1121, 382);
		 this.splitContainerMain.SplitterDistance = 419;
		 this.splitContainerMain.TabIndex = 0;
		 // 
		 // listViewConversations
		 // 
		 this.listViewConversations.Dock = System.Windows.Forms.DockStyle.Fill;
		 this.listViewConversations.FullRowSelect = true;
		 this.listViewConversations.Location = new System.Drawing.Point(0, 100);
		 this.listViewConversations.MultiSelect = false;
		 this.listViewConversations.Name = "listViewConversations";
		 this.listViewConversations.Size = new System.Drawing.Size(419, 165);
		 this.listViewConversations.TabIndex = 1;
		 this.listViewConversations.UseCompatibleStateImageBehavior = false;
		 this.listViewConversations.View = System.Windows.Forms.View.Details;
		 // 
		 // panelPagination
		 // 
		 this.panelPagination.Controls.Add(this.btnNextPage);
		 this.panelPagination.Controls.Add(this.lblPageInfo);
		 this.panelPagination.Controls.Add(this.btnPreviousPage);
		 this.panelPagination.Dock = System.Windows.Forms.DockStyle.Bottom;
		 this.panelPagination.Location = new System.Drawing.Point(0, 265);
		 this.panelPagination.Name = "panelPagination";
		 this.panelPagination.Size = new System.Drawing.Size(419, 117);
		 this.panelPagination.TabIndex = 2;
		 // 
		 // btnNextPage
		 // 
		 this.btnNextPage.Anchor = System.Windows.Forms.AnchorStyles.None;
		 this.btnNextPage.Location = new System.Drawing.Point(69, 14);
		 this.btnNextPage.Name = "btnNextPage";
		 this.btnNextPage.Size = new System.Drawing.Size(75, 25);
		 this.btnNextPage.TabIndex = 2;
		 this.btnNextPage.Text = "❯ بعدی";
		 this.btnNextPage.UseVisualStyleBackColor = true;
		 // 
		 // lblPageInfo
		 // 
		 this.lblPageInfo.Anchor = System.Windows.Forms.AnchorStyles.None;
		 this.lblPageInfo.Location = new System.Drawing.Point(149, 18);
		 this.lblPageInfo.Name = "lblPageInfo";
		 this.lblPageInfo.Size = new System.Drawing.Size(110, 17);
		 this.lblPageInfo.TabIndex = 1;
		 this.lblPageInfo.Text = "صفحه 1 از 1";
		 this.lblPageInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		 // 
		 // btnPreviousPage
		 // 
		 this.btnPreviousPage.Anchor = System.Windows.Forms.AnchorStyles.None;
		 this.btnPreviousPage.Location = new System.Drawing.Point(264, 14);
		 this.btnPreviousPage.Name = "btnPreviousPage";
		 this.btnPreviousPage.Size = new System.Drawing.Size(75, 25);
		 this.btnPreviousPage.TabIndex = 0;
		 this.btnPreviousPage.Text = "قبلی ❮";
		 this.btnPreviousPage.UseVisualStyleBackColor = true;
		 // 
		 // panelListHeader
		 // 
		 this.panelListHeader.Controls.Add(this.panelFilters);
		 this.panelListHeader.Dock = System.Windows.Forms.DockStyle.Top;
		 this.panelListHeader.Location = new System.Drawing.Point(0, 0);
		 this.panelListHeader.Name = "panelListHeader";
		 this.panelListHeader.Padding = new System.Windows.Forms.Padding(8);
		 this.panelListHeader.Size = new System.Drawing.Size(419, 100);
		 this.panelListHeader.TabIndex = 0;
		 // 
		 // panelFilters
		 // 
		 this.panelFilters.Controls.Add(this.btnRefresh);
		 this.panelFilters.Controls.Add(this.btnSearch);
		 this.panelFilters.Controls.Add(this.txtSearch);
		 this.panelFilters.Controls.Add(this.cmbInboxFilter);
		 this.panelFilters.Controls.Add(this.lblInboxFilter);
		 this.panelFilters.Controls.Add(this.cmbStatusFilter);
		 this.panelFilters.Controls.Add(this.lblStatusFilter);
		 this.panelFilters.Dock = System.Windows.Forms.DockStyle.Fill;
		 this.panelFilters.Location = new System.Drawing.Point(8, 8);
		 this.panelFilters.Name = "panelFilters";
		 this.panelFilters.Size = new System.Drawing.Size(403, 84);
		 this.panelFilters.TabIndex = 0;
		 // 
		 // btnRefresh
		 // 
		 this.btnRefresh.Location = new System.Drawing.Point(341, 39);
		 this.btnRefresh.Name = "btnRefresh";
		 this.btnRefresh.Size = new System.Drawing.Size(45, 25);
		 this.btnRefresh.TabIndex = 6;
		 this.btnRefresh.Text = "🔄";
		 this.btnRefresh.UseVisualStyleBackColor = true;
		 // 
		 // btnSearch
		 // 
		 this.btnSearch.Location = new System.Drawing.Point(31, 39);
		 this.btnSearch.Name = "btnSearch";
		 this.btnSearch.Size = new System.Drawing.Size(45, 25);
		 this.btnSearch.TabIndex = 5;
		 this.btnSearch.Text = "🔍";
		 this.btnSearch.UseVisualStyleBackColor = true;
		 // 
		 // txtSearch
		 // 
		 this.txtSearch.Location = new System.Drawing.Point(81, 40);
		 this.txtSearch.Name = "txtSearch";
		 this.txtSearch.PlaceholderText = "جستجو در مکالمات...";
		 this.txtSearch.Size = new System.Drawing.Size(255, 23);
		 this.txtSearch.TabIndex = 4;
		 // 
		 // cmbInboxFilter
		 // 
		 this.cmbInboxFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		 this.cmbInboxFilter.Location = new System.Drawing.Point(30, 7);
		 this.cmbInboxFilter.Name = "cmbInboxFilter";
		 this.cmbInboxFilter.Size = new System.Drawing.Size(125, 23);
		 this.cmbInboxFilter.TabIndex = 3;
		 // 
		 // lblInboxFilter
		 // 
		 this.lblInboxFilter.AutoSize = true;
		 this.lblInboxFilter.Location = new System.Drawing.Point(160, 10);
		 this.lblInboxFilter.Name = "lblInboxFilter";
		 this.lblInboxFilter.Size = new System.Drawing.Size(47, 15);
		 this.lblInboxFilter.TabIndex = 2;
		 this.lblInboxFilter.Text = "صندوق:";
		 // 
		 // cmbStatusFilter
		 // 
		 this.cmbStatusFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		 this.cmbStatusFilter.Location = new System.Drawing.Point(220, 7);
		 this.cmbStatusFilter.Name = "cmbStatusFilter";
		 this.cmbStatusFilter.Size = new System.Drawing.Size(115, 23);
		 this.cmbStatusFilter.TabIndex = 1;
		 // 
		 // lblStatusFilter
		 // 
		 this.lblStatusFilter.AutoSize = true;
		 this.lblStatusFilter.Location = new System.Drawing.Point(340, 10);
		 this.lblStatusFilter.Name = "lblStatusFilter";
		 this.lblStatusFilter.Size = new System.Drawing.Size(50, 15);
		 this.lblStatusFilter.TabIndex = 0;
		 this.lblStatusFilter.Text = "وضعیت:";
		 // 
		 // rtbMessages
		 // 
		 this.rtbMessages.BackColor = System.Drawing.Color.White;
		 this.rtbMessages.BorderStyle = System.Windows.Forms.BorderStyle.None;
		 this.rtbMessages.Dock = System.Windows.Forms.DockStyle.Fill;
		 this.rtbMessages.Location = new System.Drawing.Point(0, 60);
		 this.rtbMessages.Name = "rtbMessages";
		 this.rtbMessages.ReadOnly = true;
		 this.rtbMessages.Size = new System.Drawing.Size(698, 205);
		 this.rtbMessages.TabIndex = 1;
		 this.rtbMessages.Text = "";
		 // 
		 // panelMessageInput
		 // 
		 this.panelMessageInput.Controls.Add(this.btnSendMessage);
		 this.panelMessageInput.Controls.Add(this.chkPrivateNote);
		 this.panelMessageInput.Controls.Add(this.txtMessageInput);
		 this.panelMessageInput.Dock = System.Windows.Forms.DockStyle.Bottom;
		 this.panelMessageInput.Location = new System.Drawing.Point(0, 265);
		 this.panelMessageInput.Name = "panelMessageInput";
		 this.panelMessageInput.Padding = new System.Windows.Forms.Padding(8);
		 this.panelMessageInput.Size = new System.Drawing.Size(698, 117);
		 this.panelMessageInput.TabIndex = 2;
		 // 
		 // btnSendMessage
		 // 
		 this.btnSendMessage.Location = new System.Drawing.Point(10, 14);
		 this.btnSendMessage.Name = "btnSendMessage";
		 this.btnSendMessage.Size = new System.Drawing.Size(90, 25);
		 this.btnSendMessage.TabIndex = 2;
		 this.btnSendMessage.Text = "ارسال";
		 this.btnSendMessage.UseVisualStyleBackColor = true;
		 // 
		 // chkPrivateNote
		 // 
		 this.chkPrivateNote.Location = new System.Drawing.Point(611, 16);
		 this.chkPrivateNote.Name = "chkPrivateNote";
		 this.chkPrivateNote.Size = new System.Drawing.Size(71, 20);
		 this.chkPrivateNote.TabIndex = 1;
		 this.chkPrivateNote.Text = "یادداشت";
		 this.chkPrivateNote.UseVisualStyleBackColor = true;
		 // 
		 // txtMessageInput
		 // 
		 this.txtMessageInput.Location = new System.Drawing.Point(110, 14);
		 this.txtMessageInput.Multiline = true;
		 this.txtMessageInput.Name = "txtMessageInput";
		 this.txtMessageInput.Size = new System.Drawing.Size(490, 25);
		 this.txtMessageInput.TabIndex = 0;
		 // 
		 // panelMessageHeader
		 // 
		 this.panelMessageHeader.Controls.Add(this.btnChangeStatus);
		 this.panelMessageHeader.Controls.Add(this.cmbConversationStatus);
		 this.panelMessageHeader.Controls.Add(this.lblConversationTitle);
		 this.panelMessageHeader.Dock = System.Windows.Forms.DockStyle.Top;
		 this.panelMessageHeader.Location = new System.Drawing.Point(0, 0);
		 this.panelMessageHeader.Name = "panelMessageHeader";
		 this.panelMessageHeader.Padding = new System.Windows.Forms.Padding(8);
		 this.panelMessageHeader.Size = new System.Drawing.Size(698, 60);
		 this.panelMessageHeader.TabIndex = 0;
		 // 
		 // btnChangeStatus
		 // 
		 this.btnChangeStatus.Location = new System.Drawing.Point(552, 17);
		 this.btnChangeStatus.Name = "btnChangeStatus";
		 this.btnChangeStatus.Size = new System.Drawing.Size(130, 25);
		 this.btnChangeStatus.TabIndex = 2;
		 this.btnChangeStatus.Text = "تغییر وضعیت";
		 this.btnChangeStatus.UseVisualStyleBackColor = true;
		 // 
		 // cmbConversationStatus
		 // 
		 this.cmbConversationStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		 this.cmbConversationStatus.Location = new System.Drawing.Point(415, 18);
		 this.cmbConversationStatus.Name = "cmbConversationStatus";
		 this.cmbConversationStatus.Size = new System.Drawing.Size(120, 23);
		 this.cmbConversationStatus.TabIndex = 1;
		 // 
		 // lblConversationTitle
		 // 
		 this.lblConversationTitle.Dock = System.Windows.Forms.DockStyle.Fill;
		 this.lblConversationTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
		 this.lblConversationTitle.Location = new System.Drawing.Point(8, 8);
		 this.lblConversationTitle.Name = "lblConversationTitle";
		 this.lblConversationTitle.Size = new System.Drawing.Size(682, 44);
		 this.lblConversationTitle.TabIndex = 0;
		 this.lblConversationTitle.Text = "انتخاب مکالمه";
		 this.lblConversationTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		 // 
		 // statusStrip
		 // 
		 this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus,
            this.progressBar});
		 this.statusStrip.Location = new System.Drawing.Point(0, 382);
		 this.statusStrip.Name = "statusStrip";
		 this.statusStrip.Size = new System.Drawing.Size(1121, 22);
		 this.statusStrip.TabIndex = 1;
		 // 
		 // lblStatus
		 // 
		 this.lblStatus.Name = "lblStatus";
		 this.lblStatus.Size = new System.Drawing.Size(973, 17);
		 this.lblStatus.Spring = true;
		 this.lblStatus.Text = "آماده";
		 // 
		 // progressBar
		 // 
		 this.progressBar.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
		 this.progressBar.Name = "progressBar";
		 this.progressBar.Size = new System.Drawing.Size(100, 16);
		 this.progressBar.Visible = false;
		 // 
		 // ConversationsForm
		 // 
		 this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
		 this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		 this.Controls.Add(this.splitContainerMain);
		 this.Controls.Add(this.statusStrip);
		 this.Name = "ConversationsForm";
		 this.Size = new System.Drawing.Size(1121, 404);
		 this.splitContainerMain.Panel1.ResumeLayout(false);
		 this.splitContainerMain.Panel2.ResumeLayout(false);
		 ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).EndInit();
		 this.splitContainerMain.ResumeLayout(false);
		 this.panelPagination.ResumeLayout(false);
		 this.panelListHeader.ResumeLayout(false);
		 this.panelFilters.ResumeLayout(false);
		 this.panelFilters.PerformLayout();
		 this.panelMessageInput.ResumeLayout(false);
		 this.panelMessageInput.PerformLayout();
		 this.panelMessageHeader.ResumeLayout(false);
		 this.statusStrip.ResumeLayout(false);
		 this.statusStrip.PerformLayout();
		 this.ResumeLayout(false);
		 this.PerformLayout();

   }

   #endregion

   private System.Windows.Forms.SplitContainer splitContainerMain;
   private System.Windows.Forms.Panel panelListHeader;
   private System.Windows.Forms.Panel panelFilters;
   private System.Windows.Forms.Panel panelPagination;
   private System.Windows.Forms.Panel panelMessageHeader;
   private System.Windows.Forms.Panel panelMessageInput;

   private System.Windows.Forms.ListView listViewConversations;
   private System.Windows.Forms.RichTextBox rtbMessages;

   private System.Windows.Forms.Label lblStatusFilter;
   private System.Windows.Forms.ComboBox cmbStatusFilter;
   private System.Windows.Forms.Label lblInboxFilter;
   private System.Windows.Forms.ComboBox cmbInboxFilter;
   private System.Windows.Forms.TextBox txtSearch;
   private System.Windows.Forms.Button btnSearch;
   private System.Windows.Forms.Button btnRefresh;

   private System.Windows.Forms.Button btnPreviousPage;
   private System.Windows.Forms.Label lblPageInfo;
   private System.Windows.Forms.Button btnNextPage;

   private System.Windows.Forms.Label lblConversationTitle;
   private System.Windows.Forms.ComboBox cmbConversationStatus;
   private System.Windows.Forms.Button btnChangeStatus;

   private System.Windows.Forms.TextBox txtMessageInput;
   private System.Windows.Forms.CheckBox chkPrivateNote;
   private System.Windows.Forms.Button btnSendMessage;

   private System.Windows.Forms.StatusStrip statusStrip;
   private System.Windows.Forms.ToolStripStatusLabel lblStatus;
   private System.Windows.Forms.ToolStripProgressBar progressBar;
}
