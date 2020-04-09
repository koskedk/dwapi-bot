using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CSharpFunctionalExtensions;
using Dwapi.Bot.Core.Application.Indices.Events;
using Dwapi.Bot.Core.Domain.Indices;
using Dwapi.Bot.Core.Domain.Readers;
using MediatR;
using Serilog;

namespace Dwapi.Bot.Core.Application.Indices.Commands.Handlers
{
    public class ScanInterSiteHandler : IRequestHandler<ScanInterSite, Result>
    {
        private readonly IMediator _mediator;
        private readonly IPatientIndexRepository _repository;

        public ScanInterSiteHandler(IMediator mediator, IPatientIndexRepository repository)
        {
            _mediator = mediator;
            _repository = repository;
        }

        public async Task<Result> Handle(ScanInterSite request, CancellationToken cancellationToken)
        {
            Log.Debug("refreshing patient index...");
            try
            {
                int page = 1;
                var totalRecords = await _repository.GetRecordCount();

                if(totalRecords==0)
                    return Result.Ok();

                var pageCount = _repository.PageCount( request.BatchSize,totalRecords);

                while (page <= pageCount)
                {
                    var mpis = await _repository.Read(page, request.BatchSize);

                    // SCORE

                    await _mediator.Publish(new InterSiteScanned(mpis.Count, totalRecords));

                    page++;
                }
                return Result.Ok();
            }
            catch (Exception e)
            {
                Log.Error(e, $"{nameof(ScanInterSiteHandler)} Error");
                return Result.Fail(e.Message);
            }
        }
    }
}
