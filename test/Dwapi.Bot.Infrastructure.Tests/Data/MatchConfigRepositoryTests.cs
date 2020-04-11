using System.Linq;
using Dwapi.Bot.Core.Domain.Configs;
using Dwapi.Bot.Infrastructure.Tests.TestArtifacts;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Serilog;

namespace Dwapi.Bot.Infrastructure.Tests.Data
{
    [TestFixture]
    public class MatchConfigRepositoryTests
    {
        private IMatchConfigRepository _repository;

        [OneTimeSetUp]
        public void Init()
        {
            TestInitializer.ClearDb();
            TestInitializer.SeedData(TestData.GenerateSubjects(true));
        }

        [SetUp]
        public void SetUp()
        {
            _repository = TestInitializer.ServiceProvider.GetService<IMatchConfigRepository>();
        }
        [Test,Order(1)]
        public void should_Get_All()
        {
            var configs = _repository.GetConfigs().ToList();
            Assert.True(configs.Any());
            Log.Debug($"{configs.Count} Records");
        }
 }
}
