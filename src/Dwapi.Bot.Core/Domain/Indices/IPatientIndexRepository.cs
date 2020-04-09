using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dwapi.Bot.Core.Domain.Indices
{
    public interface IPatientIndexRepository
    {
        int PageCount(int batchSize, long totalRecords);
        Task<int> GetRecordCount();
        Task<List<PatientIndex>> Read(int page, int pageSize,int? siteCode=null);
        void CreateOrUpdate(IEnumerable<PatientIndex> indices);
        void CreateOrUpdate(IEnumerable<PatientIndexSiteScore> indices);
        void CreateOrUpdate(IEnumerable<PatientIndexInterSiteScore> indices);
    }
}
