using Dwapi.Bot.Core.Domain.Configs;
using Dwapi.Bot.Infrastructure.Tests.TestArtifacts;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Serilog;

namespace Dwapi.Bot.Infrastructure.Tests.Data
{
    [TestFixture]
    public class DataSetRepositoryTests
    {
        private IDataSetRepository _repository;

        [OneTimeSetUp]
        public void Init()
        {
            TestInitializer.ClearDb();
            TestInitializer.SeedData(TestData.GenerateSubjects(true));
        }

        [SetUp]
        public void SetUp()
        {
            _repository = TestInitializer.ServiceProvider.GetService<IDataSetRepository>();
        }

        [Test, Order(1)]
        public void should_Get_By_Name()
        {
            var dataSet = _repository.GetByName("Siaya");
            Assert.NotNull(dataSet);
            Log.Debug($"{dataSet.Definition}");
        }
    }
}
