using System.Collections.Generic;
using Dwapi.Bot.Core.Domain.Common;

namespace Dwapi.Bot.Core.Domain.Readers
{
    public interface IMasterPatientIndexReader
    {
        DataSourceInfo SourceInfo { get; }
        int PageCount(int batchSize, long totalRecords);

        int GetRecordCount();
        IEnumerable<MasterPatientIndex> Read(int page, int pageSize);
    }
}
