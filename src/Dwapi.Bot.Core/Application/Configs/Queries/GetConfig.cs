using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Dwapi.Bot.Core.Domain.Configs;
using MediatR;

namespace Dwapi.Bot.Core.Application.Configs.Queries
{
    public class GetConfig:IRequest<Result<IEnumerable<MatchConfig>>>
    {

    }
}
