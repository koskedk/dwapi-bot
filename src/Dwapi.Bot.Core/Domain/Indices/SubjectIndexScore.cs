using System;
using Dwapi.Bot.Core.Algorithm.JaroWinkler;
using Dwapi.Bot.SharedKernel.Enums;
using Dwapi.Bot.SharedKernel.Model;

namespace Dwapi.Bot.Core.Domain.Indices
{
    public class SubjectIndexScore:Entity<Guid>
    {
        public ScanLevel ScanLevel { get; set; }
        public string ScanLevelCode { get; set; }
        public Guid OtherSubjectIndexId { get; set; }
        public SubjectField Field { get; set; }
        public double Score { get; set; }
        public Guid SubjectIndexId { get; set; }

        public SubjectIndexScore()
        {
        }

        public SubjectIndexScore(ScanLevel scanLevel, Guid subjectIndexId, Guid otherSubjectIndexId, SubjectField field,
            string scanLevelCode)
        {
            ScanLevel = scanLevel;
            SubjectIndexId = subjectIndexId;
            OtherSubjectIndexId = otherSubjectIndexId;
            Field = field;
            if (!string.IsNullOrWhiteSpace(scanLevelCode))
                ScanLevelCode = scanLevelCode;
        }

        public static SubjectIndexScore GenerateScore(SubjectIndex subjectIndex,SubjectIndex otherSubjectIndex,ScanLevel scanLevel, IJaroWinklerScorer scorer,SubjectField field,string levelCode)
        {
            var indexScore=new SubjectIndexScore(scanLevel,subjectIndex.Id,otherSubjectIndex.Id,field,levelCode);
            indexScore.SetScore(subjectIndex, otherSubjectIndex, scorer, field);
            return indexScore;
        }

        private void SetScore(SubjectIndex subjectIndex, SubjectIndex otherSubjectIndex, IJaroWinklerScorer scorer,
            SubjectField field)
        {
            if (field == SubjectField.PKV)
                Score = scorer.Generate(subjectIndex.sxdmPKValueDoB, otherSubjectIndex.sxdmPKValueDoB);
            if (field == SubjectField.Serial)
                Score = scorer.GenerateExact(subjectIndex.Serial, otherSubjectIndex.Serial);
        }

        public override string ToString()
        {
            var code = string.IsNullOrWhiteSpace(ScanLevelCode) ? "" : $"[{ScanLevelCode}]";
            return $"{ScanLevel} {code} {Field} {Score}";
        }
    }
}
