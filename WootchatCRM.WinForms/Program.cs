using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WootchatCRM.Core.Interfaces;
using WootchatCRM.Core.Interfaces.Services;
using WootchatCRM.Forms.Contacts;
using WootchatCRM.Infrastructure.Chatwoot;
using WootchatCRM.Infrastructure.Data;
using WootchatCRM.Infrastructure.Repositories;
using WootchatCRM.Infrastructure.Services;
using WootchatCRM.Windows.Forms;

namespace WootchatCRM.WinForms;

static class Program
{
   [STAThread]
   static void Main()
   {
      ApplicationConfiguration.Initialize();

      // ═══════════════════════════════════════════════
      // Setup DI Container
      // ═══════════════════════════════════════════════
      var services = new ServiceCollection();
      ConfigureServices(services);

      var serviceProvider = services.BuildServiceProvider();

      // ═══════════════════════════════════════════════
      // Ensure Database Created (برای توسعه)
      // ═══════════════════════════════════════════════
      using (var scope = serviceProvider.CreateScope())
      {
         var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
         dbContext.Database.EnsureCreated();
      }

      // ═══════════════════════════════════════════════
      // Run Application
      // ═══════════════════════════════════════════════
      Application.Run(serviceProvider.GetRequiredService<MainForm>());
   }

   private static void ConfigureServices(IServiceCollection services)
   {
      // ───────────────────────────────────────────────
      // Database
      // ───────────────────────────────────────────────
      services.AddDbContext<AppDbContext>(options =>
          options.UseSqlite("Data Source=WootchatCRM.db"));

      // ───────────────────────────────────────────────
      // Repositories
      // ───────────────────────────────────────────────
      services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

      // ───────────────────────────────────────────────
      // Services
      // ───────────────────────────────────────────────
      services.AddScoped<IContactService, ContactService>();
      services.AddScoped<ITagService, TagService>();
      services.AddScoped<IConversationService, ConversationService>();
      services.AddScoped<IMessageService, MessageService>();
      services.AddScoped<ICampaignService, CampaignService>();
      services.AddScoped<IUserService, UserService>();
      services.AddScoped<ISettingsService, SettingsService>();

      // ───────────────────────────────────────────────
      // Chatwoot API Client
      // ───────────────────────────────────────────────
      // نکته: ChatwootApiClient خودش تنظیمات را Lazy می‌خواند
      // بنابراین نیازی به پیکربندی در Startup نیست
      services.AddHttpClient<IChatwootApiClient, ChatwootApiClient>();

      // ───────────────────────────────────────────────
      // Forms
      // ───────────────────────────────────────────────
      services.AddScoped<MainForm>();
      services.AddScoped<ContactListForm>();
      services.AddScoped<ContactDetailForm>();
   }
}
