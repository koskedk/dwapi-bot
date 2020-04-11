using System;
using Dwapi.Bot.Core.Algorithm.JaroWinkler;
using Dwapi.Bot.SharedKernel.Enums;
using Dwapi.Bot.SharedKernel.Model;

namespace Dwapi.Bot.Core.Domain.Indices
{
    public class SubjectIndexScore:Entity<Guid>
    {
        public ScanLevel ScanLevel { get; set; }
        public Guid OtherSubjectIndexId { get; set; }
        public SubjectField Field { get; set; }
        public double Score { get; set; }
        public Guid SubjectIndexId { get; set; }

        public SubjectIndexScore()
        {
        }

        public SubjectIndexScore(ScanLevel scanLevel, Guid subjectIndexId, Guid otherSubjectIndexId, SubjectField field)
        {
            ScanLevel = scanLevel;
            SubjectIndexId = subjectIndexId;
            OtherSubjectIndexId = otherSubjectIndexId;
            Field = field;
        }

        public static SubjectIndexScore GenerateScore(SubjectIndex subjectIndex,SubjectIndex otherSubjectIndex,ScanLevel scanLevel, IJaroWinklerScorer scorer,SubjectField field)
        {
            var indexScore=new SubjectIndexScore(scanLevel,subjectIndex.Id,otherSubjectIndex.Id,field);
            indexScore.SetScore(subjectIndex, otherSubjectIndex, scorer, field);
            return indexScore;
        }

        private void SetScore(SubjectIndex subjectIndex, SubjectIndex otherSubjectIndex, IJaroWinklerScorer scorer,
            SubjectField field)
        {
            if (field == SubjectField.PKV)
                Score = scorer.Generate(subjectIndex.sxdmPKValueDoB, otherSubjectIndex.sxdmPKValueDoB);
        }

        public override string ToString()
        {
            return $"{ScanLevel} {Field} {Score}";
        }
    }
}
