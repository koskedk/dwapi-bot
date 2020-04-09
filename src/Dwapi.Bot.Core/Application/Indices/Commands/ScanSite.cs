using CSharpFunctionalExtensions;
using MediatR;

namespace Dwapi.Bot.Core.Application.Indices.Commands
{
    public class ScanSite:IRequest<Result>
    {
        public int SiteCode { get; }
        public int BatchSize { get;  }

        public ScanSite(int siteCode,int batchSize)
        {
            SiteCode = siteCode;
            BatchSize = batchSize;
        }
    }
}
