using System;
using Dwapi.Bot.Core.Algorithm.JaroWinkler;
using Dwapi.Bot.SharedKernel.Enums;
using Dwapi.Bot.SharedKernel.Model;

namespace Dwapi.Bot.Core.Domain.Indices
{
    public class SubjectIndexScore:Entity<Guid>
    {
        public ScanLevel ScanLevel { get; set; }
        public Guid PatientIndexId { get; set; }
        public Guid OtherPatientIndexId { get; set; }
        public SubjectField Field { get; set; }
        public double Score { get; set; }

        public void GenerateScore(SubjectIndex subjectIndex,SubjectIndex otherSubjectIndex, IJaroWinklerScorer scorer,SubjectField field)
        {
            if (field == SubjectField.PKV)
            {
                Score = scorer.Generate(subjectIndex.sxdmPKValueDoB, otherSubjectIndex.sxdmPKValueDoB);
            }

        }
    }
}
