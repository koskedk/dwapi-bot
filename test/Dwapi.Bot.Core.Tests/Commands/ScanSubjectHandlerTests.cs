using Dwapi.Bot.Core.Application.Indices.Commands;
using Dwapi.Bot.Core.Application.Matching.Commands;
using Dwapi.Bot.SharedKernel.Enums;
using MediatR;
using NUnit.Framework;
using Microsoft.Extensions.DependencyInjection;

namespace Dwapi.Bot.Core.Tests.Commands
{
    [TestFixture]
    public class ScanIndexHandlerTests
    {
        private IMediator _mediator;
        private string _jobId;

        [OneTimeSetUp]
        public void Init()
        {
            TestInitializer.ClearDb();
            _mediator = TestInitializer.ServiceProvider.GetService<IMediator>();
            var refreshResult=_mediator.Send(new RefreshIndex(100,string.Empty,ScanLevel.Site)).Result;
            var command = new BlockIndex(string.Empty, ScanLevel.Site);
            var result = _mediator.Send(command).Result;
            _jobId = result.Value;
            Assert.True(result.IsSuccess);
            Assert.True(refreshResult.IsSuccess);
        }

        [Test]
        public void should_Scan_PKV_Site()
        {
            var command = new ScanIndex(_jobId,ScanLevel.Site,SubjectField.PKV,100);
            var result = _mediator.Send(command).Result;
            Assert.True(result.IsSuccess);
        }


        [Test]
        public void should_Scan_Serial_Site()
        {
            var command = new ScanIndex(_jobId,ScanLevel.Site, SubjectField.Serial,100);
            var result = _mediator.Send(command).Result;
            Assert.True(result.IsSuccess);
        }

        [Test]
        public void should_Scan_PKV_Inter_Site()
        {
            var command = new ScanIndex(_jobId,ScanLevel.InterSite,SubjectField.PKV,100);
            var result = _mediator.Send(command).Result;
            Assert.True(result.IsSuccess);
        }

        [Test]
        public void should_Scan_Serial_Inter_Site()
        {
            var command = new ScanIndex(_jobId,ScanLevel.InterSite,SubjectField.Serial,100);
            var result = _mediator.Send(command).Result;
            Assert.True(result.IsSuccess);
        }
    }
}
