using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CSharpFunctionalExtensions;
using Dwapi.Bot.Core.Application.Indices.Events;
using Dwapi.Bot.Core.Domain.Indices;
using Dwapi.Bot.Core.Domain.Indices.Dto;
using Dwapi.Bot.Core.Domain.Readers;
using Dwapi.Bot.SharedKernel.Enums;
using Dwapi.Bot.SharedKernel.Utility;
using Hangfire;
using MediatR;
using Serilog;

namespace Dwapi.Bot.Core.Application.Indices.Commands
{
    public class RefreshIndex : IRequest<Result<string>>
    {
        public ScanLevel Level { get; }
        public int BatchSize { get; }
        public string ClearJobId { get; }

        public RefreshIndex(int batchSize, string clearJobId, ScanLevel level)
        {
            BatchSize = batchSize;
            ClearJobId = clearJobId;
            Level = level;
        }
    }

    public class RefreshIndexHandler : IRequestHandler<RefreshIndex, Result<string>>
    {
        private readonly IMediator _mediator;
        private readonly ISubjectIndexRepository _repository;
        private readonly IMasterPatientIndexReader _reader;

        public RefreshIndexHandler(IMediator mediator, ISubjectIndexRepository repository,
            IMasterPatientIndexReader reader)
        {
            _mediator = mediator;
            _repository = repository;
            _reader = reader;
        }

        public async Task<Result<string>> Handle(RefreshIndex request, CancellationToken cancellationToken)
        {
            Log.Debug("refreshing index...");
            try
            {
                Log.Debug("getting sites...");

                var sites = await _reader.GetMpiSites();
                var mpiSites = sites.ToList();

                Log.Debug($"found {mpiSites.Count} site(s)");

                int siteCount = 1;
                var tasks = new List<Task>();
                string mainJobId;

                if (string.IsNullOrWhiteSpace(request.ClearJobId))
                {
                    mainJobId = BatchJob.StartNew(x =>
                    {
                        if (!mpiSites.Any())
                        {
                            x.Enqueue(() => Log.Debug("NO SITES"));
                        }
                        else
                        {
                            foreach (var mpiSite in mpiSites)
                            {
                                var count = siteCount;
                                x.Enqueue(() => CreateTask(request, mpiSite, count, mpiSites.Count, cancellationToken));
                                siteCount++;
                            }
                        }
                    },
                    $"{nameof(RefreshIndex)}");
                }
                else
                {
                    mainJobId = BatchJob.ContinueBatchWith(request.ClearJobId, x =>
                    {
                        if (!mpiSites.Any())
                        {
                            x.Enqueue(() => Log.Debug("NO SITES"));
                        }
                        else
                        {
                            foreach (var mpiSite in mpiSites)
                            {
                                var count = siteCount;
                                x.Enqueue(() => CreateTask(request, mpiSite, count, mpiSites.Count, cancellationToken));
                                siteCount++;
                            }
                        }
                    },$"{nameof(RefreshIndex)} {request.Level}");
                }

                var jobId = BatchJob.ContinueBatchWith(mainJobId,
                    x => { x.Enqueue(() => SendNotification(mpiSites.Count, mainJobId,request.Level)); },
                    $"{nameof(RefreshIndex)} {request.Level} Notification");

                Log.Debug($"refreshing scheduled [{mainJobId}]");

                return Result.Ok(jobId);
            }
            catch (Exception e)
            {
                Log.Error(e, $"{nameof(RefreshIndex)} Error");
                return Result.Failure<string>(e.Message);
            }
        }

        [DisplayName("Refreshing {1} {2}/{3}")]
        public async Task CreateTask(RefreshIndex request, SubjectSiteDto mpiSite, int siteCount, int totalSites,
            CancellationToken cancellationToken)
        {

            var totalRecords = await _reader.GetRecordCount(mpiSite.SiteCode);
            if (totalRecords == 0)
                return;

            var pageCount = Custom.PageCount(request.BatchSize, totalRecords);
            int page = 1;
            int recordCount = 0;

            while (page <= pageCount)
            {
                var masterPatientIndices = await _reader.Read(page, request.BatchSize, mpiSite.SiteCode);

                var subjectIndices = Mapper.Map<List<SubjectIndex>>(masterPatientIndices);
                subjectIndices.Where(x => x.IsInvalidSex())
                    .ToList()
                    .ForEach(p => p.cleanUpSex());
                recordCount += subjectIndices.Count;
                await _repository.Merge<SubjectIndex, Guid>(subjectIndices);
                page++;
            }

            await _mediator.Publish(new IndexSiteRefreshed(mpiSite, recordCount));
        }

        public async Task SendNotification(int count, string jobId, ScanLevel level)
        {
            await _mediator.Publish(new IndexRefreshed(count, jobId, level));
        }
    }
}
