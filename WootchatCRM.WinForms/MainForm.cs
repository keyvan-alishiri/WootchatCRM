using Microsoft.Extensions.DependencyInjection;
using WootchatCRM.Forms.Contacts;
using WootchatCRM.UI.Forms;

namespace WootchatCRM.WinForms;

public partial class MainForm : Form
{
   private readonly IServiceProvider _serviceProvider;

   public MainForm(IServiceProvider serviceProvider)
   {
      InitializeComponent();
      _serviceProvider = serviceProvider;
   }

   private void MainForm_Load(object sender, EventArgs e)
   {
      LoadDashboard();
   }

   // ═══════════════════════════════════════════════
   // Navigation helpers
   // ═══════════════════════════════════════════════
   private void SetActiveButton(Button activeButton)
   {
      foreach (Control control in pnlSidebar.Controls)
      {
         if (control is Button btn)
            btn.BackColor = Color.FromArgb(45, 55, 72);
      }

      activeButton.BackColor = Color.FromArgb(74, 85, 104);
   }

   private void LoadContent(Control control, string title)
   {
      pnlContent.Controls.Clear();
      control.Dock = DockStyle.Fill;
      pnlContent.Controls.Add(control);
      lblPageTitle.Text = title;
   }

   // ═══════════════════════════════════════════════
   // Dashboard
   // ═══════════════════════════════════════════════
   private void LoadDashboard()
   {
      SetActiveButton(btnDashboard);

      var dashboard = new Label
      {
         Dock = DockStyle.Fill,
         Text = "📊 داشبورد\n\n(در مرحله بعد UserControl واقعی اینجا می‌آید)",
         Font = new Font("Segoe UI", 14F),
         TextAlign = ContentAlignment.MiddleCenter
      };

      LoadContent(dashboard, "📊 داشبورد");
   }

   // ═══════════════════════════════════════════════
   // Menu Clicks
   // ═══════════════════════════════════════════════
   private void btnDashboard_Click(object sender, EventArgs e)
       => LoadDashboard();

   private void btnContacts_Click(object sender, EventArgs e)
   {
      SetActiveButton(btnContacts);

      // ✅ لود ContactListForm از DI Container
      var contactListForm = _serviceProvider.GetRequiredService<ContactListForm>();
      LoadContent(contactListForm, "👥 مخاطبین");
   }

   // ═══════════════════════════════════════════════
   // ✅ اصلاح‌شده: لود ConversationsForm از DI Container
   // ═══════════════════════════════════════════════
   private void btnConversations_Click(object sender, EventArgs e)
   {
      SetActiveButton(btnConversations);

      var conversationsForm = _serviceProvider.GetRequiredService<ConversationsForm>();
      LoadContent(conversationsForm, "💬 مکالمات");
   }

   private void btnCampaigns_Click(object sender, EventArgs e)
   {
      SetActiveButton(btnCampaigns);
      LoadContent(new Label { Text = "📢 فرم کمپین‌ها", Dock = DockStyle.Fill }, "📢 کمپین‌ها");
   }

   private void btnUsers_Click(object sender, EventArgs e)
   {
      SetActiveButton(btnUsers);
      LoadContent(new Label { Text = "👤 فرم کاربران", Dock = DockStyle.Fill }, "👤 کاربران");
   }

   private void btnTags_Click(object sender, EventArgs e)
   {
      SetActiveButton(btnTags);
      LoadContent(new Label { Text = "🏷️ فرم تگ‌ها", Dock = DockStyle.Fill }, "🏷️ تگ‌ها");
   }

   private void btnSettings_Click(object sender, EventArgs e)
   {
      using var settingsForm = _serviceProvider.GetRequiredService<SettingsForm>();
      settingsForm.ShowDialog(this);
   }
}
