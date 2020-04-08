using System;
using Dwapi.Bot.Core.Utility;
using NUnit.Framework;
using Serilog;
using Microsoft.Extensions.DependencyInjection;

namespace Dwapi.Bot.Core.Tests.Utility
{
    [TestFixture]
    public class JaroWinklerScorerTests
    {
        private IJaroWinklerScorer _jaroWinkler;
        [SetUp]
        public void SetUp()
        {
            _jaroWinkler = TestInitializer.ServiceProvider.GetService<IJaroWinklerScorer>();
        }
        [TestCase("maun","maun")]
        [TestCase("mAun","mauN")]
        [TestCase("maun","Baun")]
        public void should_Generate(string a,string b)
        {
            var score = _jaroWinkler.Generate(a, b);
            Assert.NotZero(score);
            Log.Debug($"{a}|{b}, {score}");
        }

        [TestCase("","mauN")]
        [TestCase("maun","")]
        [TestCase("","")]
        public void should_Generate_Throws_If_Empty(string a,string b)
        {
            var ex= Assert.Throws<ArgumentNullException>(()=>_jaroWinkler.Generate(a, b));
            Log.Debug(ex,"Validation");
        }
    }
}
