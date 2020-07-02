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
using Dwapi.Bot.SharedKernel.Enums;
using Dwapi.Bot.SharedKernel.Utility;
using MediatR;
using Serilog;

namespace Dwapi.Bot.Core.Application.Indices.Commands
{
    public class ClearIndex : IRequest<Result>
    {
    }

    public class ClearIndexHandler : IRequestHandler<ClearIndex, Result>
    {
        private readonly IMediator _mediator;
        private readonly ISubjectIndexRepository _repository;
        private readonly IBlockStageRepository _blockStageRepository;
        private readonly IMasterPatientIndexReader _reader;

        public ClearIndexHandler(IMediator mediator, ISubjectIndexRepository repository, IMasterPatientIndexReader reader, IBlockStageRepository blockStageRepository)
        {
            _mediator = mediator;
            _repository = repository;
            _reader = reader;
            _blockStageRepository = blockStageRepository;
        }

        public async Task<Result> Handle(ClearIndex request, CancellationToken cancellationToken)
        {
            Log.Debug("Clearing patient index...");
            try
            {

                var sites =await  _repository.GetSubjectSiteDtos();
                var subjectSites = sites.ToList();

                await _mediator.Publish(new EventOccured("GetSites", $"Clearing {subjectSites.Count}", Convert.ToInt64(subjectSites.Count)),cancellationToken);

                var tasks=new List<Task>();

                foreach (var site in subjectSites)
                {
                    var task = ClearIndex(site,subjectSites.Count);
                    tasks.Add(task);
                }

                if (tasks.Any())
                    await Task.WhenAll(tasks.ToArray());


                Log.Debug("clearing patient index done");

                return Result.Ok();
            }
            catch (Exception e)
            {
                Log.Error(e, $"{nameof(ClearIndexHandler)} Error");
                return Result.Failure(e.Message);
            }
        }

        private async Task ClearIndex(SubjectSiteDto siteDto,int count)
        {
            await _repository.Clear(siteDto.SiteCode);
            await _mediator.Publish(new IndexSiteCleared(siteDto,count));
        }
    }
}
