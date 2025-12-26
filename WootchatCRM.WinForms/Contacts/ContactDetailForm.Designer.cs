namespace WootchatCRM.Windows.Forms;

partial class ContactDetailForm
{
   private System.ComponentModel.IContainer components = null;

   private TextBox txtName;
   private TextBox txtPhone;
   private TextBox txtEmail;
   private TextBox txtCompany;
   private TextBox txtCustomFields;
   private TextBox txtNewTag;

   private CheckedListBox clbTags;

   private Button btnSave;
   private Button btnCancel;
   private Button btnAddTag;

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
	  txtName = new TextBox();
	  txtPhone = new TextBox();
	  txtEmail = new TextBox();
	  txtCompany = new TextBox();
	  txtCustomFields = new TextBox();
	  txtNewTag = new TextBox();

	  clbTags = new CheckedListBox();

	  btnSave = new Button();
	  btnCancel = new Button();
	  btnAddTag = new Button();

	  SuspendLayout();

	  // ───── Form ─────
	  RightToLeft = RightToLeft.Yes;
	  RightToLeftLayout = true;
	  ClientSize = new Size(600, 520);
	  Text = "جزئیات مخاطب";

	  // ───── txtName ─────
	  txtName.Location = new Point(30, 30);
	  txtName.Size = new Size(250, 23);
	  txtName.PlaceholderText = "نام مخاطب";

	  // ───── txtPhone ─────
	  txtPhone.Location = new Point(30, 70);
	  txtPhone.Size = new Size(250, 23);
	  txtPhone.PlaceholderText = "شماره تلفن";

	  // ───── txtEmail ─────
	  txtEmail.Location = new Point(30, 110);
	  txtEmail.Size = new Size(250, 23);
	  txtEmail.PlaceholderText = "ایمیل";

	  // ───── txtCompany ─────
	  txtCompany.Location = new Point(30, 150);
	  txtCompany.Size = new Size(250, 23);
	  txtCompany.PlaceholderText = "شرکت";

	  // ───── txtCustomFields ─────
	  txtCustomFields.Location = new Point(30, 190);
	  txtCustomFields.Size = new Size(250, 60);
	  txtCustomFields.Multiline = true;
	  txtCustomFields.PlaceholderText = "فیلدهای سفارشی (JSON)";

	  // ───── clbTags ─────
	  clbTags.Location = new Point(320, 30);
	  clbTags.Size = new Size(240, 200);

	  // ───── txtNewTag ─────
	  txtNewTag.Location = new Point(320, 245);
	  txtNewTag.Size = new Size(160, 23);
	  txtNewTag.PlaceholderText = "تگ جدید";

	  // ───── btnAddTag ─────
	  btnAddTag.Location = new Point(490, 245);
	  btnAddTag.Size = new Size(70, 23);
	  btnAddTag.Text = "➕";
	  btnAddTag.Click += btnAddTag_Click;

	  // ───── btnSave ─────
	  btnSave.Location = new Point(380, 460);
	  btnSave.Size = new Size(90, 30);
	  btnSave.Text = "💾 ذخیره";
	  btnSave.Click += btnSave_Click;

	  // ───── btnCancel ─────
	  btnCancel.Location = new Point(480, 460);
	  btnCancel.Size = new Size(80, 30);
	  btnCancel.Text = "انصراف";
	  btnCancel.Click += btnCancel_Click;

	  // ───── Add Controls ─────
	  Controls.Add(txtName);
	  Controls.Add(txtPhone);
	  Controls.Add(txtEmail);
	  Controls.Add(txtCompany);
	  Controls.Add(txtCustomFields);
	  Controls.Add(clbTags);
	  Controls.Add(txtNewTag);
	  Controls.Add(btnAddTag);
	  Controls.Add(btnSave);
	  Controls.Add(btnCancel);

	  ResumeLayout(false);
	  PerformLayout();
   }
}
