using System.Collections.Generic;
using Dwapi.Bot.Core.Domain.Configs;
using Dwapi.Bot.SharedKernel.Enums;
using NUnit.Framework;
using Serilog;

namespace Dwapi.Bot.Core.Tests.Domain.Configs
{
    [TestFixture]
    public class MatchConfigTests
    {
        private List<MatchConfig> _matchConfigs;
        [SetUp]
        public void Setup()
        {
            _matchConfigs=new List<MatchConfig>
            {
                new MatchConfig(MatchStatus.Match,1,1.1,$"{MatchStatus.Match}"),
                new MatchConfig(MatchStatus.Possible,.96,1,$"{MatchStatus.Possible}"),
                new MatchConfig(MatchStatus.NonMatch,0,.96,$"{MatchStatus.NonMatch}")
            };
        }

        [TestCase(1,MatchStatus.Match)]
        [TestCase(.96,MatchStatus.Possible)]
        [TestCase(.95,MatchStatus.NonMatch)]
        [TestCase(.86,MatchStatus.NonMatch)]
        [TestCase(0,MatchStatus.NonMatch)]
        public void should_Generate_Status(double score,MatchStatus status)
        {
            Assert.AreEqual(MatchConfig.GenerateStatus(_matchConfigs, score),status);
            Log.Debug($"{score} | {status}");
        }
    }
}
