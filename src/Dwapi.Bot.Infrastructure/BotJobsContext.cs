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
    }
}
