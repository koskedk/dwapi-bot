using System;
using System.Collections.Generic;
using System.Data;
using Dwapi.Bot.Core.Domain.Common;
using Dwapi.Bot.Core.Domain.Readers;
using Microsoft.Data.Sqlite;
using DbType = Dwapi.Bot.Core.Domain.Common.DbType;

namespace Dwapi.Bot.Infrastructure.Data
{
    public class MasterPatientIndexReader:IMasterPatientIndexReader
    {
        public DataSourceInfo SourceInfo { get; }

        public MasterPatientIndexReader(DataSourceInfo sourceInfo)
        {
            SourceInfo = sourceInfo;
        }

        public int PageCount(int batchSize, long totalRecords)
        {
            if (totalRecords > 0) {
                if (totalRecords < batchSize) {
                    return 1;
                }
                return (int)Math.Ceiling(totalRecords / (double)batchSize);
            }
            return 0;
        }

        public int GetRecordCount()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<MasterPatientIndex> Read(int page, int pageSize)
        {
            throw new NotImplementedException();
        }

        private IDbConnection GetConnection()
        {
                var connectionString = SourceInfo.Connection;

                if (SourceInfo.DbType == DbType.MsSQL)
                    return new System.Data.SqlClient.SqlConnection(connectionString);

                if (SourceInfo.DbType == DbType.SQLite)
                    return new SqliteConnection(connectionString);

                return null;
        }
    }
}
