using Microsoft.EntityFrameworkCore;
using WootchatCRM.Core.Entities;

namespace WootchatCRM.Infrastructure.Data;

public class AppDbContext : DbContext
{
   public AppDbContext(DbContextOptions<AppDbContext> options)
       : base(options)
   {
   }

   // ══════════════════════════════════════════════════════════
   // DbSets
   // ══════════════════════════════════════════════════════════

   public DbSet<Channel> Channels => Set<Channel>();
   public DbSet<Inbox> Inboxes => Set<Inbox>();
   public DbSet<Contact> Contacts => Set<Contact>();
   public DbSet<User> Users => Set<User>();
   public DbSet<Team> Teams => Set<Team>();
   public DbSet<Conversation> Conversations => Set<Conversation>();
   public DbSet<Message> Messages => Set<Message>();
   public DbSet<Attachment> Attachments => Set<Attachment>();
   public DbSet<Note> Notes => Set<Note>();
   public DbSet<CrmTask> CrmTasks => Set<CrmTask>();
   public DbSet<Tag> Tags => Set<Tag>();
   public DbSet<ContactTag> ContactTags => Set<ContactTag>();
   public DbSet<ConversationTag> ConversationTags => Set<ConversationTag>();
   public DbSet<Campaign> Campaigns => Set<Campaign>();
   public DbSet<CampaignRecipient> CampaignRecipients => Set<CampaignRecipient>();
   public DbSet<Settings> Settings => Set<Settings>();

   // ══════════════════════════════════════════════════════════
   // Fluent API Configuration
   // ══════════════════════════════════════════════════════════

