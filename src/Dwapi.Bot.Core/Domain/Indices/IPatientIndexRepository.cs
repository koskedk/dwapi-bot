using System.Collections.Generic;

namespace Dwapi.Bot.Core.Domain.Indices
{
    public interface IPatientIndexRepository
    {
        void CreateOrUpdate(IEnumerable<PatientIndex> indices);
        void CreateOrUpdate(IEnumerable<PatientIndexSiteScore> indices);
        void CreateOrUpdate(IEnumerable<PatientIndexInterSiteScore> indices);
    }
}
