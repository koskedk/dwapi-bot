using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Dwapi.Bot.Infrastructure.Tests
{
    public class BotContextFactory : IDesignTimeDbContextFactory<BotContext>
    {
        public BotContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<BotContext>();
            optionsBuilder.UseSqlite("Data Source=TestArtifacts/Database/dwapibot.db");
            return new BotContext(optionsBuilder.Options);
        }
    }
}
