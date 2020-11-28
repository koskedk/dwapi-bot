using System.Linq;
using Dwapi.Bot.Core.Application.Catalogs.Commands;
using Dwapi.Bot.Infrastructure;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Dwapi.Bot.Core.Tests.Commands
{
    [TestFixture]
    public class LoadSiteTests
    {
        private IMediator _mediator;

        [OneTimeSetUp]
        public void Init()
        {
            TestInitializer.ClearDb();
        }

        [SetUp]
        public void SetUp()
        {
            _mediator = TestInitializer.ServiceProvider.GetService<IMediator>();
        }

        [Test]
        public void should_Load_Sites()
        {
            var command = new LoadSites();
            var result = _mediator.Send(command).Result;
            Assert.True(result.IsSuccess);

            var context = TestInitializer.ServiceProvider.GetService<BotCleanerContext>();
            Assert.True(context.Sites.Any());
        }
    }
}