   protected override void OnModelCreating(ModelBuilder modelBuilder)
   {
      base.OnModelCreating(modelBuilder);

      // ─────────────── BaseEntity Default Values ───────────────
      foreach (var entityType in modelBuilder.Model.GetEntityTypes())
      {
         if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
         {
            modelBuilder.Entity(entityType.ClrType)
                .Property(nameof(BaseEntity.CreatedAt))
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
         }
      }

      // ─────────────── Channel ───────────────
      modelBuilder.Entity<Channel>(entity =>
      {
         entity.Property(x => x.Name)
               .IsRequired()
               .HasMaxLength(100);

         entity.Property(x => x.Configuration)
               .HasColumnType("TEXT");
      });

      // ─────────────── Inbox ───────────────
      modelBuilder.Entity<Inbox>(entity =>
      {
         entity.Property(x => x.Name)
               .IsRequired()
               .HasMaxLength(100);

         entity.HasIndex(x => x.ChatwootInboxId);
      });

      // ─────────────── Contact ───────────────
      modelBuilder.Entity<Contact>(entity =>
      {
         entity.Property(x => x.Name)
               .IsRequired()
               .HasMaxLength(150);

         entity.Property(x => x.CustomFields)
               .HasColumnType("TEXT");

         entity.HasIndex(x => x.PhoneNumber);
         entity.HasIndex(x => x.Email);
         entity.HasIndex(x => x.ChatwootContactId);
      });

      // ─────────────── User ───────────────
      modelBuilder.Entity<User>(entity =>
      {
         entity.Property(x => x.Name)
               .IsRequired()
               .HasMaxLength(100);

         entity.HasIndex(x => x.ChatwootUserId);

         // User belongs to Team
         entity.HasOne(x => x.Team)
               .WithMany(x => x.Members)
               .HasForeignKey(x => x.TeamId)
               .OnDelete(DeleteBehavior.SetNull);
      });

      // ─────────────── Team ───────────────
      modelBuilder.Entity<Team>(entity =>
      {
         entity.Property(x => x.Name)
               .IsRequired()
               .HasMaxLength(100);
      });

      // ─────────────── Conversation ───────────────
      modelBuilder.Entity<Conversation>(entity =>
      {
         entity.HasIndex(x => x.ChatwootConversationId);
         entity.HasIndex(x => x.Status);

         // Conversation -> Contact
         entity.HasOne(x => x.Contact)
               .WithMany(x => x.Conversations)
               .HasForeignKey(x => x.ContactId)
               .OnDelete(DeleteBehavior.Restrict);

         // Conversation -> Inbox
         entity.HasOne(x => x.Inbox)
               .WithMany(x => x.Conversations)
               .HasForeignKey(x => x.InboxId)
               .OnDelete(DeleteBehavior.Restrict);

         // Conversation -> Assignee (User)
         entity.HasOne(x => x.Assignee)
               .WithMany(x => x.AssignedConversations)
               .HasForeignKey(x => x.AssigneeId)
               .OnDelete(DeleteBehavior.SetNull);

         // Conversation -> Team
         entity.HasOne(x => x.Team)
               .WithMany(x => x.Conversations)
               .HasForeignKey(x => x.TeamId)
               .OnDelete(DeleteBehavior.SetNull);
      });

      // ─────────────── Message ───────────────
      modelBuilder.Entity<Message>(entity =>
      {
         entity.Property(x => x.Content)
               .IsRequired();

         entity.HasIndex(x => x.ChatwootMessageId);

         // Message -> Conversation
         entity.HasOne(x => x.Conversation)
               .WithMany(x => x.Messages)
               .HasForeignKey(x => x.ConversationId)
               .OnDelete(DeleteBehavior.Cascade);

         // Message -> Sender (User)
         entity.HasOne(x => x.Sender)
               .WithMany(x => x.Messages)
               .HasForeignKey(x => x.SenderId)
               .OnDelete(DeleteBehavior.SetNull);

         // Message -> ReplyToMessage (Self-Reference)
         entity.HasOne(x => x.ReplyToMessage)
               .WithMany()
               .HasForeignKey(x => x.ReplyToMessageId)
               .OnDelete(DeleteBehavior.SetNull);
      });

      // ─────────────── Attachment ───────────────
      modelBuilder.Entity<Attachment>(entity =>
      {
         entity.Property(x => x.FileName)
               .IsRequired()
               .HasMaxLength(255);

         entity.Property(x => x.FileType)
               .HasMaxLength(50);

         entity.Property(x => x.Url)
               .IsRequired()
               .HasMaxLength(500);

         entity.HasOne(x => x.Message)
               .WithMany(x => x.Attachments)
               .HasForeignKey(x => x.MessageId)
               .OnDelete(DeleteBehavior.Cascade);
      });

      // ─────────────── Note ───────────────
      modelBuilder.Entity<Note>(entity =>
      {
         entity.Property(x => x.Content)
               .IsRequired();

         entity.HasOne(x => x.Contact)
               .WithMany(x => x.Notes)
               .HasForeignKey(x => x.ContactId)
               .OnDelete(DeleteBehavior.Cascade);

         entity.HasOne(x => x.Conversation)
               .WithMany(x => x.Notes)
               .HasForeignKey(x => x.ConversationId)
               .OnDelete(DeleteBehavior.Cascade);
      });

      // ─────────────── CrmTask ───────────────
      modelBuilder.Entity<CrmTask>(entity =>
      {
         entity.Property(x => x.Title)
               .IsRequired()
               .HasMaxLength(200);

         entity.HasIndex(x => x.Status);
         entity.HasIndex(x => x.DueDate);

         // Task -> AssignedTo (User)
         entity.HasOne(x => x.AssignedTo)
               .WithMany(x => x.AssignedTasks)
               .HasForeignKey(x => x.AssignedToId)
               .OnDelete(DeleteBehavior.SetNull);

         // Task -> Contact
         entity.HasOne(x => x.Contact)
               .WithMany(x => x.CrmTasks)
               .HasForeignKey(x => x.ContactId)
               .OnDelete(DeleteBehavior.SetNull);

         // Task -> Conversation
         entity.HasOne(x => x.Conversation)
               .WithMany(x => x.CrmTasks)
               .HasForeignKey(x => x.ConversationId)
               .OnDelete(DeleteBehavior.SetNull);
      });

      // ─────────────── Tag ───────────────
      modelBuilder.Entity<Tag>(entity =>
      {
         entity.Property(x => x.Name)
               .IsRequired()
               .HasMaxLength(100);

         entity.HasIndex(x => x.Name)
               .IsUnique();
      });

      // ─────────────── ContactTag (M2M) ───────────────
      modelBuilder.Entity<ContactTag>(entity =>
      {
         entity.HasKey(x => new { x.ContactId, x.TagId });

         entity.HasOne(x => x.Contact)
               .WithMany(x => x.ContactTags)
               .HasForeignKey(x => x.ContactId);

         entity.HasOne(x => x.Tag)
               .WithMany(x => x.ContactTags)
               .HasForeignKey(x => x.TagId);
      });

      // ─────────────── ConversationTag (M2M) ───────────────
      modelBuilder.Entity<ConversationTag>(entity =>
      {
         entity.HasKey(x => new { x.ConversationId, x.TagId });

         entity.HasOne(x => x.Conversation)
               .WithMany(x => x.ConversationTags)
               .HasForeignKey(x => x.ConversationId);

         entity.HasOne(x => x.Tag)
               .WithMany(x => x.ConversationTags)
               .HasForeignKey(x => x.TagId);
      });

      // ─────────────── Campaign ───────────────
      modelBuilder.Entity<Campaign>(entity =>
      {
         entity.Property(x => x.Name)
               .IsRequired()
               .HasMaxLength(150);

         entity.HasIndex(x => x.Status);
      });

      // ─────────────── CampaignRecipient ───────────────
      modelBuilder.Entity<CampaignRecipient>(entity =>
      {
         entity.HasKey(x => new { x.CampaignId, x.ContactId });

         entity.HasOne(x => x.Campaign)
               .WithMany(x => x.Recipients)
               .HasForeignKey(x => x.CampaignId);

         entity.HasOne(x => x.Contact)
               .WithMany()
               .HasForeignKey(x => x.ContactId);
      });

      // ─────────────── Settings ───────────────
      modelBuilder.Entity<Settings>(entity =>
      {
         entity.HasIndex(x => x.Key)
               .IsUnique();

         entity.Property(x => x.Key)
               .IsRequired()
               .HasMaxLength(100);

         entity.Property(x => x.Value)
               .HasColumnType("TEXT");
      });
   }
}
