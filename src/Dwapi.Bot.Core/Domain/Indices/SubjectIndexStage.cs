using System;
using Dwapi.Bot.SharedKernel.Enums;
using Dwapi.Bot.SharedKernel.Model;

namespace Dwapi.Bot.Core.Domain.Indices
{
    public class SubjectIndexStage:Entity<Guid>
    {
        public ScanStatus Status { get; set; }
        public string StatusInfo { get; set; }
        public DateTime StatusDate { get; set; } = DateTime.Now;
        public Guid PatientIndexId { get; set; }

    }
}
