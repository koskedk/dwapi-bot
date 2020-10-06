using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Dwapi.Bot.Core.Domain.Indices.Dto;
using Dwapi.Bot.Core.Domain.Readers;
using Dwapi.Bot.Infrastructure.Configuration;
using Dwapi.Bot.SharedKernel.Common;
using Microsoft.Data.Sqlite;

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

        public async Task<int> GetRecordCount()
        {
            int count = 0;
            var sql = @"SELECT COUNT(Id) FROM MasterPatientIndices WHERE Gender<>'U' AND NOT ISNULL(sxdmPKValueDoB,'') = ''";
            if (SourceInfo.DbType == SharedKernel.Enums.DbType.SQLite)
                sql = sql.Replace("ISNULL", "IFNULL");
            using (var con = GetConnection())
            {
                con.Open();
                count = await con.ExecuteScalarAsync<int>(sql);
            }

            return count;
        }

        public async Task<int> GetRecordCount(int siteCode)
        {
            int count = 0;
            var sql = @"SELECT COUNT(Id) FROM MasterPatientIndices WHERE SiteCode=@siteCode AND Gender<>'U' AND NOT ISNULL(sxdmPKValueDoB,'') = ''";
            if (SourceInfo.DbType == SharedKernel.Enums.DbType.SQLite)
                sql = sql.Replace("ISNULL", "IFNULL");
            using (var con = GetConnection())
            {
                con.Open();
                count = await con.ExecuteScalarAsync<int>(sql,new {siteCode});
            }
            return count;
        }

        public async Task<IEnumerable<SubjectSiteDto>> GetMpiSites()
        {
            var sql = $@"SELECT DISTINCT {nameof(SubjectSiteDto.SiteCode)},MAX({nameof(SubjectSiteDto.FacilityName)}) FacilityName 
                                FROM MasterPatientIndices
                                WHERE  Gender<>'U' AND NOT ISNULL(sxdmPKValueDoB,'') = ''
                                GROUP BY SiteCode";

            if (SourceInfo.DbType == SharedKernel.Enums.DbType.SQLite)
                sql = sql.Replace("ISNULL", "IFNULL");

            return await  GetConnection().QueryAsync<SubjectSiteDto>(sql);
        }

        public async Task<IEnumerable<MasterPatientIndex>> Read(int page, int pageSize)
        {
            IEnumerable<MasterPatientIndex> records;
            page = page < 0 ? 1 : page;
            pageSize = pageSize < 0 ? 1 : pageSize;

            var sql = @"SELECT * FROM MasterPatientIndices WHERE Gender<>'U' AND NOT ISNULL(sxdmPKValueDoB,'') = '' ORDER BY RowId";
            if (SourceInfo.DbType == SharedKernel.Enums.DbType.SQLite)
                sql = sql.Replace("ISNULL", "IFNULL");

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

        public async Task<IEnumerable<MasterPatientIndex>> Read(int page, int pageSize, int siteCode)
        {
            IEnumerable<MasterPatientIndex> records;
            page = page < 0 ? 1 : page;
            pageSize = pageSize < 0 ? 1 : pageSize;

            var sql = @"SELECT * FROM MasterPatientIndices WHERE SiteCode=@siteCode AND Gender<>'U' AND NOT ISNULL(sxdmPKValueDoB,'') = '' ORDER BY RowId";
            if (SourceInfo.DbType == SharedKernel.Enums.DbType.SQLite)
                sql = sql.Replace("ISNULL", "IFNULL");

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
                    siteCode,
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
