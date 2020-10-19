using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Dwapi.Bot.Core.Application.Matching.Events;
using Dwapi.Bot.Core.Domain.Indices;
using Dwapi.Bot.Core.Domain.Indices.Dto;
using Dwapi.Bot.SharedKernel.Enums;
using Hangfire;
using MediatR;
using Serilog;

namespace Dwapi.Bot.Core.Application.Matching.Commands
{
    public class BlockIndex : IRequest<Result<string>>
    {
        public ScanLevel Level { get; }
        public string JobId { get; }
        public bool NotifyNextLevel { get; }
        public bool OnNotify{ get; }

        public BlockIndex(string jobId, ScanLevel level, bool notifyNextLevel = false,bool onNotify=false)
        {
            JobId = jobId;
            Level = level;
            NotifyNextLevel = notifyNextLevel;
            OnNotify = onNotify;
        }
    }

    public class BlockIndexHandler : IRequestHandler<BlockIndex, Result<string>>
    {
        private readonly IMediator _mediator;
        private readonly ISubjectIndexRepository _repository;

        public BlockIndexHandler(IMediator mediator, ISubjectIndexRepository repository)
        {
            _mediator = mediator;
            _repository = repository;
        }

        public async Task<Result<string>> Handle(BlockIndex request, CancellationToken cancellationToken)
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

                var blockSites = blocks.ToList().Where(x => x.IsValid).ToList();

               var mainJobId= BatchJob.StartNew(x =>
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
               },
                   $"{nameof(BlockIndex)} {request.Level}");

                var jobId=  BatchJob.ContinueBatchWith(mainJobId,
                   x => { x.Enqueue(() => SendNotification(blockSites.Count,mainJobId,request.Level,request.NotifyNextLevel,request.OnNotify)); },
                   $"{nameof(BlockIndex)} {request.Level} Notification");

               Log.Debug($"blocking subject scheduled {mainJobId}");

                return Result.Ok(jobId);
            }
            catch (Exception e)
            {
                Log.Error(e, $"{nameof(BlockIndex)} Error");
                return Result.Failure<string>(e.Message);
            }
        }

        [DisplayName("Blocking {1}/{2} ")]
        [DisableConcurrentExecution(timeoutInSeconds: 10 * 60)]
        public async Task BlockSiteIndex(SubjectBlockDto siteDto,int count,int total)
        {
            await _repository.BlockSiteSubjects(siteDto);
            await _mediator.Publish(new IndexSiteBlocked(siteDto));
        }

        [DisplayName("Blocking-Inter {1}/{2} ")]
        [DisableConcurrentExecution(timeoutInSeconds: 10 * 60)]
        public async Task BlockInterSiteIndex(SubjectBlockDto siteDto,int count,int total)
        {
            await _repository.BlockInterSiteSubjects(siteDto);
            await _mediator.Publish(new IndexSiteBlocked(siteDto));
        }

        public async Task SendNotification(int count,string jobId, ScanLevel scanLevel,bool notifyLevel,bool onNotify)
        {
            if (notifyLevel)
            {
                await _mediator.Publish(new PartIndexBlocked(count,jobId, scanLevel));
                return;
            }

            if (onNotify)
                scanLevel = ScanLevel.Both;

            await _mediator.Publish(new IndexBlocked(count,jobId, scanLevel));
        }
    }
}
