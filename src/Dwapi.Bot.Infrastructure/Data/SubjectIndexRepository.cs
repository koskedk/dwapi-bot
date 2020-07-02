using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Dwapi.Bot.Core.Domain.Indices;
using Dwapi.Bot.Core.Domain.Indices.Dto;
using Dwapi.Bot.SharedKernel.Enums;
using Dwapi.Bot.SharedKernel.Utility;
using Microsoft.EntityFrameworkCore;
using Serilog;

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

        public Task<int> GetRecordCount()
        {
            return GetCount<SubjectIndex, Guid>();
        }

        public Task<int> GetRecordCount(ScanLevel level, string code)
        {
            if (level == ScanLevel.Site)
            {
                var isSiteCode = Int32.TryParse(code, out var siteCode);

                if (isSiteCode)
                    return GetCount<SubjectIndex, Guid>(x => x.SiteCode == siteCode);
            }

            return GetCount<SubjectIndex, Guid>();
        }

        public Task<int> GetRecordCount(ScanLevel level, Guid blockId)
        {
            if (level == ScanLevel.Site)
            {
                return GetCount<SubjectIndex, Guid>(x => x.SiteBlockId == blockId);
            }

            return GetCount<SubjectIndex, Guid>(x => x.InterSiteBlockId == blockId);
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

        public Task<List<SubjectIndex>> Read(int page, int pageSize, ScanLevel level, Guid blockId)
        {
            if (level == ScanLevel.Site)
            {
                    return GetAllPaged<SubjectIndex, Guid>(page, pageSize, nameof(SubjectIndex.RowId),
                            x => x.SiteBlockId == blockId)
                        .ToListAsync();
            }

            return GetAllPaged<SubjectIndex, Guid>(page, pageSize, nameof(SubjectIndex.RowId),
                    x => x.InterSiteBlockId == blockId)
                .ToListAsync();

        }


        public Task<int> GetBlockRecordCount(SubjectIndex subject, ScanLevel level)
        {
            var query = GetAll<SubjectIndex, Guid>(x =>
                x.Id != subject.Id &&
                x.Gender == subject.Gender &&
                x.DOB.Value.Year == subject.DOB.Value.Year);

            if (level == ScanLevel.Site)
                query = query.Where(x => x.SiteCode == subject.SiteCode);

            if (level == ScanLevel.InterSite)
                query = query.Where(x => x.SiteCode != subject.SiteCode);

            return query.Select(x => x.Id).CountAsync();
        }

        public Task<List<SubjectIndex>> ReadBlock(int page, int pageSize, SubjectIndex subject, ScanLevel level)
        {
            if (level == ScanLevel.Site)
                return GetAllPaged<SubjectIndex, Guid>(page, pageSize, nameof(SubjectIndex.RowId), x =>
                        x.Id != subject.Id &&
                        x.SiteCode == subject.SiteCode &&
                        x.Gender == subject.Gender &&
                        x.DOB.Value.Year == subject.DOB.Value.Year)
                    .ToListAsync();

            return GetAllPaged<SubjectIndex, Guid>(page, pageSize, nameof(SubjectIndex.RowId), x =>
                    x.Id != subject.Id &&
                    x.SiteCode != subject.SiteCode &&
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

        public Task Clear(int siteCode)
        {
            using var cn = GetConnectionOnly();
            cn.Open();
            var sql =
                $"DELETE FROM {nameof(BotContext.SubjectIndices)} Where {nameof(SubjectIndex.SiteCode)}=@siteCode";

            return cn.ExecuteAsync(sql, new {siteCode});
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

        public async Task<IEnumerable<SubjectSiteDto>> GetSubjectSiteDtos()
        {
            var sql =
                $@"SELECT DISTINCT {nameof(SubjectSiteDto.SiteCode)},MAX({nameof(SubjectSiteDto.FacilityName)}) FacilityName  
                                FROM {nameof(BotContext.SubjectIndices)} GROUP BY SiteCode";

            return await GetConnection().QueryAsync<SubjectSiteDto>(sql);
        }

        public async Task<IEnumerable<SubjectBlockDto>> GetSubjectInterSiteBlockDtos()
        {
            var sql = $@"

              select * from (
                  select distinct Year(DOB) BirthYear, {nameof(SubjectIndex.Gender)}, Count({nameof(SubjectIndex.Id)}) BlockCount
                  from {nameof(BotContext.SubjectIndices)}
                  group by Year(DOB), Gender
              )x
                where x.BlockCount>1";

            if (Context.Database.IsSqlite())
                sql = sql.Replace("Year(DOB)", @"strftime('%Y',DOB)");

            return await GetConnection().QueryAsync<SubjectBlockDto>(sql);
        }

        public async Task<IEnumerable<SubjectBlockDto>> GetSubjectSiteBlockDtos()
        {
            var sql = $@"
              select * from (
                  select distinct Year(DOB) BirthYear, {nameof(SubjectIndex.Gender)}, Count({nameof(SubjectIndex.Id)}) BlockCount,SiteCode
                  from {nameof(BotContext.SubjectIndices)}
                  group by Year(DOB), Gender,SiteCode
              )x
                where x.BlockCount>1";

            if (Context.Database.IsSqlite())
                sql = sql.Replace("Year(DOB)", @"strftime('%Y',DOB)");

            return await GetConnection().QueryAsync<SubjectBlockDto>(sql);
        }

        public Task BlockInterSiteSubjects(SubjectBlockDto blockDto)
        {
            var sql = $@"
              update {nameof(BotContext.SubjectIndices)} 
              set InterSiteBlockId=@blockId 
              where Year(DOB)=@year and Gender=@gender";

            if (Context.Database.IsSqlite())
                sql = sql.Replace("Year(DOB)", @"CAST(strftime('%Y',DOB) as integer)");

            using var cn = GetConnectionOnly();
            cn.Open();
            return cn.ExecuteAsync(sql,
                new {year = blockDto.BirthYear, gender = blockDto.Gender,
                    blockId = LiveGuid.NewGuid()});
        }

        public Task BlockSiteSubjects(SubjectBlockDto blockDto)
        {
            var sql = $@"
              update {nameof(BotContext.SubjectIndices)} 
              set SiteBlockId=@blockId 
              where Year(DOB)=@year and Gender=@gender and SiteCode=@siteCode";

            if (Context.Database.IsSqlite())
                sql = sql.Replace("Year(DOB)", @"CAST(strftime('%Y',DOB) as integer)");

            using var cn = GetConnectionOnly();
            cn.Open();
            return cn.ExecuteAsync(sql,
                new
                {
                    year = blockDto.BirthYear, gender = blockDto.Gender, siteCode = blockDto.SiteCode,
                    blockId = LiveGuid.NewGuid()
                });
        }

        public async Task<IEnumerable<Guid>> GetSiteBlocks()
        {
            var sql =
                $@"SELECT DISTINCT {nameof(SubjectIndex.SiteBlockId)} FROM {nameof(BotContext.SubjectIndices)} WHERE {nameof(SubjectIndex.SiteBlockId)} IS NOT NULL ";

            var results=await GetConnection().QueryAsync<Guid>(sql);
            return results;
        }

        public async Task<IEnumerable<Guid>> GetInterSiteBlocks()
        {
            var sql =
                $@"SELECT DISTINCT {nameof(SubjectIndex.InterSiteBlockId)} FROM {nameof(BotContext.SubjectIndices)} WHERE {nameof(SubjectIndex.InterSiteBlockId)} IS NOT NULL";

            return await GetConnection().QueryAsync<Guid>(sql);
        }
    }
}
