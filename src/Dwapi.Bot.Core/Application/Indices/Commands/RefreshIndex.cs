using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CSharpFunctionalExtensions;
using Dwapi.Bot.Core.Application.Common.Events;
using Dwapi.Bot.Core.Application.Indices.Events;
using Dwapi.Bot.Core.Domain.Indices;
using Dwapi.Bot.Core.Domain.Indices.Dto;
using Dwapi.Bot.Core.Domain.Readers;
using Dwapi.Bot.SharedKernel.Utility;
using MediatR;
using Serilog;

namespace Dwapi.Bot.Core.Application.Indices.Commands
{
    public class RefreshIndex : IRequest<Result>
    {
        public int BatchSize { get; }

        public RefreshIndex(int batchSize)
        {
            BatchSize = batchSize;
        }
    }

    public class RefreshIndexHandler : IRequestHandler<RefreshIndex, Result>
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

        public async Task<Result> Handle(RefreshIndex request, CancellationToken cancellationToken)
        {
            Log.Debug("refreshing patient index...");
            try
            {
                Log.Debug("getting sites...");

                var sites = await _reader.GetMpiSites();
                var mpiSites = sites.ToList();

                await _mediator.Publish(
                    new EventOccured("GetMpiSites", $"MPI Sites Found", Convert.ToInt64(mpiSites.Count)),
                    cancellationToken);

                int siteCount = 1;
                var tasks = new List<Task>();

                foreach (var mpiSite in mpiSites)
                {

                    var task = CreateTask(request, mpiSite, siteCount, mpiSites.Count, cancellationToken);
                    tasks.Add(task);
                    siteCount++;
                }

                if (tasks.Any())
                    Task.WaitAll(tasks.ToArray());

                return Result.Ok();
            }
            catch (Exception e)
            {
                Log.Error(e, $"{nameof(RefreshIndexHandler)} Error");
                return Result.Failure(e.Message);
            }
        }

        private async Task CreateTask(RefreshIndex request, SubjectSiteDto mpiSite, int siteCount, int totalSites,
            CancellationToken cancellationToken)
        {
            await _mediator.Publish(
                new EventOccured("RefreshSites", $"Refreshing {mpiSite.FacilityName}", siteCount, totalSites),
                cancellationToken);

            var totalRecords = await _reader.GetRecordCount(mpiSite.SiteCode);

            if (totalRecords == 0)
                return;

            var pageCount = Custom.PageCount(request.BatchSize, totalRecords);
            int page = 1;
            while (page <= pageCount)
            {
                Log.Debug($"Reading {page} of {pageCount}...");


                var mpis = await _reader.Read(page, request.BatchSize, mpiSite.SiteCode);

                var pis = Mapper.Map<List<SubjectIndex>>(mpis);

                await _repository.CreateOrUpdate(pis);

                await _mediator.Publish(
                    new IndexRefreshed(pis.Count, totalRecords)
                        {Site = mpiSite, SiteCount = siteCount, TotalSiteCount = totalSites}, cancellationToken);

                page++;

                await _mediator.Publish(
                    new EventOccured("RefreshMpi", $"Refreshing {mpiSite.FacilityName} MPI", pis.Count, totalRecords),
                    cancellationToken);
            }
        }
    }
}
