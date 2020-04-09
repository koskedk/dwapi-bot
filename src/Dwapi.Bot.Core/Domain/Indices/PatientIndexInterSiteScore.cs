using System;
using System.ComponentModel.DataAnnotations;
using Dwapi.Bot.Core.Utility;

namespace Dwapi.Bot.Core.Domain.Indices
{
    public class PatientIndexInterSiteScore
    {
        public Guid Id { get; set; }
        public double? Score { get; set; }
        private Guid? PatientIndexId { get; set; }
        private Guid? OtherPatientIndexId { get; set; }
        public Guid? Session { get; set; }
        public double? Rank { get; set; }

        public PatientIndexInterSiteScore()
        {
            Id = LiveGuid.NewGuid();
        }
    }
}
