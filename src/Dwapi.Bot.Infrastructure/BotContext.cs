using Dwapi.Bot.Core.Domain.Indices;
using Dwapi.Bot.Infrastructure.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Dwapi.Bot.Infrastructure
{
    public class BotContext:DbContext
    {
        public DbSet<SubjectIndex> SubjectIndices { get; set; }
        public DbSet<SubjectIndexScore> SubjectIndexScores { get; set; }
        public DbSet<SubjectIndexStage> SubjectIndexStages { get; set; }
        public BotContext(DbContextOptions<BotContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new SubjectIndexConfiguration());
            modelBuilder.ApplyConfiguration(new SubjectIndexScoreConfiguration());
            modelBuilder.ApplyConfiguration(new SubjectIndexStageConfiguration());
        }
    }
}
