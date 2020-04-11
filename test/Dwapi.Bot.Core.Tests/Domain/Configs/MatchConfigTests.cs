using System.Collections.Generic;
using System.Linq;
using Dwapi.Bot.Core.Domain.Configs;
using Dwapi.Bot.Infrastructure;
using Dwapi.Bot.SharedKernel.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Serilog;

namespace Dwapi.Bot.Core.Tests.Domain.Configs
{
    [TestFixture]
    public class MatchConfigTests
    {
        private List<MatchConfig> _matchConfigs;

        [OneTimeSetUp]
        public void Init()
        {
            TestInitializer.ClearDb();
            _matchConfigs = TestInitializer.ServiceProvider.GetService<BotContext>().MatchConfigs.AsNoTracking().ToList();
        }

        [TestCase(1,MatchStatus.Match)]
        [TestCase(.98,MatchStatus.Possible)]
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
