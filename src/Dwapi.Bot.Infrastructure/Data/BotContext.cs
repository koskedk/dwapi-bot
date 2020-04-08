using Dwapi.Bot.Core.Domain.Indices;
using Microsoft.EntityFrameworkCore;

namespace Dwapi.Bot.Infrastructure.Data
{
    public class BotContext:DbContext
    {
        public DbSet<PatientIndex> PatientIndices { get; set; }
        public DbSet<PatientIndexSiteScore> SiteScores { get; set; }
        public DbSet<PatientIndexInterSiteScore> InterSiteScores { get; set; }
        public BotContext(DbContextOptions<BotContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new PatientIndexConfiguration());
            modelBuilder.ApplyConfiguration(new PatientIndexSiteScoreConfiguration());
            modelBuilder.ApplyConfiguration(new PatientIndexInterSiteScoreConfiguration());
        }
    }
}
