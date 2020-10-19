using System;
using System.Collections.Generic;
using System.Linq;
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

    public class DataSetRepository : BaseRepository<DataSet, Guid>, IDataSetRepository
    {
        public DataSetRepository(BotContext context) : base(context)
        {

        }
        public DataSet GetByName(string name)
        {
            return GetAll<DataSet, Guid>().FirstOrDefault(x => x.Name.ToLower()==name.ToLower());
        }
    }
}
