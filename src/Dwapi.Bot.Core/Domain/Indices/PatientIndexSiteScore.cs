using System;
using Dwapi.Bot.Core.Utility;

namespace Dwapi.Bot.Core.Domain.Indices
{
    public class PatientIndexSiteScore
    {
        public Guid Id { get;  }
        public double Score { get; set; }
        private Guid PatientIndexId { get; set; }
        private Guid OtherPatientIndexId { get; set; }
        public Guid Session { get; set; }
        public double Rank { get; set; }
        public PatientIndexSiteScore()
        {
            Id = LiveGuid.NewGuid();
        }
    }
}
