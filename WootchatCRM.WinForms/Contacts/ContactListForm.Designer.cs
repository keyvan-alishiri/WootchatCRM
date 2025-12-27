namespace WootchatCRM.Forms.Contacts
{
   partial class ContactListForm
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

	  #region Component Designer generated code

	  private void InitializeComponent()
	  {
		 this.pnlHeader = new System.Windows.Forms.Panel();
		 this.lblTitle = new System.Windows.Forms.Label();
		 this.pnlToolbar = new System.Windows.Forms.Panel();
		 this.pnlButtons = new System.Windows.Forms.Panel();
		 this.btnRefresh = new System.Windows.Forms.Button();
		 this.btnDelete = new System.Windows.Forms.Button();
		 this.btnEdit = new System.Windows.Forms.Button();
		 this.btnAdd = new System.Windows.Forms.Button();
		 this.btnSendMessage = new System.Windows.Forms.Button();
		 this.pnlSearchBox = new System.Windows.Forms.Panel();
		 this.btnClearSearch = new System.Windows.Forms.Button();
		 this.btnSearch = new System.Windows.Forms.Button();
		 this.txtSearch = new System.Windows.Forms.TextBox();
		 this.dgvContacts = new System.Windows.Forms.DataGridView();
		 this.colId = new System.Windows.Forms.DataGridViewTextBoxColumn();
		 this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
		 this.colEmail = new System.Windows.Forms.DataGridViewTextBoxColumn();
		 this.colPhoneNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
		 this.colChatwootId = new System.Windows.Forms.DataGridViewTextBoxColumn();
		 this.colCreatedAt = new System.Windows.Forms.DataGridViewTextBoxColumn();
		 this.pnlStatusBar = new System.Windows.Forms.Panel();
		 this.lblStatus = new System.Windows.Forms.Label();
		 this.lblCount = new System.Windows.Forms.Label();
		 this.pnlHeader.SuspendLayout();
		 this.pnlToolbar.SuspendLayout();
		 this.pnlButtons.SuspendLayout();
		 this.pnlSearchBox.SuspendLayout();
		 ((System.ComponentModel.ISupportInitialize)(this.dgvContacts)).BeginInit();
		 this.pnlStatusBar.SuspendLayout();
		 this.SuspendLayout();
		 // 
		 // pnlHeader
		 // 
		 this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
		 this.pnlHeader.Controls.Add(this.lblTitle);
		 this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
		 this.pnlHeader.Location = new System.Drawing.Point(0, 0);
		 this.pnlHeader.Name = "pnlHeader";
		 this.pnlHeader.Size = new System.Drawing.Size(1000, 60);
		 this.pnlHeader.TabIndex = 3;
		 // 
		 // lblTitle
		 // 
		 this.lblTitle.Anchor = System.Windows.Forms.AnchorStyles.Right;
		 this.lblTitle.AutoSize = true;
		 this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
		 this.lblTitle.ForeColor = System.Drawing.Color.White;
		 this.lblTitle.Location = new System.Drawing.Point(770, 15);
		 this.lblTitle.Name = "lblTitle";
		 this.lblTitle.Size = new System.Drawing.Size(92, 30);
		 this.lblTitle.TabIndex = 0;
		 this.lblTitle.Text = "مخاطبین";
		 // 
		 // pnlToolbar
		 // 
		 this.pnlToolbar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
		 this.pnlToolbar.Controls.Add(this.pnlButtons);
		 this.pnlToolbar.Controls.Add(this.pnlSearchBox);
		 this.pnlToolbar.Dock = System.Windows.Forms.DockStyle.Top;
		 this.pnlToolbar.Location = new System.Drawing.Point(0, 60);
		 this.pnlToolbar.Name = "pnlToolbar";
		 this.pnlToolbar.Padding = new System.Windows.Forms.Padding(10);
		 this.pnlToolbar.Size = new System.Drawing.Size(1000, 70);
		 this.pnlToolbar.TabIndex = 2;
		 // 
		 // pnlButtons
		 // 
		 this.pnlButtons.Controls.Add(this.btnRefresh);
		 this.pnlButtons.Controls.Add(this.btnDelete);
		 this.pnlButtons.Controls.Add(this.btnEdit);
		 this.pnlButtons.Controls.Add(this.btnAdd);
		 this.pnlButtons.Controls.Add(this.btnSendMessage);
		 this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Left;
		 this.pnlButtons.Location = new System.Drawing.Point(10, 10);
		 this.pnlButtons.Name = "pnlButtons";
		 this.pnlButtons.Size = new System.Drawing.Size(523, 50);
		 this.pnlButtons.TabIndex = 0;
		 // 
		 // btnRefresh
		 // 
		 this.btnRefresh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(89)))), ((int)(((byte)(182)))));
		 this.btnRefresh.FlatAppearance.BorderSize = 0;
		 this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		 this.btnRefresh.ForeColor = System.Drawing.Color.White;
		 this.btnRefresh.Location = new System.Drawing.Point(0, 8);
		 this.btnRefresh.Name = "btnRefresh";
		 this.btnRefresh.Size = new System.Drawing.Size(100, 35);
		 this.btnRefresh.TabIndex = 0;
		 this.btnRefresh.Text = "بروزرسانی";
		 this.btnRefresh.UseVisualStyleBackColor = false;
		 // 
		 // btnDelete
		 // 
		 this.btnDelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(76)))), ((int)(((byte)(60)))));
		 this.btnDelete.FlatAppearance.BorderSize = 0;
		 this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		 this.btnDelete.ForeColor = System.Drawing.Color.White;
		 this.btnDelete.Location = new System.Drawing.Point(105, 8);
		 this.btnDelete.Name = "btnDelete";
		 this.btnDelete.Size = new System.Drawing.Size(100, 35);
		 this.btnDelete.TabIndex = 1;
		 this.btnDelete.Text = "حذف";
		 this.btnDelete.UseVisualStyleBackColor = false;
		 // 
		 // btnEdit
		 // 
		 this.btnEdit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
		 this.btnEdit.FlatAppearance.BorderSize = 0;
		 this.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		 this.btnEdit.ForeColor = System.Drawing.Color.White;
		 this.btnEdit.Location = new System.Drawing.Point(210, 8);
		 this.btnEdit.Name = "btnEdit";
		 this.btnEdit.Size = new System.Drawing.Size(100, 35);
		 this.btnEdit.TabIndex = 2;
		 this.btnEdit.Text = "ویرایش";
		 this.btnEdit.UseVisualStyleBackColor = false;
		 // 
		 // btnAdd
		 // 
		 this.btnAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
		 this.btnAdd.FlatAppearance.BorderSize = 0;
		 this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		 this.btnAdd.ForeColor = System.Drawing.Color.White;
		 this.btnAdd.Location = new System.Drawing.Point(315, 8);
		 this.btnAdd.Name = "btnAdd";
		 this.btnAdd.Size = new System.Drawing.Size(100, 35);
		 this.btnAdd.TabIndex = 3;
		 this.btnAdd.Text = "افزودن";
		 this.btnAdd.UseVisualStyleBackColor = false;
		 // 
		 // btnSendMessage
		 // 
		 this.btnSendMessage.BackColor = System.Drawing.Color.FromArgb(26, 188, 156);
		 this.btnSendMessage.FlatAppearance.BorderSize = 0;
		 this.btnSendMessage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		 this.btnSendMessage.ForeColor = System.Drawing.Color.White;
		 this.btnSendMessage.Location = new System.Drawing.Point(420, 8);
		 this.btnSendMessage.Name = "btnSendMessage";
		 this.btnSendMessage.Size = new System.Drawing.Size(100, 35);
		 this.btnSendMessage.TabIndex = 4;
		 this.btnSendMessage.Text = "💬 ارسال پیام";
		 this.btnSendMessage.UseVisualStyleBackColor = false;
		 // 
		 // pnlSearchBox
		 // 
		 this.pnlSearchBox.Controls.Add(this.btnClearSearch);
		 this.pnlSearchBox.Controls.Add(this.btnSearch);
		 this.pnlSearchBox.Controls.Add(this.txtSearch);
		 this.pnlSearchBox.Dock = System.Windows.Forms.DockStyle.Right;
		 this.pnlSearchBox.Location = new System.Drawing.Point(640, 10);
		 this.pnlSearchBox.Name = "pnlSearchBox";
		 this.pnlSearchBox.Size = new System.Drawing.Size(350, 50);
		 this.pnlSearchBox.TabIndex = 1;
		 // 
		 // btnClearSearch
		 // 
		 this.btnClearSearch.Location = new System.Drawing.Point(5, 9);
		 this.btnClearSearch.Name = "btnClearSearch";
		 this.btnClearSearch.Size = new System.Drawing.Size(35, 29);
		 this.btnClearSearch.TabIndex = 0;
		 this.btnClearSearch.Text = "✕";
		 // 
		 // btnSearch
		 // 
		 this.btnSearch.Location = new System.Drawing.Point(45, 9);
		 this.btnSearch.Name = "btnSearch";
		 this.btnSearch.Size = new System.Drawing.Size(40, 29);
		 this.btnSearch.TabIndex = 1;
		 this.btnSearch.Text = "🔍";
		 // 
		 // txtSearch
		 // 
		 this.txtSearch.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
		 this.txtSearch.Location = new System.Drawing.Point(90, 10);
		 this.txtSearch.Name = "txtSearch";
		 this.txtSearch.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
		 this.txtSearch.Size = new System.Drawing.Size(250, 27);
		 this.txtSearch.TabIndex = 2;
		 // 
		 // dgvContacts
		 // 
		 this.dgvContacts.AllowUserToAddRows = false;
		 this.dgvContacts.AllowUserToDeleteRows = false;
		 this.dgvContacts.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colId,
            this.colName,
            this.colEmail,
            this.colPhoneNumber,
            this.colChatwootId,
            this.colCreatedAt});
		 this.dgvContacts.Dock = System.Windows.Forms.DockStyle.Fill;
		 this.dgvContacts.Location = new System.Drawing.Point(0, 130);
		 this.dgvContacts.Name = "dgvContacts";
		 this.dgvContacts.ReadOnly = true;
		 this.dgvContacts.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
		 this.dgvContacts.RowHeadersVisible = false;
		 this.dgvContacts.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
		 this.dgvContacts.Size = new System.Drawing.Size(1000, 440);
		 this.dgvContacts.TabIndex = 0;
		 // 
		 // colId
		 // 
		 this.colId.DataPropertyName = "Id";
		 this.colId.HeaderText = "ID";
		 this.colId.Name = "colId";
		 this.colId.ReadOnly = true;
		 // 
		 // colName
		 // 
		 this.colName.DataPropertyName = "Name";
		 this.colName.HeaderText = "نام";
		 this.colName.Name = "colName";
		 this.colName.ReadOnly = true;
		 // 
		 // colEmail
		 // 
		 this.colEmail.DataPropertyName = "Email";
		 this.colEmail.HeaderText = "ایمیل";
		 this.colEmail.Name = "colEmail";
		 this.colEmail.ReadOnly = true;
		 // 
		 // colPhoneNumber
		 // 
		 this.colPhoneNumber.DataPropertyName = "PhoneNumber";
		 this.colPhoneNumber.HeaderText = "تلفن";
		 this.colPhoneNumber.Name = "colPhoneNumber";
		 this.colPhoneNumber.ReadOnly = true;
		 // 
		 // colChatwootId
		 // 
		 this.colChatwootId.DataPropertyName = "ChatwootContactId";
		 this.colChatwootId.HeaderText = "Chatwoot ID";
		 this.colChatwootId.Name = "colChatwootId";
		 this.colChatwootId.ReadOnly = true;
		 // 
		 // colCreatedAt
		 // 
		 this.colCreatedAt.DataPropertyName = "CreatedAt";
		 this.colCreatedAt.HeaderText = "تاریخ ایجاد";
		 this.colCreatedAt.Name = "colCreatedAt";
		 this.colCreatedAt.ReadOnly = true;
		 // 
		 // pnlStatusBar
		 // 
		 this.pnlStatusBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
		 this.pnlStatusBar.Controls.Add(this.lblStatus);
		 this.pnlStatusBar.Controls.Add(this.lblCount);
		 this.pnlStatusBar.Dock = System.Windows.Forms.DockStyle.Bottom;
		 this.pnlStatusBar.Location = new System.Drawing.Point(0, 570);
		 this.pnlStatusBar.Name = "pnlStatusBar";
		 this.pnlStatusBar.Size = new System.Drawing.Size(1000, 30);
		 this.pnlStatusBar.TabIndex = 1;
		 // 
		 // lblStatus
		 // 
		 this.lblStatus.ForeColor = System.Drawing.Color.White;
		 this.lblStatus.Location = new System.Drawing.Point(10, 6);
		 this.lblStatus.Name = "lblStatus";
		 this.lblStatus.Size = new System.Drawing.Size(100, 23);
		 this.lblStatus.TabIndex = 0;
		 this.lblStatus.Text = "آماده";
		 // 
		 // lblCount
		 // 
		 this.lblCount.Anchor = System.Windows.Forms.AnchorStyles.Right;
		 this.lblCount.ForeColor = System.Drawing.Color.White;
		 this.lblCount.Location = new System.Drawing.Point(850, 6);
		 this.lblCount.Name = "lblCount";
		 this.lblCount.Size = new System.Drawing.Size(100, 23);
		 this.lblCount.TabIndex = 1;
		 this.lblCount.Text = "0";
		 // 
		 // ContactListForm
		 // 
		 this.Controls.Add(this.dgvContacts);
		 this.Controls.Add(this.pnlStatusBar);
		 this.Controls.Add(this.pnlToolbar);
		 this.Controls.Add(this.pnlHeader);
		 this.Name = "ContactListForm";
		 this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
		 this.Size = new System.Drawing.Size(1000, 600);
		 this.pnlHeader.ResumeLayout(false);
		 this.pnlHeader.PerformLayout();
		 this.pnlToolbar.ResumeLayout(false);
		 this.pnlButtons.ResumeLayout(false);
		 this.pnlSearchBox.ResumeLayout(false);
		 this.pnlSearchBox.PerformLayout();
		 ((System.ComponentModel.ISupportInitialize)(this.dgvContacts)).EndInit();
		 this.pnlStatusBar.ResumeLayout(false);
		 this.ResumeLayout(false);

	  }

	  #endregion

	  private System.Windows.Forms.Panel pnlHeader;
	  private System.Windows.Forms.Label lblTitle;
	  private System.Windows.Forms.Panel pnlToolbar;
	  private System.Windows.Forms.Panel pnlButtons;
	  private System.Windows.Forms.Button btnRefresh;
	  private System.Windows.Forms.Button btnDelete;
	  private System.Windows.Forms.Button btnEdit;
	  private System.Windows.Forms.Button btnAdd;
	  private System.Windows.Forms.Panel pnlSearchBox;
	  private System.Windows.Forms.Button btnClearSearch;
	  private System.Windows.Forms.Button btnSearch;
	  private System.Windows.Forms.TextBox txtSearch;
	  private System.Windows.Forms.DataGridView dgvContacts;
	  private System.Windows.Forms.DataGridViewTextBoxColumn colId;
	  private System.Windows.Forms.DataGridViewTextBoxColumn colName;
	  private System.Windows.Forms.DataGridViewTextBoxColumn colEmail;
	  private System.Windows.Forms.DataGridViewTextBoxColumn colPhoneNumber;
	  private System.Windows.Forms.DataGridViewTextBoxColumn colChatwootId;
	  private System.Windows.Forms.DataGridViewTextBoxColumn colCreatedAt;
	  private System.Windows.Forms.Panel pnlStatusBar;
	  private System.Windows.Forms.Label lblStatus;
	  private System.Windows.Forms.Label lblCount;
	  private System.Windows.Forms.Button btnSendMessage;
   }
}
