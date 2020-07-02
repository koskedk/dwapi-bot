using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Dwapi.Bot.Core.Application.Common.Events;
using Dwapi.Bot.Core.Application.Indices.Events;
using Dwapi.Bot.Core.Domain.Indices;
using Dwapi.Bot.Core.Domain.Indices.Dto;
using Dwapi.Bot.Core.Domain.Readers;
using Dwapi.Bot.SharedKernel.Enums;
using Hangfire;
using MediatR;
using Serilog;

namespace Dwapi.Bot.Core.Application.Indices.Commands
{
    public class BlockIndex:IRequest<Result>
    {
        public ScanLevel Level { get; set; }

        public BlockIndex(ScanLevel level = ScanLevel.Site)
        {
            Level = level;
        }
    }

    public class BlockIndexHandler : IRequestHandler<BlockIndex, Result>
    {
        private readonly IMediator _mediator;
        private readonly ISubjectIndexRepository _repository;

        public BlockIndexHandler(IMediator mediator, ISubjectIndexRepository repository)
        {
            _mediator = mediator;
            _repository = repository;
        }

        public async Task<Result> Handle(BlockIndex request, CancellationToken cancellationToken)
        {
            Log.Debug("Blocking index...");
            try
            {
                IEnumerable<SubjectBlockDto> blocks;

                if (request.Level == ScanLevel.Site)
                {
                    blocks = await _repository.GetSubjectSiteBlockDtos();
                }
                else
                {
                    blocks = await _repository.GetSubjectInterSiteBlockDtos();
                }

                var blockSites = blocks.ToList();

               var jobId= BatchJob.StartNew(x =>
                {
                    foreach (var site in blockSites)
                    {
                        if (request.Level == ScanLevel.Site)
                        {
                            x.Enqueue(() => BlockSiteIndex(site));
                        }
                        else
                        {
                            x.Enqueue(() => BlockInterSiteIndex(site));
                        }
                    }
                });

               var id = BatchJob.ContinueBatchWith(jobId,
                   x => { x.Enqueue(() => SendNotification(blockSites.Count)); });

               Log.Debug($"blocking index scheduled {jobId}");

                return Result.Ok();
            }
            catch (Exception e)
            {
                Log.Error(e, $"{nameof(BlockIndexHandler)} Error");
                return Result.Failure(e.Message);
            }
        }

        private async Task BlockSiteIndex(SubjectBlockDto siteDto)
        {
            await _repository.BlockSiteSubjects(siteDto);
            await _mediator.Publish(new IndexSiteBlocked(siteDto));
        }

        private async Task BlockInterSiteIndex(SubjectBlockDto siteDto)
        {
            await _repository.BlockInterSiteSubjects(siteDto);
            await _mediator.Publish(new IndexSiteBlocked(siteDto));
        }

        public async Task SendNotification(int count)
        {
            await _mediator.Publish(new IndexBlocked(count));
        }
    }
}
