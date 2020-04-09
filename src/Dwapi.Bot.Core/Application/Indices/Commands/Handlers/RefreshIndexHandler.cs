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
    public class RefreshIndexHandler : IRequestHandler<RefreshIndex, Result>
    {
        private readonly IMediator _mediator;
        private readonly ISubjectIndexRepository _repository;
        private readonly IMasterPatientIndexReader _reader;

        public RefreshIndexHandler(IMediator mediator, ISubjectIndexRepository repository, IMasterPatientIndexReader reader)
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
                int page = 1;
                var totalRecords = await _reader.GetRecordCount();

                if(totalRecords==0)
                    return Result.Ok();

                var pageCount = _reader.PageCount(request.BatchSize, totalRecords);

                while (page <= pageCount)
                {
                    var mpis = await _reader.Read(page, request.BatchSize);

                    var pis= Mapper.Map<List<SubjectIndex>>(mpis);

                    _repository.CreateOrUpdate(pis);

                    await _mediator.Publish(new IndexRefreshed(pis.Count, request.BatchSize));

                    page++;
                }
                return Result.Ok();
            }
            catch (Exception e)
            {
                Log.Error(e, $"{nameof(RefreshIndexHandler)} Error");
                return Result.Fail(e.Message);
            }
        }
    }
}
