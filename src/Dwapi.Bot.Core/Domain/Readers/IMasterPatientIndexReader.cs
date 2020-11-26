using System.Collections.Generic;
using System.Threading.Tasks;
using Dwapi.Bot.Core.Domain.Indices.Dto;
using Dwapi.Bot.SharedKernel.Common;
using Dwapi.Bot.SharedKernel.Interfaces.Data;
using FluentValidation.Resources;

namespace Dwapi.Bot.Core.Domain.Readers
{

    public interface IMasterPatientIndexReader:ISourceReader
    {
        Task<IEnumerable<SubjectSiteDto>> GetMpiSites();
        Task<IEnumerable<SubjectSiteDto>> GetMpiSites(string dataset);
        Task<IEnumerable<MasterPatientIndex>> Read(int page, int pageSize);
        Task<IEnumerable<MasterPatientIndex>> Read(int page, int pageSize,int siteCode);
    }
}
