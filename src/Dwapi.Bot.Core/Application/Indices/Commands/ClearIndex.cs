using System;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Dwapi.Bot.Core.Application.Common.Events;
using Dwapi.Bot.Core.Application.Indices.Events;
using Dwapi.Bot.Core.Domain.Indices;
using Dwapi.Bot.Core.Domain.Indices.Dto;
using Dwapi.Bot.SharedKernel.Enums;
using Hangfire;
using MediatR;
using Serilog;

namespace Dwapi.Bot.Core.Application.Indices.Commands
{
    public class ClearIndex : IRequest<Result<string>>
    {
        public ScanLevel Level { get; }

        public ClearIndex(ScanLevel level)
        {
            Level = level;
        }
    }

    public class ClearIndexHandler : IRequestHandler<ClearIndex, Result<string>>
    {
        private readonly IMediator _mediator;
        private readonly ISubjectIndexRepository _repository;

        public ClearIndexHandler(IMediator mediator, ISubjectIndexRepository repository)
        {
            _mediator = mediator;
            _repository = repository;
        }

        public async Task<Result<string>> Handle(ClearIndex request, CancellationToken cancellationToken)
        {
            Log.Debug("Clearing index...");

            try
            {
                var sites = await _repository.GetSubjectSiteDtos();
                var subjectSites = sites.ToList();

                await _mediator.Publish(
                    new EventOccured("GetSites", $"Clearing {subjectSites.Count}", Convert.ToInt64(subjectSites.Count)),
                    cancellationToken);
                
                var initJobId = BatchJob.StartNew(x =>
                {
                    if (!subjectSites.Any())
                    {
                        x.Enqueue(() => Log.Debug("NO SITES"));
                    }
                    else
                    {
                        x.Enqueue(() => CreateInitTask());
                    }

                },$"Initializing {nameof(ClearIndex)} {request.Level}");
                

                var mainJobId = BatchJob.ContinueBatchWith(initJobId,x =>
                {
                    if (!subjectSites.Any())
                    {
                        x.Enqueue(() => Log.Debug("NO SITES"));
                    }
                    else
                    {
                        int count = 1;
                        foreach (var site in subjectSites)
                        {
                            var index = count;
                            x.Enqueue(() => CreateTask(site, index, subjectSites.Count));
                            count++;
                        }
                    }

                },$"{nameof(ClearIndex)} {request.Level}");

                var jobId= BatchJob.ContinueBatchWith(mainJobId,
                    x => { x.Enqueue(() => SendNotification(subjectSites.Count,mainJobId,request.Level)); },
                $"{nameof(ClearIndex)} {request.Level} Notification");

                Log.Debug($"clearing index scheduled {mainJobId}");

                return Result.Ok(jobId);
            }
            catch (Exception e)
            {
                Log.Error(e, $"{nameof(ClearIndex)} Error");
                return Result.Failure<string>(e.Message);
            }
        }
        
        [DisplayName("Initializing Clearing")]
        public async Task CreateInitTask()
        {
            await _repository.InitClear();

        }

        [DisplayName("Clearing {0} {1}/{2} ")]
        public async Task CreateTask(SubjectSiteDto siteDto,int count,int total)
        {
            await _repository.Clear(siteDto.SiteCode);
            await _mediator.Publish(new IndexSiteCleared(siteDto));

        }
        
        public async Task SendNotification(int count,string jobId,ScanLevel level)
        {
            await _mediator.Publish(new IndexCleared(count,jobId,level));
        }
    }
}
