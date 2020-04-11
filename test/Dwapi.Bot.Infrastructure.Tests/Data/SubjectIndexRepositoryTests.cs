using System;
using System.Linq;
using Dapper;
using Dwapi.Bot.Core.Domain.Indices;
using Dwapi.Bot.Core.Domain.Readers;
using Dwapi.Bot.Infrastructure.Tests.TestArtifacts;
using Dwapi.Bot.SharedKernel.Enums;
using FizzWare.NBuilder;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Serilog;

namespace Dwapi.Bot.Infrastructure.Tests.Data
{
    [TestFixture]
    public class SubjectIndexRepositoryTests
    {
        private ISubjectIndexRepository _repository;

        [OneTimeSetUp]
        public void Init()
        {
            TestInitializer.ClearDb();
            TestInitializer.SeedData(TestData.GenerateSubjects(true));
        }

        [SetUp]
        public void SetUp()
        {
            _repository = TestInitializer.ServiceProvider.GetService<ISubjectIndexRepository>();
        }

        [Test,Order(1)]
        public void should_Get_Count()
        {
            var count = _repository.GetRecordCount().Result;
            Assert.True((count > 0));
            Log.Debug($"{count} Records");
        }

        [Test,Order(1)]
        public void should_Read_Paged()
        {
            var top5 = _repository.Read(1, 5).Result.ToList();
            Assert.True((top5.Count == 5));

            var bottom5 = _repository.Read(2, 5).Result.ToList();
            Assert.True((bottom5.Count == 5));

        }

        [Test,Order(1)]
        public void should_Read_Site_Paged()
        {
            var bySitePg1 = _repository.Read(1, 3,13165).Result.ToList();
            Assert.True((bySitePg1.Count == 3));
            var bySitePg2 = _repository.Read(2, 3,13165).Result.ToList();
            Assert.True((bySitePg2.Count == 2));
        }

        [Test,Order(1)]
        public void should_Get_Block_Count()
        {
            // 2 F 1996 13165
            var subject = TestData.GenerateSubject();

            var count = _repository.GetBlockRecordCount(subject,ScanLevel.Site).Result;
            Assert.True((count > 0));
            Log.Debug($"{count} Records");
        }

        [Test,Order(1)]
        public void should_Read_Block_Paged()
        {
            // 2 F 1996 13165
            var subject = TestData.GenerateSubject();

            var data = _repository.ReadBlock(1,5,subject,ScanLevel.Site).Result;
            Assert.True(data.Count==2);
        }

        [Test, Order(1)]
        public void should_Save_Indices()
        {
            var indices = TestData.GenerateSubjects(false, 2);
            var Ids = indices.Select(x => x.Id).ToArray();
            _repository.CreateOrUpdate(indices);

            var data = _repository.GetConnection()
                .ExecuteScalar<int>($"SELECT COUNT(ID) FROM {nameof(BotContext.SubjectIndices)} WHERE Id IN @Ids",
                    new {Ids});

            Assert.True(data == 2);
        }

        [Test,Order(1)]
        public void should_Save_Scores()
        {
            var scores = TestData.GenerateSubjectScores();
            var Ids = scores.Select(x => x.Id).ToArray();
            _repository.CreateOrUpdateScores(scores);

            var data = _repository.GetConnection()
                .ExecuteScalar<int>($"SELECT COUNT(ID) FROM {nameof(BotContext.SubjectIndexScores)} WHERE Id IN @Ids",
                    new {Ids});

            Assert.True(data == 2);
        }

        [Test,Order(1)]
        public void should_Save_Stages()
        {
            var stages = TestData.GenerateSubjectStages();
            var Ids = stages.Select(x => x.Id).ToArray();
            _repository.CreateOrUpdateStages(stages);

            var data = _repository.GetConnection()
                .ExecuteScalar<int>($"SELECT COUNT(ID) FROM {nameof(BotContext.SubjectIndexStages)} WHERE Id IN @Ids",
                    new {Ids});

            Assert.True(data == 2);
        }

        [Test,Order(99)]
        public void should_Clear()
        {
            _repository.Clear().Wait();

            var data = _repository.GetConnection()
                .ExecuteScalar<int>($"SELECT COUNT(ID) FROM {nameof(BotContext.SubjectIndices)}");

            Assert.True(data==0);
        }

    }
}
