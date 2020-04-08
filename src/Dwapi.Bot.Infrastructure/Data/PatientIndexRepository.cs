using System.Collections.Generic;
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
