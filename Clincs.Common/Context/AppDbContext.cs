using Clincs.Common.Models.Database;
using Clincs.Common.Models.Database.API;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Clincs.Common.Context
{
    public class AppDbContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(x => new { x.Id });
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<ConversationReferenceEntity> conversationReferenceEntities { get; set; }
        public DbSet<NotificationEntity> notificationEntities { get; set; }

    }
}
