using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Dwapi.Bot.Core.Domain.Configs;
using Dwapi.Bot.SharedKernel.Enums;
using MediatR;
using Serilog;

namespace Dwapi.Bot.Core.Application.Configs.Queries.Handlers
{
    public class GetConfigHandler:IRequestHandler<GetConfig,Result<IEnumerable<MatchConfig>>>
    {
        private readonly IMatchConfigRepository _repository;

        public GetConfigHandler(IMatchConfigRepository repository)
        {
            _repository = repository;
        }

        public Task<Result<IEnumerable<MatchConfig>>> Handle(GetConfig request, CancellationToken cancellationToken)
        {
            try
            {
                var configs =  _repository.GetConfigs()
                    .Where(x=>x.MatchStatus!=MatchStatus.None)
                    .ToList();

                return Task.FromResult(Result.Ok<IEnumerable<MatchConfig>>(configs));
            }
            catch (Exception e)
            {
                Log.Error(e,$"{nameof(GetConfigHandler)} error");
                return Task.FromResult(Result.Failure<IEnumerable<MatchConfig>>(e.Message));
            }
        }
    }
}
