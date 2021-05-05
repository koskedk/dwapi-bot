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
    public class MarkPreferredExtractsTests
    {
        private IMediator _mediator;
        private Result _siteResult,_subjectResult,_prefSubjResult;

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
            _prefSubjResult =  _mediator.Send(new MarkPreferredSubjects("testLoadJobId")).Result;
        }

        [Test]
        public void should_Prefer_Extracts()
        {
            var command = new MarkPreferredExtracts("testLoadJobId");
            var result = _mediator.Send(command).Result;
            Assert.True(result.IsSuccess);

            var context = TestInitializer.ServiceProvider.GetService<BotCleanerContext>();
            var preferredExtracts = context.Extracts.Where(x => x.CandidatePatientId.HasValue).ToList();
            Assert.True(preferredExtracts.Any());
        }
    }
}
