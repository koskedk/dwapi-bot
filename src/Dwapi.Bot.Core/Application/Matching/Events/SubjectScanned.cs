using Dwapi.Bot.SharedKernel.Enums;
using MediatR;

namespace Dwapi.Bot.Core.Application.Matching.Events
{
    public class SubjectScanned : INotification
    {
        public ScanLevel Level { get; }
        public string LevelCode { get; }
        public int Count { get; set; }
        public int TotalCount { get; set; }

        public SubjectScanned(ScanLevel level, string levelCode, int count, int totalCount)
        {
            Level = level;
            LevelCode = levelCode;
            Count = count;
            TotalCount = totalCount;
        }
    }
}
