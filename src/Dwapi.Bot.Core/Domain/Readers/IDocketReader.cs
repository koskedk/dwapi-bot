using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dwapi.Bot.Core.Domain.Catalogs;
using Dwapi.Bot.SharedKernel.Interfaces.Data;

namespace Dwapi.Bot.Core.Domain.Readers
{
    public interface IDocketReader:ISourceReader
    {
        Task<IEnumerable<Site>> GetSites();
        Task<IEnumerable<Site>> GetSites(int[] siteCodes);

        Task<IEnumerable<Subject>> GetSubjects(Guid facilityId);
        Task<IEnumerable<SubjectExtract>> GetSubjectExtracts(List<Guid> siteSubjectIds);

        Task CleanExtract(string extract, Guid candidatePatientId,List<Guid> patientIds,List<Guid> extractIds);
        Task CleanSubject(List<Guid> patientIds);
    }
}
