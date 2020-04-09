using CSharpFunctionalExtensions;
using MediatR;

namespace Dwapi.Bot.Core.Application.Indices.Commands
{
    public class ScanInterSite:IRequest<Result>
    {

        public int BatchSize { get;  }
        public ScanInterSite(int batchSize)
        {
            BatchSize = batchSize;
        }

    }
}
