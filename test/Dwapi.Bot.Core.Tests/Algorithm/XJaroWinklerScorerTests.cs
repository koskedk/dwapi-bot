using System;
using Dwapi.Bot.Core.Algorithm.JaroWinkler;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Serilog;

namespace Dwapi.Bot.Core.Tests.Algorithm
{
    [TestFixture]
    public class XJaroWinklerScorerTests
    {
        private IJaroWinklerScorer _jaroWinkler;
        [SetUp]
        public void SetUp()
        {
            _jaroWinkler = new XJaroWinklerScorer();
        }
        [TestCase("maun","maun")]
        [TestCase("mAun","mauN")]
        [TestCase("MARTHA","MATHRA")]
        public void should_Generate(string a,string b)
        {
            var score = _jaroWinkler.Generate(a, b);
            Assert.NotZero(score);
            Log.Debug($"{a}|{b}, {score}");
        }
/*
  select [dbo].[fn_calculateJaroWinkler]('','') UNION all
 select [dbo].[fn_calculateJaroWinkler]('','') UNION all
 select [dbo].[fn_calculateJaroWinkler]('','') UNION all
select [dbo].[fn_calculateJaroWinkler]('','') UNION all
select [dbo].[fn_calculateJaroWinkler]('','') UNION all
select [dbo].[fn_calculateJaroWinkler]('','')
 */
        [TestCase("FB362AKN19830625", "FB362AKN19830625")]
        [TestCase("FB362AKN19830625", "FE540ATN19880806")]
        [TestCase("FM263AKNK19560615","FE540ATN19880806")]
        [TestCase("FM4253AK19810615", "FE540ATN19880806")]
        [TestCase("FM4253AK19810615", "FM450ANNK20070615")]
        [TestCase("FE540ATN19880806", "FE540ATN19880806")]
        public void should_Generate_(string a,string b)
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
