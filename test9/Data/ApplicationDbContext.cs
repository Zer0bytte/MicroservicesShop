using InstagramDMs.API.Models.Instagram;
using Microsoft.EntityFrameworkCore;

namespace InstagramDMs.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        private readonly byte[] _knownKey = { 58, 115, 66, 118, 108, 161, 4, 199, 248, 158, 13, 9, 166, 109, 122, 209 };
        private readonly byte[] _knownIV = { 109, 178, 123, 159, 123, 180, 108, 179, 222, 120, 181, 49, 122, 34, 210, 80 };


        public DbSet<InstagramConversation> InstagramConversations { get; set; }
        public DbSet<InstagramMessage> InstagramMessages { get; set; }
        public DbSet<InstagramConversationTimelineEvent> InstagramConversationTimelineEvents { get; set; }
        public DbSet<InstagramTeamMember> InstagramTeamMembers { get; set; }
        public DbSet<InstagramBusiness> InstagramBusinesses { get; set; }
        public DbSet<InstagramTeamMemberMessage> InstagramTeamMemberMessages { get; set; }
        public DbSet<InstagramContact> InstagramContacts { get; set; }
        public DbSet<InstagramBusinessMedia> InstagramBusinessMedia { get; set; }
        public DbSet<InstagramBusinessTemplate> InstagramBusinessTemplates { get; set; }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
            // Conversation Relationships
            modelBuilder.Entity<InstagramConversation>()
                .HasMany(c => c.Messages)
                .WithOne(m => m.Conversation)
                .HasForeignKey(m => m.ConversationId);

            // Timeline Event Relationships
            modelBuilder.Entity<InstagramConversationTimelineEvent>()
                .HasOne(e => e.Conversation)
                .WithMany(c => c.TimelineEvents)
                .HasForeignKey(e => e.ConversationId);

            // Message to Timeline Event
            modelBuilder.Entity<InstagramMessage>()
                .HasOne(m => m.TimelineEvent)
                .WithOne()
                .HasForeignKey<InstagramMessage>(m => m.TimelineEventId)
                .IsRequired(false);

            modelBuilder.Entity<InstagramConversationTimelineEvent>()
                .Property(e => e.EventType)
                .HasConversion(e => e.ToString(),
                e => (InstagramConversationTimeLineEeventType)Enum.Parse(typeof(InstagramConversationTimeLineEeventType), e)
                );

            modelBuilder.Entity<InstagramConversationTimelineEvent>()
               .Property(e => e.EventSubtype)
               .HasConversion(e => e.ToString(),
               e => (InstagramConversationTimeLineEeventSubType)Enum.Parse(typeof(InstagramConversationTimeLineEeventSubType), e)
               );



            modelBuilder.Entity<InstagramConversation>().Property(p => p.LastMessageType)
                .HasConversion(t => t.ToString(), t => (MessageType)Enum.Parse(typeof(MessageType),t));


            base.OnModelCreating(modelBuilder);
        }
    }
}
