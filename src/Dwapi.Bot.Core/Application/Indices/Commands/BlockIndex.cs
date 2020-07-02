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
        private readonly IBlockStageRepository _blockStageRepository;
        private readonly IMasterPatientIndexReader _reader;

        public BlockIndexHandler(IMediator mediator, ISubjectIndexRepository repository, IMasterPatientIndexReader reader, IBlockStageRepository blockStageRepository)
        {
            _mediator = mediator;
            _repository = repository;
            _reader = reader;
            _blockStageRepository = blockStageRepository;
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

                await _mediator.Publish(new EventOccured("GetBlocks", $"Blocking", Convert.ToInt64(blockSites.Count)),
                    cancellationToken);

                var tasks = new List<Task>();

                foreach (var site in blockSites)
                {
                    if (request.Level == ScanLevel.Site)
                    {
                        var task = BlockSiteIndex(site);
                        tasks.Add(task);
                    }
                    else
                    {
                        var task = BlockInterSiteIndex(site);
                        tasks.Add(task);
                    }
                }

                if (tasks.Any())
                    await Task.WhenAll(tasks.ToArray());

                await _blockStageRepository.InitBlock(request.Level);

                Log.Debug("blocking done");

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
    }
}
