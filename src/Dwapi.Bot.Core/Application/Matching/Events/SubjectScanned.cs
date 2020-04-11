using System;
using Dwapi.Bot.SharedKernel.Enums;
using MediatR;

namespace Dwapi.Bot.Core.Application.Matching.Events
{
    public class SubjectScanned : INotification
    {
        public Guid SubjectIndexId { get; }
        public ScanLevel Level { get; }

        public SubjectScanned(Guid subjectIndexId, ScanLevel level)
        {
            SubjectIndexId = subjectIndexId;
            Level = level;
        }
    }
}
