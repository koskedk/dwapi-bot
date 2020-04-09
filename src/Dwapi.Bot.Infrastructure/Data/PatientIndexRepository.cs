using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dwapi.Bot.Core.Domain.Indices;
using Microsoft.EntityFrameworkCore;
using Z.Dapper.Plus;

namespace Dwapi.Bot.Infrastructure.Data
{
    public class PatientIndexRepository:IPatientIndexRepository
    {
        private readonly BotContext _context;

        public PatientIndexRepository(BotContext context)
        {
            _context = context;
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
            var count =await _context.PatientIndices.AsNoTracking()
                .Select(x => x.Id)
                .CountAsync();
            return count;
        }

        public Task<List<PatientIndex>> Read(int page, int pageSize, int? siteCode)
        {
            page = page < 0 ? 1 : page;
            pageSize = pageSize < 0 ? 1 : pageSize;

            var query= _context.PatientIndices.AsNoTracking();

            if (siteCode.HasValue)
                query = query.Where(x => x.SiteCode == siteCode.Value);

            return query
                .Skip((page - 1) * pageSize)
                .Take(pageSize).ToListAsync();
        }

        public void CreateOrUpdate(IEnumerable<PatientIndex> indices)
        {
            _context.Database.GetDbConnection().BulkInsert(indices);
        }

        public void CreateOrUpdate(IEnumerable<PatientIndexSiteScore> indices)
        {
            throw new System.NotImplementedException();
        }

        public void CreateOrUpdate(IEnumerable<PatientIndexInterSiteScore> indices)
        {
            throw new System.NotImplementedException();
        }
    }
}
