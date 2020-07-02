using System.Collections.Generic;
using System.Linq;
using Dwapi.Bot.Core.Application.Indices.Commands;
using Dwapi.Bot.Core.Application.Matching.Commands;
using Dwapi.Bot.Core.Domain.Indices;
using Dwapi.Bot.Infrastructure;
using Dwapi.Bot.SharedKernel.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Dwapi.Bot.Core.Tests.Commands
{
    [TestFixture]
    public class ScanSubjectHandlerTests
    {
        private IMediator _mediator;
        private BotContext _context;

        [OneTimeSetUp]
        public void Init()
        {
            TestInitializer.ClearDb();
            _mediator = TestInitializer.ServiceProvider.GetService<IMediator>();
            var refreshResult=_mediator.Send(new RefreshIndex(100)).Result;
            var command = new BlockIndex();
            var result = _mediator.Send(command).Result;
            Assert.True(result.IsSuccess);
            Assert.True(refreshResult.IsSuccess);
        }

        [SetUp]
        public void SetUp()
        {
            _context = TestInitializer.ServiceProvider.GetService<BotContext>();
        }

        [Test]
        public void should_Scan_PKV_Site()
        {
            var command = new ScanSubject(ScanLevel.Site);
            var result = _mediator.Send(command).Result;
            Assert.True(result.IsSuccess);
            var indices    = Indices().Where(x => x.SiteCode == 13165).ToList();
            var scores = indices.SelectMany(x => x.IndexScores).ToList();
            Assert.True(scores.Any());
            PrintScores(13165);
        }


        [Test]
        public void should_Scan_Serial_Site()
        {
            var command = new ScanSubject(ScanLevel.Site, SubjectField.Serial);
            var result = _mediator.Send(command).Result;
            Assert.True(result.IsSuccess);
            var indices    = Indices().Where(x => x.SiteCode == 13165).ToList();
            var scores = indices.SelectMany(x => x.IndexScores).ToList();
            Assert.True(scores.Any());
            PrintScores(13165);
        }

        [Test]
        public void should_Scan_PKV_Inter_Site()
        {
            var command = new ScanSubject(ScanLevel.InterSite);
            var result = _mediator.Send(command).Result;
            Assert.True(result.IsSuccess);
            var scores = Indices().SelectMany(x => x.IndexScores).ToList();
            Assert.True(scores.Any());
            PrintScores();
        }

        [Test]
        public void should_Scan_Serial_Inter_Site()
        {
            var command = new ScanSubject(ScanLevel.InterSite,SubjectField.Serial);
            var result = _mediator.Send(command).Result;
            Assert.True(result.IsSuccess);
            var scores = Indices().SelectMany(x => x.IndexScores).ToList();
            Assert.True(scores.Any());
            PrintScores();
        }

        private List<SubjectIndex> Indices()
        {
            return _context.SubjectIndices.AsNoTracking()
                .Include(s => s.IndexScores)
                .Include(s => s.IndexStages)
                .ToList();
        }

        private void PrintScores(int siteCode=0)
        {
            var indices = Indices().ToList();

            if (siteCode > 0)
                indices = Indices().Where(x => x.SiteCode == 13165).ToList();

            foreach (var subjectIndex in indices)
            {
                if (subjectIndex.IndexScores.Any())
                {
                    Log.Debug($"{subjectIndex}");
                    foreach (var score in subjectIndex.IndexScores)
                    {
                        Log.Debug($"   >> {score}");
                    }
                }
            }
        }
    }
}
