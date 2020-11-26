using Dwapi.Bot.Core.Domain.Catalogs;
using Microsoft.EntityFrameworkCore;

namespace Dwapi.Bot.Infrastructure
{
    public class BotCleanerContext:DbContext
    {
        public DbSet<Site> Sites { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<SubjectExtract> Extracts { get; set; }

        public BotCleanerContext(DbContextOptions<BotCleanerContext> options) : base(options)
        {
        }
    }
}