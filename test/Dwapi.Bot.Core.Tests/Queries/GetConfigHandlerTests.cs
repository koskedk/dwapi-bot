using Dwapi.Bot.Core.Application.Configs.Queries;
using MediatR;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Serilog;

namespace Dwapi.Bot.Core.Tests.Queries
{
    [TestFixture]
    public class GetConfigHandlerTests
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
        public void should_Refresh()
        {
            var getConfig = new GetConfig();

            var result = _mediator.Send(getConfig).Result;
            Assert.True(result.IsSuccess);
            Assert.True(result.Value.Any());
            Log.Debug($"{result.Value.Count()} Records");
        }
    }
}

