using Dwapi.Bot.Core.Domain.Configs;
using Microsoft.EntityFrameworkCore;

namespace Dwapi.Bot.Infrastructure
{
    public class BotJobsContext:DbContext
    {
        public DbSet<Manifest> Manifests { get; set; }

        public BotJobsContext(DbContextOptions<BotJobsContext> options) : base(options)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Manifest>(entity =>
            {
                entity.HasKey(e => e.Id);
            });
        }
    }
}
