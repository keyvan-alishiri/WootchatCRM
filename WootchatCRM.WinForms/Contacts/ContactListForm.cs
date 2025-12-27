using WootchatCRM.Core.Entities;
using WootchatCRM.Core.Interfaces.Services;
using WootchatCRM.Windows.Forms;
using WootchatCRM.WinForms.Contacts;

namespace WootchatCRM.Forms.Contacts
{
   public partial class ContactListForm : UserControl
   {
      private readonly IContactService _contactService;
      private readonly ITagService _tagService;
      private readonly IServiceProvider _serviceProvider;
      private readonly Func<Contact, NewConversationForm> _conversationFormFactory;

      public ContactListForm(
     IContactService contactService,
     ITagService tagService,
     IServiceProvider serviceProvider,
     Func<Contact, NewConversationForm> conversationFormFactory)
      {
         InitializeComponent();

         _contactService = contactService;
         _tagService = tagService;
         _serviceProvider = serviceProvider;
         _conversationFormFactory = conversationFormFactory;

         // رویدادها
         this.Load += ContactListForm_Load;
         this.btnAdd.Click += btnAdd_Click;
         this.btnEdit.Click += btnEdit_Click;
         this.btnDelete.Click += btnDelete_Click;
         this.btnRefresh.Click += btnRefresh_Click;
         this.btnSearch.Click += btnSearch_Click;
         this.btnClearSearch.Click += btnClearSearch_Click;
         this.btnSendMessage.Click += btnSendMessage_Click;
         this.txtSearch.KeyDown += txtSearch_KeyDown;
         this.dgvContacts.CellDoubleClick += dgvContacts_CellDoubleClick;
      }


      #region Load & Refresh

      private async void ContactListForm_Load(object sender, EventArgs e)
      {
         await LoadContactsAsync();
      }

