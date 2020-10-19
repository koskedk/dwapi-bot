using Dwapi.Bot.Core.Application.Indices.Commands;
using Dwapi.Bot.SharedKernel.Enums;
using MediatR;
using NUnit.Framework;
using Microsoft.Extensions.DependencyInjection;

namespace Dwapi.Bot.Core.Tests.Commands
{
    [TestFixture]
    public class RefreshIndexHandlerTests
    {
        private IMediator _mediator;
        private string _jobId;

        [OneTimeSetUp]
        public void Init()
        {
            TestInitializer.ClearDb();
            _mediator = TestInitializer.ServiceProvider.GetService<IMediator>();
            var command = new ClearIndex(ScanLevel.Site,string.Empty);
            var result = _mediator.Send(command).Result;
            _jobId = result.Value;
        }

        [Test]
        public void should_Refresh()
        {
            var command = new RefreshIndex(5, _jobId,ScanLevel.Site,string.Empty);

            var result = _mediator.Send(command).Result;
            Assert.True(result.IsSuccess);
        }
    }
}
