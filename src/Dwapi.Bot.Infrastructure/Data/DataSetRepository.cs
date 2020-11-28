using System;
using System.Linq;
using Dwapi.Bot.Core.Domain.Configs;

namespace Dwapi.Bot.Infrastructure.Data
{
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