using System.Collections.Generic;
using System.Threading.Tasks;
using Dwapi.Bot.SharedKernel.Common;

namespace Dwapi.Bot.Core.Domain.Readers
{
    public interface IMasterPatientIndexReader
    {
        DataSourceInfo SourceInfo { get; }
        Task<int> GetRecordCount();
        Task<IEnumerable<MasterPatientIndex>> Read(int page, int pageSize);
    }
}
