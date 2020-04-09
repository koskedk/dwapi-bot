using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Dwapi.Bot.Core.Domain.Readers;
using Dwapi.Bot.Infrastructure.Configuration;
using Dwapi.Bot.SharedKernel.Common;
using Microsoft.Data.Sqlite;
using Z.Dapper.Plus;

namespace Dwapi.Bot.Infrastructure.Data
{
    public class MasterPatientIndexReader : IMasterPatientIndexReader
    {
        private readonly SourceConfiguration _sourceConfiguration;
        public DataSourceInfo SourceInfo { get; }

        public MasterPatientIndexReader(DataSourceInfo sourceInfo)
        {
            SourceInfo = sourceInfo;
            _sourceConfiguration = new SourceConfiguration();
        }

        public int PageCount(int batchSize, long totalRecords)
        {
            if (totalRecords > 0)
            {
                if (totalRecords < batchSize)
                {
                    return 1;
                }

                return (int) Math.Ceiling(totalRecords / (double) batchSize);
            }

            return 0;
        }

        public async Task<int> GetRecordCount()
        {
            int count = 0;
            var sql = @"SELECT COUNT(Id) FROM MasterPatientIndices";
            using (var con = GetConnection())
            {
                con.Open();
                count = await con.ExecuteScalarAsync<int>(sql);
            }

            return count;
        }

        public async Task<IEnumerable<MasterPatientIndex>> Read(int page, int pageSize)
        {
            IEnumerable<MasterPatientIndex> records;
            page = page < 0 ? 1 : page;
            pageSize = pageSize < 0 ? 1 : pageSize;

            var sql = @"SELECT * FROM MasterPatientIndices ORDER BY RowId";

            var sqlPaging = @"
                 OFFSET @Offset ROWS 
                 FETCH NEXT @PageSize ROWS ONLY
            ";

            if (SourceInfo.DbType == SharedKernel.Enums.DbType.SQLite)
            {
                sqlPaging = @" LIMIT @PageSize OFFSET @Offset;";
            }

            sql = $"{sql}{sqlPaging}";

            using (var con = GetConnection())
            {
                con.Open();
                records = await con.QueryAsync<MasterPatientIndex>(sql, new
                {
                    Offset = (page - 1) * pageSize,
                    PageSize = pageSize
                });
            }

            return records;
        }

        private IDbConnection GetConnection()
        {
            var connectionString = SourceInfo.Connection;

            if (SourceInfo.DbType == SharedKernel.Enums.DbType.MsSQL)
                return new System.Data.SqlClient.SqlConnection(connectionString);

            if (SourceInfo.DbType == SharedKernel.Enums.DbType.SQLite)
                return new SqliteConnection(connectionString);

            return null;
        }
    }
}
