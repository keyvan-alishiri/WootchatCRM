using WootchatCRM.Core.Entities;
using WootchatCRM.Core.Interfaces.Services;

namespace WootchatCRM.Windows.Forms;

public partial class ContactDetailForm : Form
{
   private readonly IContactService _contactService;
   private readonly ITagService _tagService;
   private readonly int? _contactId;
   private Contact? _contact;
   private List<Tag> _allTags = new();

   // ───────────────── Constructor: Add Mode ─────────────────
   public ContactDetailForm(IContactService contactService, ITagService tagService)
   {
      InitializeComponent();
      _contactService = contactService;
      _tagService = tagService;
      _contactId = null;
      this.Text = "افزودن مخاطب جدید";
   }

   // ───────────────── Constructor: Edit Mode ─────────────────
   public ContactDetailForm(IContactService contactService, ITagService tagService, int contactId)
       : this(contactService, tagService)
   {
      _contactId = contactId;
      this.Text = "ویرایش مخاطب";
   }

   // ───────────────── Load ─────────────────
   protected override async void OnLoad(EventArgs e)
   {
      base.OnLoad(e);
      await LoadTagsAsync();

      if (_contactId.HasValue)
      {
         await LoadContactAsync();
      }
   }

   private async Task LoadTagsAsync()
   {
      try
      {
         _allTags = (await _tagService.GetAllAsync()).ToList();
         clbTags.Items.Clear();

         foreach (var tag in _allTags)
         {
            clbTags.Items.Add(tag.Name);
         }
      }
      catch (Exception ex)
      {
         MessageBox.Show($"خطا در بارگذاری تگ‌ها:\n{ex.Message}", "خطا",
             MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
   }

   private async Task LoadContactAsync()
   {
      try
      {
         _contact = await _contactService.GetByIdAsync(_contactId!.Value);

         if (_contact == null)
         {
            MessageBox.Show("مخاطب یافت نشد!", "خطا",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            this.Close();
            return;
         }

         // Fill form fields
         txtName.Text = _contact.Name;
         txtPhone.Text = _contact.PhoneNumber;
         txtEmail.Text = _contact.Email;
         txtCompany.Text = _contact.Company;

         // Check contact's tags
         var contactTags = await _contactService.GetContactTagsAsync(_contactId!.Value);
         var contactTagNames = contactTags.Select(t => t.Name).ToHashSet();

         for (int i = 0; i < clbTags.Items.Count; i++)
         {
            if (contactTagNames.Contains(clbTags.Items[i].ToString()))
            {
               clbTags.SetItemChecked(i, true);
            }
         }
      }
      catch (Exception ex)
      {
         MessageBox.Show($"خطا در بارگذاری مخاطب:\n{ex.Message}", "خطا",
             MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
   }

   // ───────────────── Save ─────────────────
   private async void btnSave_Click(object sender, EventArgs e)
   {
      // Validation
      if (string.IsNullOrWhiteSpace(txtName.Text))
      {
         MessageBox.Show("نام مخاطب الزامی است!", "خطا",
             MessageBoxButtons.OK, MessageBoxIcon.Warning);
         txtName.Focus();
         return;
      }

      try
      {
         btnSave.Enabled = false;

         if (_contactId.HasValue)
         {
            await UpdateContactAsync();
         }
         else
         {
            await CreateContactAsync();
         }

         this.DialogResult = DialogResult.OK;
         this.Close();
      }
      catch (Exception ex)
      {
         MessageBox.Show($"خطا در ذخیره‌سازی:\n{ex.Message}", "خطا",
             MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
      finally
      {
         btnSave.Enabled = true;
      }
   }

   private async Task CreateContactAsync()
   {
      var contact = new Contact
      {
         Name = txtName.Text.Trim(),
         PhoneNumber = txtPhone.Text.Trim(),
         Email = txtEmail.Text.Trim(),
         Company = txtCompany.Text.Trim()
      };

      var createdContact = await _contactService.CreateAsync(contact);

      // Add selected tags
      await UpdateContactTagsAsync(createdContact.Id);

      MessageBox.Show("مخاطب با موفقیت ایجاد شد.", "موفق",
          MessageBoxButtons.OK, MessageBoxIcon.Information);
   }

   private async Task UpdateContactAsync()
   {
      _contact!.Name = txtName.Text.Trim();
      _contact.PhoneNumber = txtPhone.Text.Trim();
      _contact.Email = txtEmail.Text.Trim();
      _contact.Company = txtCompany.Text.Trim();

      await _contactService.UpdateAsync(_contact);

      // Update tags
      await UpdateContactTagsAsync(_contactId!.Value);

      MessageBox.Show("مخاطب با موفقیت به‌روزرسانی شد.", "موفق",
          MessageBoxButtons.OK, MessageBoxIcon.Information);
   }

   private async Task UpdateContactTagsAsync(int contactId)
   {
      // Get current tags
      var currentTags = (await _contactService.GetContactTagsAsync(contactId)).ToList();
      var currentTagIds = currentTags.Select(t => t.Id).ToHashSet();

      // Get selected tags from CheckedListBox
      var selectedTagIds = new HashSet<int>();
      for (int i = 0; i < clbTags.Items.Count; i++)
      {
         if (clbTags.GetItemChecked(i))
         {
            var tagName = clbTags.Items[i].ToString();
            var tag = _allTags.FirstOrDefault(t => t.Name == tagName);
            if (tag != null)
            {
               selectedTagIds.Add(tag.Id);
            }
         }
      }

      // Remove unchecked tags
      foreach (var tagId in currentTagIds.Except(selectedTagIds))
      {
         await _contactService.RemoveTagAsync(contactId, tagId);
      }

      // Add new tags
      foreach (var tagId in selectedTagIds.Except(currentTagIds))
      {
         await _contactService.AddTagAsync(contactId, tagId);
      }
   }

   // ───────────────── Add New Tag (Inline) ─────────────────
   private async void btnAddTag_Click(object sender, EventArgs e)
   {
      var tagName = txtNewTag.Text.Trim();

      if (string.IsNullOrWhiteSpace(tagName))
      {
         MessageBox.Show("نام تگ را وارد کنید!", "خطا",
             MessageBoxButtons.OK, MessageBoxIcon.Warning);
         return;
      }

      // Check duplicate
      if (_allTags.Any(t => t.Name.Equals(tagName, StringComparison.OrdinalIgnoreCase)))
      {
         MessageBox.Show("این تگ قبلاً وجود دارد!", "خطا",
             MessageBoxButtons.OK, MessageBoxIcon.Warning);
         return;
      }

      try
      {
         var newTag = new Tag { Name = tagName, Color = "#3498db" };
         await _tagService.CreateAsync(newTag);

         // Refresh tags list
         await LoadTagsAsync();

         // Check the new tag
         int index = clbTags.Items.IndexOf(tagName);
         if (index >= 0)
         {
            clbTags.SetItemChecked(index, true);
         }

         txtNewTag.Clear();
      }
      catch (Exception ex)
      {
         MessageBox.Show($"خطا در ایجاد تگ:\n{ex.Message}", "خطا",
             MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
   }

   // ───────────────── Cancel ─────────────────
   private void btnCancel_Click(object sender, EventArgs e)
   {
      this.DialogResult = DialogResult.Cancel;
      this.Close();
   }
}
