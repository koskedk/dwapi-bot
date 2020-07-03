using Dwapi.Bot.Core.Application.Indices.Commands;
using Dwapi.Bot.Core.Application.Matching.Commands;
using Dwapi.Bot.Core.Tests.TestArtifacts;
using Dwapi.Bot.Infrastructure;
using Dwapi.Bot.SharedKernel.Enums;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Dwapi.Bot.Core.Tests.Commands
{
    [TestFixture]
    public class BlockSubjectHandlerTests
    {
        private IMediator _mediator;
        private BotContext _context;

        [OneTimeSetUp]
        public void Init()
        {
            TestInitializer.ClearDb();
            TestInitializer.SeedData(TestData.GenerateSubjects(true));
        }
        [SetUp]
        public void SetUp()
        {
            _context= TestInitializer.ServiceProvider.GetService<BotContext>();
            _mediator = TestInitializer.ServiceProvider.GetService<IMediator>();
        }

        [Test]
        public void should_Block()
        {
            var command = new BlockSubject();

            var result = _mediator.Send(command).Result;
            Assert.True(result.IsSuccess);
        }

        [Test]
        public void should_Block_Inter()
        {
            var command = new BlockSubject(ScanLevel.InterSite);

            var result = _mediator.Send(command).Result;
            Assert.True(result.IsSuccess);
        }
    }
}
