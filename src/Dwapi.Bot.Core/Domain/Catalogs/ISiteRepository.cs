using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dwapi.Bot.Core.Domain.Catalogs.Dtos;
using Dwapi.Bot.SharedKernel.Interfaces.Data;

namespace Dwapi.Bot.Core.Domain.Catalogs
{
    public interface ISiteRepository : IRepository<Site, Guid>
    {
        Task Clear();
        Task Clear(int[] siteCodes);
        IEnumerable<SubjectExtract> GetExtracts(Guid siteId,List<Guid> patientIds);
    }
}
