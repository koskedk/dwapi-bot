using Dwapi.Bot.Core.Application.Indices.Commands;
using Dwapi.Bot.Core.Tests.TestArtifacts;
using Dwapi.Bot.SharedKernel.Enums;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Dwapi.Bot.Core.Tests.Commands
{
    [TestFixture]
    public class ClearIndexHandlerTests
    {
        private IMediator _mediator;

        [OneTimeSetUp]
        public void Init()
        {
            TestInitializer.ClearDb();
            TestInitializer.SeedData(TestData.GenerateSubjects(true));
        }
        [SetUp]
        public void SetUp()
        {
            _mediator = TestInitializer.ServiceProvider.GetService<IMediator>();
        }

        [Test]
        public void should_Clear()
        {
            var command = new ClearIndex(ScanLevel.Site);

            var result = _mediator.Send(command).Result;
            Assert.True(result.IsSuccess);
        }
    }
}
