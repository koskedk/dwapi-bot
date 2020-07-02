using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Dwapi.Bot.Core.Algorithm.JaroWinkler;
using Dwapi.Bot.Core.Application.Matching.Events;
using Dwapi.Bot.Core.Domain.Configs;
using Dwapi.Bot.Core.Domain.Indices;
using Dwapi.Bot.SharedKernel.Enums;
using Dwapi.Bot.SharedKernel.Utility;
using MediatR;
using Serilog;

namespace Dwapi.Bot.Core.Application.Matching.Commands
{
    public class ScanSubject : IRequest<Result>
    {
        public int Size { get; }
        public int BlockSize { get; }
        public SubjectField Field { get; }

        public ScanSubject(SubjectField field = SubjectField.PKV, int size = 500, int blockSize = 500)
        {
            Size = size;
            BlockSize = blockSize;
            Field = field;
        }
    }

    public class ScanSubjectHandler : IRequestHandler<ScanSubject, Result>
    {
        private readonly IMediator _mediator;
        private readonly ISubjectIndexRepository _repository;
        private readonly IMatchConfigRepository _configRepository;
        private readonly IJaroWinklerScorer _scorer;

        public ScanSubjectHandler(IMediator mediator, ISubjectIndexRepository repository, IJaroWinklerScorer scorer,
            IMatchConfigRepository configRepository)
        {
            _mediator = mediator;
            _repository = repository;
            _scorer = scorer;
            _configRepository = configRepository;
        }

        public async Task<Result> Handle(ScanSubject request, CancellationToken cancellationToken)
        {
            Log.Debug($"scanning Within Sites ...");

            var configs = _configRepository.GetConfigs().ToList();

            try
            {
                var blocks = await _repository.GetSiteBlocks();
                var siteBlocks = blocks.ToList();

                int blockCount = 1;
                Log.Debug($"Scanning blocks {siteBlocks.Count}...");
                var tasks = new List<Task>();
                foreach (var siteBlock in siteBlocks)
                {
                    var task = CreateTask(request, siteBlock, blockCount, siteBlocks.Count, configs, cancellationToken);
                    tasks.Add(task);
                    blockCount++;
                }

                if (tasks.Any())
                    await Task.WhenAll(tasks.ToArray());

                return Result.Ok();
            }
            catch (Exception e)
            {
                Log.Error(e, $"{nameof(ScanSubjectHandler)} Error");
                return Result.Failure(e.Message);
            }
        }

        private async Task CreateTask(ScanSubject request, Guid siteBlock, int blockCount, int siteBlocksCount,
            List<MatchConfig> configs, CancellationToken cancellationToken)
        {
            Log.Debug($"Scanning Block {blockCount}/{siteBlocksCount}...");

            int page = 1;
            var totalRecords = await _repository.GetRecordCount(ScanLevel.Site, siteBlock);

            var pageCount = Custom.PageCount(request.Size, totalRecords);

            while (page <= pageCount)
            {
                // Subjects
                var subjectIndices = await _repository.Read(page, request.Size, ScanLevel.Site, siteBlock);
                var subjects = subjectIndices.ToList();
                int subIndex = 0;
                var subjectsCount = subjects.Count;

                foreach (var subject in subjects)
                {
                    subIndex++;
                    var scores = new List<SubjectIndexScore>();

                    // SCORE

                    var otherSubjects = subjects.Where(x => x.Id != subject.Id);

                    foreach (var otherSubject in otherSubjects)
                    {
                        var score = SubjectIndexScore.GenerateScore(subject, otherSubject, ScanLevel.Site,
                            _scorer, request.Field, subject.SiteCode.ToString(), configs);
                        scores.Add(score);
                    }

                    await _repository.Merge<SubjectIndexScore, Guid>(scores);
                }

                Log.Debug($"    --------------------------------------------------------");
                Log.Debug($"    --------------------------------------------------------");
                Log.Debug(
                    $"    Scanning Block {blockCount}/{siteBlocksCount} {Custom.GetPerc(page, pageCount)}% [{subjectsCount}]");
                Log.Debug($"    --------------------------------------------------------");
                Log.Debug($"    --------------------------------------------------------");
                page++;
            }

            await _mediator.Publish(new BlockScanned(siteBlock, ScanLevel.Site, ScanStatus.Scanned),
                cancellationToken);

            await _mediator.Publish(new BlockUpdated(ScanLevel.Site),
                cancellationToken);
        }
    }
}
