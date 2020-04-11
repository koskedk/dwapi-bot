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

        public async Task<IEnumerable<SubjectIndex>> GetAllSubjects(int page = 1, int pageSize = 500)
        {
            page = page < 0 ? 1 : page;
            pageSize = pageSize < 0 ? 1 : pageSize;

            var querry = Context.Set<SubjectIndex>().AsNoTracking()
                .Include(x => x.IndexScores)
                .Include(x => x.IndexStages)
                .OrderBy(x => x.RowId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            return await querry.ToListAsync();
        }

        public  Task<int> GetRecordCount()
        {
            return GetCount<SubjectIndex, Guid>();
        }

        public Task<int> GetRecordCount(ScanLevel level, string code)
        {
            if (level == ScanLevel.Site)
            {
                var isSiteCode = Int32.TryParse(code, out var siteCode);

                if (isSiteCode)
                    return GetCount<SubjectIndex, Guid>(x=>x.SiteCode==siteCode);
            }

            return GetCount<SubjectIndex, Guid>();
        }

        public Task<List<SubjectIndex>> Read(int page, int pageSize)
        {
            return GetAllPaged<SubjectIndex, Guid>(page, pageSize, nameof(SubjectIndex.RowId))
                .ToListAsync();
        }

        public Task<List<SubjectIndex>> Read(int page, int pageSize, ScanLevel level, string code)
        {
            if (level == ScanLevel.Site)
            {
                var isSiteCode = Int32.TryParse(code, out var siteCode);

                if (isSiteCode)
                    return GetAllPaged<SubjectIndex, Guid>(page, pageSize, nameof(SubjectIndex.RowId),
                            x => x.SiteCode == siteCode)
                        .ToListAsync();
            }

            return Read(page, pageSize);
        }

        public Task<int> GetBlockRecordCount(SubjectIndex subject, ScanLevel level)
        {
            var query = GetAll<SubjectIndex, Guid>(x =>
                x.Id!=subject.Id &&
                x.Gender == subject.Gender &&
                x.DOB.Value.Year == subject.DOB.Value.Year);

            if (level == ScanLevel.Site)
                query = query.Where(x => x.SiteCode == subject.SiteCode);

            return query.Select(x => x.Id).CountAsync();
        }

        public Task<List<SubjectIndex>> ReadBlock(int page, int pageSize, SubjectIndex subject, ScanLevel level)
        {
            if (level == ScanLevel.Site)
                return GetAllPaged<SubjectIndex, Guid>(page, pageSize, nameof(SubjectIndex.RowId), x =>
                        x.Id!=subject.Id &&
                        x.SiteCode == subject.SiteCode &&
                        x.Gender == subject.Gender &&
                        x.DOB.Value.Year == subject.DOB.Value.Year)
                    .ToListAsync();

            return GetAllPaged<SubjectIndex, Guid>(page, pageSize, nameof(SubjectIndex.RowId), x =>
                    x.Id!=subject.Id &&
                    x.Gender == subject.Gender &&
                    x.DOB.Value.Year == subject.DOB.Value.Year)
                .ToListAsync();
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

        public Task CreateOrUpdateScores(IEnumerable<SubjectIndexScore> scores)
        {
            return CreateOrUpdateAsync<SubjectIndexScore, Guid>(scores);
        }

        public Task CreateOrUpdateStages(IEnumerable<SubjectIndexStage> stages)
        {
            return CreateOrUpdateAsync<SubjectIndexStage, Guid>(stages);
        }
    }
}
