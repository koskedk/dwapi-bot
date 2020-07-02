using System.Linq;
using Dwapi.Bot.Core.Domain.Configs;
using Dwapi.Bot.Core.Domain.Indices;
using Dwapi.Bot.Infrastructure.Tests.TestArtifacts;
using Dwapi.Bot.SharedKernel.Enums;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Serilog;

namespace Dwapi.Bot.Infrastructure.Tests.Data
{
    [TestFixture]
    public class BlockStageRepositoryTests
    {
        private IBlockStageRepository _repository;

        [OneTimeSetUp]
        public void Init()
        {
            TestInitializer.ClearDb();
            TestInitializer.SeedData(TestData.GenerateBlocStages());
            TestInitializer.SeedData(TestData.GenerateSubjectsWithStages());
        }

        [SetUp]
        public void SetUp()
        {
            _repository = TestInitializer.ServiceProvider.GetService<IBlockStageRepository>();
        }

        [Test,Order(1)]
        public void should_Get_Stage()
        {
            var blockStage = _repository.GetBlockStage(ScanLevel.Site).Result;
            Assert.NotNull(blockStage);
            Log.Debug($"{blockStage}");
        }

        [Test,Order(1)]
        public void should_init_Stage()
        {
             _repository.InitBlock(ScanLevel.Site).Wait();

             var blockStage = _repository.GetBlockStage(ScanLevel.Site).Result;
             Assert.AreEqual(1,blockStage.BlockCount);
             Assert.AreEqual(0,blockStage.Completed);
             Log.Debug($"{blockStage}");
        }

        [Test,Order(1)]
        public void should_Update_Stage()
        {
            _repository.InitBlock(ScanLevel.InterSite).Wait();

            _repository.UpdateBlock(ScanLevel.InterSite).Wait();

            var blockStage = _repository.GetBlockStage(ScanLevel.InterSite).Result;
            Assert.AreEqual(1,blockStage.BlockCount);
            Assert.AreEqual(1,blockStage.Completed);
            Log.Debug($"{blockStage}");
        }
    }
}
