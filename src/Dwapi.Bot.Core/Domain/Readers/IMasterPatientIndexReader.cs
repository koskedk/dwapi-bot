using System.Collections.Generic;
using System.Threading.Tasks;
using Dwapi.Bot.SharedKernel.Common;

namespace Dwapi.Bot.Core.Domain.Readers
{
    public interface IMasterPatientIndexReader
    {
        DataSourceInfo SourceInfo { get; }
        int PageCount(int batchSize, long totalRecords);
        Task<int> GetRecordCount();
        Task<IEnumerable<MasterPatientIndex>> Read(int page, int pageSize);
    }
}
