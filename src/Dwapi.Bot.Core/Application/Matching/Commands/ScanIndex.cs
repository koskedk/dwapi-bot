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
    public class ScanIndex : IRequest<Result<string>>
    {
        public string JobId { get;  }
        public ScanLevel Level { get; }
        public int Size { get; }
        public SubjectField Field { get; }

        public ScanIndex(string jobId, ScanLevel level, SubjectField field , int size)
        {
            Level = level;
            JobId = jobId;
            Size = size;
            Field = field;
        }
    }

    public class ScanIndexHandler : IRequestHandler<ScanIndex, Result<string>>
    {
        private readonly IMediator _mediator;
        private readonly ISubjectIndexRepository _repository;
        private readonly IMatchConfigRepository _configRepository;
        private readonly IJaroWinklerScorer _scorer;

        public ScanIndexHandler(IMediator mediator, ISubjectIndexRepository repository, IJaroWinklerScorer scorer,
            IMatchConfigRepository configRepository)
        {
            _mediator = mediator;
            _repository = repository;
            _scorer = scorer;
            _configRepository = configRepository;
        }

        public async Task<Result<string>> Handle(ScanIndex request, CancellationToken cancellationToken)
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

                string mainJobId;
                if (string.IsNullOrWhiteSpace(request.JobId))
                {
                    mainJobId= BatchJob.StartNew(x =>
                    {
                        foreach (var siteBlock in siteBlocks)
                        {
                            var count = blockCount;
                            x.Enqueue(() => CreateTask(request, siteBlock, count, siteBlocks.Count, configs));
                            blockCount++;
                        }

                    },
                        $"{nameof(ScanIndex)} {request.Level}");
                }
                else
                {
                    mainJobId = BatchJob.ContinueBatchWith(request.JobId, x =>
                    {
                        foreach (var siteBlock in siteBlocks)
                        {
                            var count = blockCount;
                            x.Enqueue(() => CreateTask(request, siteBlock, count, siteBlocks.Count, configs));
                            blockCount++;
                        }

                    },
                        $"{nameof(ScanIndex)} {request.Level}");
                }

                var jobId=  BatchJob.ContinueBatchWith(mainJobId,
                    x => { x.Enqueue(() => SendNotification(siteBlocks.Count,mainJobId,request.Level)); },
                    $"{nameof(ScanIndex)} {request.Level} Notification");

                Log.Debug($"Scanning scheduled {jobId}...");
                return Result.Ok(jobId);
            }
            catch (Exception e)
            {
                Log.Error(e, $"{nameof(ScanIndex)} Error");
                return Result.Failure<string>(e.Message);
            }
        }

        [DisplayName("Scanning {2}/{3}")]
        public async Task CreateTask(ScanIndex request, Guid siteBlock, int blockCount, int siteBlocksCount,
            List<MatchConfig> configs)
        {
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
                page++;
            }
            await _mediator.Publish(new IndexSiteScanned(siteBlock, request.Level, ScanStatus.Scanned));
        }

        public async Task SendNotification(int count,string jobId, ScanLevel scanLevel)
        {
            await _mediator.Publish(new IndexScanned(count,jobId, scanLevel));
        }
    }
}
