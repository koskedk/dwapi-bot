using Microsoft.EntityFrameworkCore;

namespace Dwapi.Bot.Infrastructure.Data
{
    public class BotContext:DbContext
    {
        public BotContext(DbContextOptions<BotContext> options) : base(options)
        {
        }
    }
}
