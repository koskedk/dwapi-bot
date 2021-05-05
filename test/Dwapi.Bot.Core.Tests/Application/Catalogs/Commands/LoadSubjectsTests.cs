using System.Collections.Generic;
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
    public class LoadSubjectsTests
    {
        private IMediator _mediator;
        private Result _siteResult;

        private List<string> _extracts = new List<string>
        {
            "PatientArtExtract",
            "PatientBaselinesExtract",
            "PatientStatusExtract",
            "PatientAdverseEventExtract",
            "PatientArtExtract",
            "PatientPharmacyExtract",
            "PatientLaboratoryExtract",
            "PatientVisitExtract"
        };

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
        }

        [Test]
        public void should_LoadSubjects()
        {
            var command = new LoadSubjects("testJobId");

            var result = _mediator.Send(command).Result;
            Assert.True(result.IsSuccess);

            var context = TestInitializer.ServiceProvider.GetService<BotCleanerContext>();
            Assert.True(context.Subjects.Any());
            _extracts.ForEach(x =>
                Assert.True(context.Extracts.Any(e => e.Extract == x)));
        }
    }
}
