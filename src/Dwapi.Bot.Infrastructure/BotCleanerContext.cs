using Dwapi.Bot.Core.Domain.Catalogs;
using Dwapi.Bot.Infrastructure.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Dwapi.Bot.Infrastructure
{
    public class BotCleanerContext : DbContext
    {
        public DbSet<Site> Sites { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<SubjectExtract> Extracts { get; set; }

        public BotCleanerContext(DbContextOptions<BotCleanerContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new SiteConfiguration());
            modelBuilder.ApplyConfiguration(new SubjectConfiguration());
            modelBuilder.ApplyConfiguration(new SubjectExtractConfiguration());
        }
    }
}
