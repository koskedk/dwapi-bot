using System;
using System.Collections.Generic;
using Dwapi.Bot.Core.Domain.Configs;

namespace Dwapi.Bot.Infrastructure.Data
{
    public class MatchConfigRepository : BaseRepository<MatchConfig, Guid>, IMatchConfigRepository
    {
        public MatchConfigRepository(BotContext context) : base(context)
        {

        }

        public IEnumerable<MatchConfig> GetConfigs()
        {
            return GetAll<MatchConfig, Guid>();
        }
    }
}
