using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Dwapi.Bot.Core.Domain.Catalogs;
using Dwapi.Bot.Core.Domain.Catalogs.Dtos;
using Dwapi.Bot.Core.Domain.Configs;
using Dwapi.Bot.Core.Domain.Indices;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Z.Dapper.Plus;

namespace Dwapi.Bot.Infrastructure.Data
{
    public class SiteRepository : BaseRepository<MatchConfig, Guid>, ISiteRepository
    {
        public SiteRepository(BotCleanerContext context) : base(context)
        {
        }

        public async Task Clear()
        {
            var sql = $@" DELETE FROM {nameof(BotCleanerContext.Sites)};";

            using (var cn = GetConnectionOnly())
            {
                if (cn.State != ConnectionState.Open)
                    cn.Open();

                await cn.ExecuteAsync(sql,commandTimeout:0);
            }
        }

        public async Task Clear(int[] siteCodes)
        {
            var sql = $@" DELETE FROM {nameof(BotCleanerContext.Sites)} WHERE {nameof(Site.Code)} in @siteCodes;";

            using (var cn = GetConnectionOnly())
            {
                if (cn.State != ConnectionState.Open)
                    cn.Open();

                await cn.ExecuteAsync(sql,new {siteCodes},commandTimeout:0);
            }
        }

        public IEnumerable<SubjectExtractDto> GetExtracts(Guid siteId)
        {
            var list=new List<SubjectExtractDto>();

            var sql = @"
                select e.*,s.PatientPk from Extracts e
                    inner join Subjects s on e.PatientId=s.PatientId and e.SiteId=s.SiteId
                where e.SiteId=@siteId
                ";

            using (var cn = GetConnectionOnly())
            {
                if (cn.State != ConnectionState.Open)
                    cn.Open();

                list = cn.Query<SubjectExtractDto>(sql,new{siteId},commandTimeout:0).ToList();
            }

            return list;
        }

        public IDbConnection GetConnectionOnly()
        {
            IDbConnection cn = null;
            if (Context.Database.IsSqlServer())
                cn = new SqlConnection(ConnectionString);

            if (Context.Database.IsSqlite())
                cn = new SqliteConnection(ConnectionString);

            return cn;
        }
    }
}
