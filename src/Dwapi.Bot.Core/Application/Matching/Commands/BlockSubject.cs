using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Dwapi.Bot.Core.Application.Indices.Events;
using Dwapi.Bot.Core.Application.Matching.Events;
using Dwapi.Bot.Core.Domain.Indices;
using Dwapi.Bot.Core.Domain.Indices.Dto;
using Dwapi.Bot.SharedKernel.Enums;
using Hangfire;
using MediatR;
using Serilog;

namespace Dwapi.Bot.Core.Application.Matching.Commands
{
    public class BlockSubject:IRequest<Result<string>>
    {
        public ScanLevel Level { get; }

        public BlockSubject(ScanLevel level = ScanLevel.Site)
        {
            Level = level;
        }
    }

    public class BlockSubjectHandler : IRequestHandler<BlockSubject, Result<string>>
    {
        private readonly IMediator _mediator;
        private readonly ISubjectIndexRepository _repository;

        public BlockSubjectHandler(IMediator mediator, ISubjectIndexRepository repository)
        {
            _mediator = mediator;
            _repository = repository;
        }

        public async Task<Result<string>> Handle(BlockSubject request, CancellationToken cancellationToken)
        {
            Log.Debug("Blocking subject...");
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
                   int count = 1;
                   if (!blockSites.Any())
                   {
                       x.Enqueue(() => Log.Debug("NO BLOCKS"));
                   }
                   else
                   {
                       foreach (var site in blockSites)
                       {
                           var index = count;
                           if (request.Level == ScanLevel.Site)
                           {
                               x.Enqueue(() => BlockSiteIndex(site, index, blockSites.Count));
                           }
                           else
                           {
                               x.Enqueue(() => BlockInterSiteIndex(site, index, blockSites.Count));
                           }

                           count++;
                       }
                   }
               });

               BatchJob.ContinueBatchWith(jobId,
                   x => { x.Enqueue(() => SendNotification(blockSites.Count)); });

               Log.Debug($"blocking subject scheduled {jobId}");

                return Result.Ok(jobId);
            }
            catch (Exception e)
            {
                Log.Error(e, $"{nameof(BlockSubjectHandler)} Error");
                return Result.Failure<string>(e.Message);
            }
        }

        [DisplayName("Blocking {1}/{2} ")]
        public async Task BlockSiteIndex(SubjectBlockDto siteDto,int count,int total)
        {
            await _repository.BlockSiteSubjects(siteDto);
            await _mediator.Publish(new AreaBlocked(siteDto));
        }

        [DisplayName("Blocking-Inter {1}/{2} ")]
        public async Task BlockInterSiteIndex(SubjectBlockDto siteDto,int count,int total)
        {
            await _repository.BlockInterSiteSubjects(siteDto);
            await _mediator.Publish(new AreaBlocked(siteDto));
        }

        public async Task SendNotification(int count)
        {
            await _mediator.Publish(new SubjectBlocked(count));
        }
    }
}
