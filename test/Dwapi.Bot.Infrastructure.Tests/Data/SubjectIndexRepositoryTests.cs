using System;
using System.Linq;
using Dwapi.Bot.Core.Domain.Indices;
using Dwapi.Bot.Core.Domain.Readers;
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

        [SetUp]
        public void SetUp()
        {
            _repository = TestInitializer.ServiceProvider.GetService<ISubjectIndexRepository>();
        }

        [Test]
        public void should_GetRecordCount()
        {
            var count = _repository.GetRecordCount().Result;
            Assert.True((count > 0));
            Log.Debug($"{count} Records");
        }

        [Test]
        public void should_Read_Paged()
        {
            var top5 = _repository.Read(1, 5).Result.ToList();
            Assert.True((top5.Count == 5));

            var bottom5 = _repository.Read(2, 5).Result.ToList();
            Assert.True((bottom5.Count == 5));
        }
    }
}
