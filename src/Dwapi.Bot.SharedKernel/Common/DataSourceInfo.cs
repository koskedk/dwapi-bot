using Dwapi.Bot.SharedKernel.Enums;

namespace Dwapi.Bot.SharedKernel.Common
{
    public class DataSourceInfo
    {
        public DbType DbType { get; set; }
        public string Connection { get; set; }

        public DataSourceInfo(DbType dbType, string connection)
        {
            DbType = dbType;
            Connection = connection;
        }
    }
}