      private async Task LoadContactsAsync()
      {
         try
         {
            lblStatus.Text = "در حال بارگذاری...";
            btnRefresh.Enabled = false;

            var contacts = await _contactService.GetAllAsync();
            var list = contacts.ToList();

            dgvContacts.DataSource = null;
            dgvContacts.DataSource = list;

            lblCount.Text = $"تعداد: {list.Count}";
            lblStatus.Text = "آماده";
         }
         catch (Exception ex)
         {
            lblStatus.Text = "خطا";
            MessageBox.Show(
                $"خطا در بارگذاری مخاطبین:\n{ex.Message}",
                "خطا",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
         }
         finally
         {
            btnRefresh.Enabled = true;
         }
      }

      #endregion

      #region Search

      private async Task SearchContactsAsync(string keyword)
      {
         try
         {
            lblStatus.Text = "در حال جستجو...";

            var contacts = await _contactService.SearchAsync(keyword);
            var list = contacts.ToList();

            dgvContacts.DataSource = null;
            dgvContacts.DataSource = list;

            lblCount.Text = $"تعداد: {list.Count}";
            lblStatus.Text = $"نتایج جستجو: {keyword}";
         }
         catch (Exception ex)
         {
            lblStatus.Text = "خطا";
            MessageBox.Show(
                $"خطا در جستجو:\n{ex.Message}",
                "خطا",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
         }
      }

      #endregion

      #region CRUD Helpers

      private int? GetSelectedContactId()
      {
         if (dgvContacts.CurrentRow == null)
            return null;

         var value = dgvContacts.CurrentRow.Cells["colId"].Value;

         if (value == null)
            return null;

         return Convert.ToInt32(value);
      }


      private void OpenDetailForm(int? contactId)
      {
         ContactDetailForm form;

         if (contactId.HasValue)
         {
            // Edit Mode
            form = new ContactDetailForm(
                _contactService,
                _tagService,
                contactId.Value
            );
         }
         else
         {
            // Add Mode
            form = new ContactDetailForm(
                _contactService,
                _tagService
            );
         }

         if (form.ShowDialog() == DialogResult.OK)
         {
            // Refresh list after save
            _ = LoadContactsAsync();
         }
      }


      #endregion

      #region Event Handlers

      private async void btnRefresh_Click(object sender, EventArgs e)
      {
         txtSearch.Clear();
         await LoadContactsAsync();
      }

      private async void btnSearch_Click(object sender, EventArgs e)
      {
         var keyword = txtSearch.Text.Trim();

         if (string.IsNullOrEmpty(keyword))
         {
            await LoadContactsAsync();
         }
         else
         {
            await SearchContactsAsync(keyword);
         }
      }

      private async void btnClearSearch_Click(object sender, EventArgs e)
      {
         txtSearch.Clear();
         await LoadContactsAsync();
      }
      private async void btnSendMessage_Click(object sender, EventArgs e)
      {
         // Debug
         if (dgvContacts.CurrentRow == null)
         {
            MessageBox.Show("CurrentRow is NULL");
            return;
         }

         MessageBox.Show($"RowIndex: {dgvContacts.CurrentRow.Index}");

         if (!dgvContacts.Columns.Contains("colId"))
         {
            MessageBox.Show("Column 'colId' not found!");
            return;
         }

         var cell = dgvContacts.CurrentRow.Cells["colId"];
         MessageBox.Show($"Cell is null: {cell == null}");

         if (cell != null)
         {
            MessageBox.Show($"Cell Value: {cell.Value ?? "NULL"}");
         }

         // ادامه کد اصلی
         var id = GetSelectedContactId();
         if (id == null)
         {
            MessageBox.Show(
                "لطفاً یک مخاطب انتخاب کنید.",
                "هشدار",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
         }

         await OpenNewConversationFormAsync(id.Value);
      }


      private async Task OpenNewConversationFormAsync(int contactId)
      {
         try
         {
            lblStatus.Text = "در حال بارگذاری...";

            var contact = await _contactService.GetByIdAsync(contactId);

            if (contact == null)
            {
               MessageBox.Show(
                   "مخاطب یافت نشد.",
                   "خطا",
                   MessageBoxButtons.OK,
                   MessageBoxIcon.Error);
               lblStatus.Text = "آماده";
               return;
            }

            if (string.IsNullOrWhiteSpace(contact.PhoneNumber))
            {
               MessageBox.Show(
                   "این مخاطب شماره تلفن ندارد.\nبرای ارسال پیام، ابتدا شماره تلفن را وارد کنید.",
                   "هشدار",
                   MessageBoxButtons.OK,
                   MessageBoxIcon.Warning);
               lblStatus.Text = "آماده";
               return;
            }

            lblStatus.Text = "آماده";

            using var form = _conversationFormFactory(contact);
            form.ShowDialog();
         }
         catch (Exception ex)
         {
            lblStatus.Text = "خطا";
            MessageBox.Show(
                $"خطا در باز کردن فرم ارسال پیام:\n{ex.Message}",
                "خطا",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
         }
      }

      private async void txtSearch_KeyDown(object sender, KeyEventArgs e)
      {
         if (e.KeyCode == Keys.Enter)
         {
            e.SuppressKeyPress = true;
            var keyword = txtSearch.Text.Trim();

            if (string.IsNullOrEmpty(keyword))
            {
               await LoadContactsAsync();
            }
            else
            {
               await SearchContactsAsync(keyword);
            }
         }
      }

      private void btnAdd_Click(object sender, EventArgs e)
      {
         OpenDetailForm(null);
      }

      private void btnEdit_Click(object sender, EventArgs e)
      {
         var id = GetSelectedContactId();
         if (id == null)
         {
            MessageBox.Show(
                "لطفاً یک مخاطب انتخاب کنید.",
                "هشدار",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
         }

         OpenDetailForm(id);
      }

      private async void btnDelete_Click(object sender, EventArgs e)
      {
         var id = GetSelectedContactId();
         if (id == null)
         {
            MessageBox.Show(
                "لطفاً یک مخاطب انتخاب کنید.",
                "هشدار",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
         }

         var confirm = MessageBox.Show(
             "آیا از حذف این مخاطب مطمئن هستید؟",
             "تأیید حذف",
             MessageBoxButtons.YesNo,
             MessageBoxIcon.Question);

         if (confirm != DialogResult.Yes)
            return;

         try
         {
            lblStatus.Text = "در حال حذف...";

            await _contactService.DeleteAsync(id.Value);
            await LoadContactsAsync();

            lblStatus.Text = "مخاطب حذف شد";
         }
         catch (Exception ex)
         {
            lblStatus.Text = "خطا در حذف";
            MessageBox.Show(
                $"خطا در حذف مخاطب:\n{ex.Message}",
                "خطا",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
         }
      }

      private void dgvContacts_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
      {
         if (e.RowIndex < 0)
            return;

         var id = GetSelectedContactId();
         if (id != null)
         {
            OpenDetailForm(id);
         }
      }

      #endregion
   }
}
