using System;
using System.Collections.Generic;
using System.Linq;
using Dwapi.Bot.SharedKernel.Enums;
using Dwapi.Bot.SharedKernel.Model;

namespace Dwapi.Bot.Core.Domain.Configs
{
    public class MatchConfig:Entity<Guid>
    {
        public MatchStatus MatchStatus { get; set; }
        public double? MinThreshold { get; set; }
        public double? MaxThreshold { get; set; }
        public string Description { get; set; }

        public MatchConfig()
        {
        }

        public MatchConfig(MatchStatus matchStatus, double minThreshold, double maxThreshold, string description)
        {
            MatchStatus = matchStatus;
            MinThreshold = minThreshold;
            MaxThreshold = maxThreshold;
            Description = description;
        }

        private bool GenerateStatus(double score)
        {
            var s = (decimal) score;

            if (
                MatchStatus != MatchStatus.None &&
                s >= (decimal) MinThreshold.Value &&
                (decimal) score < (decimal) MaxThreshold.Value)
                return true;

            return false;

        }

        public static MatchStatus GenerateStatus(List<MatchConfig> configs,double score)
        {
            var config = configs.Where(x => x.GenerateStatus(score)).ToList();
            return config.First().MatchStatus;
        }
    }
}
