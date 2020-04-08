using CSharpFunctionalExtensions;
using MediatR;

namespace Dwapi.Bot.Core.Application.Indices.Commands
{
    public class RefreshIndex:IRequest<Result>
    {
        public int BatchSize { get; }

        public RefreshIndex(int batchSize)
        {
            BatchSize = batchSize;
        }
    }
}
