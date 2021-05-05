using System.Linq;
using CSharpFunctionalExtensions;
using Dwapi.Bot.Core.Application.Catalogs.Commands;
using Dwapi.Bot.Infrastructure;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Dwapi.Bot.Core.Tests.Application.Catalogs.Commands
{
    [TestFixture]
    public class MarkPreferredSubjectsTests
    {
        private IMediator _mediator;
        private Result _siteResult,_subjectResult;

        [OneTimeSetUp]
        public void Init()
        {
            TestInitializer.ClearDb();
        }

        [SetUp]
        public void SetUp()
        {
            _mediator = TestInitializer.ServiceProvider.GetService<IMediator>();
            _siteResult = _mediator.Send(new LoadSites()).Result;
            _subjectResult = _mediator.Send(new LoadSubjects("testJobId")).Result;
        }

        [Test]
        public void should_Prefer_Subjects()
        {
            var command = new MarkPreferredSubjects("testLoadJobId");
            var result = _mediator.Send(command).Result;
            Assert.True(result.IsSuccess);

            var context = TestInitializer.ServiceProvider.GetService<BotCleanerContext>();
            var preferred = context.Subjects.OrderByDescending(x => x.Created).First();
            var others = context.Subjects.Where(x => x.SiteId == preferred.SiteId && x.PatientPk == preferred.PatientPk)
                .ToList().OrderBy(x=>x.Created).ToList();
            Assert.True(preferred.IsPreffed);
            Assert.False(others.First().IsPreffed);
            Assert.True(others.Last().IsPreffed);
        }
    }
}
