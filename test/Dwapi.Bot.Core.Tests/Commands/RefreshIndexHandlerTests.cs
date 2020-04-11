using Dwapi.Bot.Core.Application.Indices.Commands;
using Dwapi.Bot.Infrastructure;
using MediatR;
using NUnit.Framework;
using Microsoft.Extensions.DependencyInjection;

namespace Dwapi.Bot.Core.Tests.Commands
{
    [TestFixture]
    public class RefreshIndexHandlerTests
    {
        private IMediator _mediator;
        private BotContext _context;

        [OneTimeSetUp]
        public void Init()
        {
            TestInitializer.ClearDb();
        }
        [SetUp]
        public void SetUp()
        {
            _mediator = TestInitializer.ServiceProvider.GetService<IMediator>();
            _context= TestInitializer.ServiceProvider.GetService<BotContext>();
        }

        [Test]
        public void should_Refresh()
        {
            var command = new RefreshIndex(5);

            var result = _mediator.Send(command).Result;
            Assert.True(result.IsSuccess);
        }
    }
}
