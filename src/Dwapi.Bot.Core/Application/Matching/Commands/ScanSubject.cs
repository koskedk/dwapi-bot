using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using Hangfire;
using MediatR;
using Serilog;

namespace Dwapi.Bot.Core.Application.Matching.Commands
{
    public class ScanSubject : IRequest<Result>
    {
        public string JobId { get;  }
        public ScanLevel Level { get; }
        public int Size { get; }
        public int BlockSize { get; }
        public SubjectField Field { get; }

        public ScanSubject(string jobId, ScanLevel level = ScanLevel.Site, SubjectField field = SubjectField.PKV, int size = 500,
            int blockSize = 500)
        {
            Level = level;
            JobId = jobId;
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
            Log.Debug($"scanning Within {request.Level} ...");

            var configs = _configRepository.GetConfigs().ToList();

            try
            {
                var blocks = request.Level == ScanLevel.Site
                    ? await _repository.GetSiteBlocks()
                    : await _repository.GetInterSiteBlocks();

                var siteBlocks = blocks.ToList();

                int blockCount = 1;
                Log.Debug($"Scanning blocks {siteBlocks.Count}...");

                string jobId;
                if (string.IsNullOrWhiteSpace(request.JobId))
                {
                     jobId= BatchJob.StartNew(x =>
                    {
                        foreach (var siteBlock in siteBlocks)
                        {
                            var count = blockCount;
                            x.Enqueue(() => CreateTask(request, siteBlock, count, siteBlocks.Count, configs));
                            blockCount++;
                        }

                    });
                }
                else
                {
                    jobId = BatchJob.ContinueBatchWith(request.JobId, x =>
                    {
                        foreach (var siteBlock in siteBlocks)
                        {
                            var count = blockCount;
                            x.Enqueue(() => CreateTask(request, siteBlock, count, siteBlocks.Count, configs));
                            blockCount++;
                        }

                    });
                }

                Log.Debug($"Scanning scheduled {jobId}...");
                return Result.Ok();
            }
            catch (Exception e)
            {
                Log.Error(e, $"{nameof(ScanSubjectHandler)} Error");
                return Result.Failure(e.Message);
            }
        }

        [DisplayName("Scanning {2}/{3}")]
        public async Task CreateTask(ScanSubject request, Guid siteBlock, int blockCount, int siteBlocksCount,
            List<MatchConfig> configs)
        {
            // Log.Debug($"Scanning {request.Level} Block {blockCount}/{siteBlocksCount}...");

            int page = 1;
            var totalRecords = await _repository.GetRecordCount(request.Level, siteBlock);

            var pageCount = Custom.PageCount(request.Size, totalRecords);

            while (page <= pageCount)
            {
                // Subjects
                var subjectIndices = await _repository.Read(page, request.Size, request.Level, siteBlock);
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
                        var score = SubjectIndexScore.GenerateScore(subject, otherSubject, request.Level,
                            _scorer, request.Field, subject.SiteCode.ToString(), configs);
                        scores.Add(score);
                    }

                    await _repository.Merge<SubjectIndexScore, Guid>(scores);
                }

                // Log.Debug($"Scanning Block {blockCount}/{siteBlocksCount} {Custom.GetPerc(page, pageCount)}% [{subjectsCount}]");
                page++;
            }

            await _mediator.Publish(new BlockScanned(siteBlock, request.Level, ScanStatus.Scanned));

            // await _mediator.Publish(new BlockUpdated(request.Level));
        }
    }
}
