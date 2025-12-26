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

		 // pnlHeader
		 this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(52, 73, 94);
		 this.pnlHeader.Controls.Add(this.lblTitle);
		 this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
		 this.pnlHeader.Size = new System.Drawing.Size(1000, 60);

		 // lblTitle
		 this.lblTitle.Anchor = System.Windows.Forms.AnchorStyles.Right;
		 this.lblTitle.AutoSize = true;
		 this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
		 this.lblTitle.ForeColor = System.Drawing.Color.White;
		 this.lblTitle.Location = new System.Drawing.Point(770, 15);
		 this.lblTitle.Text = "مخاطبین";

		 // pnlToolbar
		 this.pnlToolbar.BackColor = System.Drawing.Color.FromArgb(236, 240, 241);
		 this.pnlToolbar.Controls.Add(this.pnlButtons);
		 this.pnlToolbar.Controls.Add(this.pnlSearchBox);
		 this.pnlToolbar.Dock = System.Windows.Forms.DockStyle.Top;
		 this.pnlToolbar.Padding = new System.Windows.Forms.Padding(10);
		 this.pnlToolbar.Size = new System.Drawing.Size(1000, 70);

		 // pnlButtons
		 this.pnlButtons.Controls.Add(this.btnRefresh);
		 this.pnlButtons.Controls.Add(this.btnDelete);
		 this.pnlButtons.Controls.Add(this.btnEdit);
		 this.pnlButtons.Controls.Add(this.btnAdd);
		 this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Left;
		 this.pnlButtons.Size = new System.Drawing.Size(420, 50);

		 // btnAdd
		 this.btnAdd.BackColor = System.Drawing.Color.FromArgb(46, 204, 113);
		 this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		 this.btnAdd.FlatAppearance.BorderSize = 0;
		 this.btnAdd.ForeColor = System.Drawing.Color.White;
		 this.btnAdd.Text = "افزودن";
		 this.btnAdd.Size = new System.Drawing.Size(100, 35);
		 this.btnAdd.Location = new System.Drawing.Point(315, 8);

		 // btnEdit
		 this.btnEdit.BackColor = System.Drawing.Color.FromArgb(52, 152, 219);
		 this.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		 this.btnEdit.FlatAppearance.BorderSize = 0;
		 this.btnEdit.ForeColor = System.Drawing.Color.White;
		 this.btnEdit.Text = "ویرایش";
		 this.btnEdit.Size = new System.Drawing.Size(100, 35);
		 this.btnEdit.Location = new System.Drawing.Point(210, 8);

		 // btnDelete
		 this.btnDelete.BackColor = System.Drawing.Color.FromArgb(231, 76, 60);
		 this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		 this.btnDelete.FlatAppearance.BorderSize = 0;
		 this.btnDelete.ForeColor = System.Drawing.Color.White;
		 this.btnDelete.Text = "حذف";
		 this.btnDelete.Size = new System.Drawing.Size(100, 35);
		 this.btnDelete.Location = new System.Drawing.Point(105, 8);

		 // btnRefresh
		 this.btnRefresh.BackColor = System.Drawing.Color.FromArgb(155, 89, 182);
		 this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		 this.btnRefresh.FlatAppearance.BorderSize = 0;
		 this.btnRefresh.ForeColor = System.Drawing.Color.White;
		 this.btnRefresh.Text = "بروزرسانی";
		 this.btnRefresh.Size = new System.Drawing.Size(100, 35);
		 this.btnRefresh.Location = new System.Drawing.Point(0, 8);

		 // pnlSearchBox
		 this.pnlSearchBox.Controls.Add(this.btnClearSearch);
		 this.pnlSearchBox.Controls.Add(this.btnSearch);
		 this.pnlSearchBox.Controls.Add(this.txtSearch);
		 this.pnlSearchBox.Dock = System.Windows.Forms.DockStyle.Right;
		 this.pnlSearchBox.Size = new System.Drawing.Size(350, 50);

		 // txtSearch
		 this.txtSearch.Font = new System.Drawing.Font("Segoe UI", 11F);
		 this.txtSearch.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
		 this.txtSearch.Size = new System.Drawing.Size(250, 27);
		 this.txtSearch.Location = new System.Drawing.Point(90, 10);

		 // btnSearch
		 this.btnSearch.Size = new System.Drawing.Size(40, 29);
		 this.btnSearch.Location = new System.Drawing.Point(45, 9);
		 this.btnSearch.Text = "🔍";

		 // btnClearSearch
		 this.btnClearSearch.Size = new System.Drawing.Size(35, 29);
		 this.btnClearSearch.Location = new System.Drawing.Point(5, 9);
		 this.btnClearSearch.Text = "✕";

		 // dgvContacts
		 this.dgvContacts.Dock = System.Windows.Forms.DockStyle.Fill;
		 this.dgvContacts.ReadOnly = true;
		 this.dgvContacts.AllowUserToAddRows = false;
		 this.dgvContacts.AllowUserToDeleteRows = false;
		 this.dgvContacts.RowHeadersVisible = false;
		 this.dgvContacts.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
		 this.dgvContacts.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
		 this.dgvContacts.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
				this.colId,
				this.colName,
				this.colEmail,
				this.colPhoneNumber,
				this.colChatwootId,
				this.colCreatedAt
			});

		 // Columns
		 this.colId.HeaderText = "ID";
		 this.colId.DataPropertyName = "Id";

		 this.colName.HeaderText = "نام";
		 this.colName.DataPropertyName = "Name";

		 this.colEmail.HeaderText = "ایمیل";
		 this.colEmail.DataPropertyName = "Email";

		 this.colPhoneNumber.HeaderText = "تلفن";
		 this.colPhoneNumber.DataPropertyName = "PhoneNumber";

		 this.colChatwootId.HeaderText = "Chatwoot ID";
		 this.colChatwootId.DataPropertyName = "ChatwootContactId";

		 this.colCreatedAt.HeaderText = "تاریخ ایجاد";
		 this.colCreatedAt.DataPropertyName = "CreatedAt";

		 // pnlStatusBar
		 this.pnlStatusBar.BackColor = System.Drawing.Color.FromArgb(52, 73, 94);
		 this.pnlStatusBar.Dock = System.Windows.Forms.DockStyle.Bottom;
		 this.pnlStatusBar.Size = new System.Drawing.Size(1000, 30);
		 this.pnlStatusBar.Controls.Add(this.lblStatus);
		 this.pnlStatusBar.Controls.Add(this.lblCount);

		 // lblStatus
		 this.lblStatus.ForeColor = System.Drawing.Color.White;
		 this.lblStatus.Location = new System.Drawing.Point(10, 6);
		 this.lblStatus.Text = "آماده";

		 // lblCount
		 this.lblCount.Anchor = System.Windows.Forms.AnchorStyles.Right;
		 this.lblCount.ForeColor = System.Drawing.Color.White;
		 this.lblCount.Location = new System.Drawing.Point(850, 6);
		 this.lblCount.Text = "0";

		 // ContactListForm
		 this.Controls.Add(this.dgvContacts);
		 this.Controls.Add(this.pnlStatusBar);
		 this.Controls.Add(this.pnlToolbar);
		 this.Controls.Add(this.pnlHeader);
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
   }
}
