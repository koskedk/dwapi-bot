using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Dapper;
using Dwapi.Bot.Core.Domain.Indices;
using Dwapi.Bot.SharedKernel.Enums;
using Microsoft.EntityFrameworkCore;
using Z.Dapper.Plus;

namespace Dwapi.Bot.Infrastructure.Data
{
    public class SubjectIndexRepository : BaseRepository<SubjectIndex, Guid>, ISubjectIndexRepository
    {
        public SubjectIndexRepository(BotContext context) : base(context)
        {
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
            var count = await GetAll<SubjectIndex, Guid>()
                .Select(x => x.Id)
                .CountAsync();
            return count;
        }

        public Task<List<SubjectIndex>> Read(int page, int pageSize, int? siteCode)
        {
            page = page < 0 ? 1 : page;
            pageSize = pageSize < 0 ? 1 : pageSize;

            var query = GetAll<SubjectIndex, Guid>();

            if (siteCode.HasValue)
                query = query.Where(x => x.SiteCode == siteCode.Value);

            return query
                .OrderBy(x => x.RowId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize).ToListAsync();
        }

        public Task<int> GetBlockRecordCount(SubjectIndex subject, ScanLevel level)
        {
            var query = GetAll<SubjectIndex, Guid>(x =>
                x.Gender == subject.Gender &&
                x.DOB.Value.Year == subject.DOB.Value.Year);

            if (level == ScanLevel.Site)
                query = query.Where(x => x.SiteCode == subject.SiteCode);

            return query.Select(x => x.Id).CountAsync();
        }

        public Task<List<SubjectIndex>> ReadBlock(int page, int pageSize, SubjectIndex subject, ScanLevel level)
        {
            page = page < 0 ? 1 : page;
            pageSize = pageSize < 0 ? 1 : pageSize;

            var query = GetAll<SubjectIndex, Guid>(x =>
                x.Gender == subject.Gender &&
                x.DOB.Value.Year == subject.DOB.Value.Year);

            if (level == ScanLevel.Site)
                query = query.Where(x => x.SiteCode == subject.SiteCode);

            return query
                .OrderBy(x => x.RowId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize).ToListAsync();
        }

        public async Task Clear()
        {
            //clear

            var sql = $"DELETE FROM {nameof(BotContext.SubjectIndices)}";

            var count = await GetConnection().ExecuteAsync(sql);
        }

        public Task CreateOrUpdate(IEnumerable<SubjectIndex> indices)
        {
            return CreateOrUpdateAsync<SubjectIndex, Guid>(indices);
        }

        public Task CreateOrUpdate(IEnumerable<SubjectIndexScore> scores)
        {
            return CreateOrUpdateAsync<SubjectIndexScore, Guid>(scores);
        }

        public Task CreateOrUpdate(IEnumerable<SubjectIndexStage> stages)
        {
            return CreateOrUpdateAsync<SubjectIndexStage, Guid>(stages);
        }
    }
}
