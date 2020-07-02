using System.Collections.Generic;
using System.Threading.Tasks;
using Dwapi.Bot.Core.Domain.Indices.Dto;
using Dwapi.Bot.SharedKernel.Common;

namespace Dwapi.Bot.Core.Domain.Readers
{
    public interface IMasterPatientIndexReader
    {
        DataSourceInfo SourceInfo { get; }
        Task<int> GetRecordCount();
        Task<int> GetRecordCount(int siteCode);
        Task<IEnumerable<SubjectSiteDto>> GetMpiSites();
        Task<IEnumerable<MasterPatientIndex>> Read(int page, int pageSize);
        Task<IEnumerable<MasterPatientIndex>> Read(int page, int pageSize,int siteCode);
    }
}
