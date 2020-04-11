using System.Linq;
using Dwapi.Bot.Core.Domain.Configs;
using Dwapi.Bot.SharedKernel.Utility;

namespace Dwapi.Bot.Infrastructure.Configuration
{
    public  static class SeedBotContextExtensions
    {
        public static void Plant(this BotContext context)
        {
            if (!context.MatchConfigs.Any())
            {
                var data = SeedDataReader.ReadCsv<MatchConfig>(typeof(BotContext).Assembly);
                context.AddRange(data);
            }

            context.SaveChanges();
        }
    }
}
