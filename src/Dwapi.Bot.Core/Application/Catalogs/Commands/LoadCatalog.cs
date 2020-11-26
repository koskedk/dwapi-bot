using System;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using MediatR;
using Serilog;

namespace Dwapi.Bot.Core.Application.Catalogs.Commands
{

    public class LoadCatalog: IRequest<Result<string>>
    {

    }

    public class LoadCatalogHandler : IRequestHandler<LoadCatalog, Result<string>>
    {
        public async Task<Result<string>> Handle(LoadCatalog request, CancellationToken cancellationToken)
        {
            string jobId = "";

            Log.Debug("loading Catalog...");
            try
            {

                await Task.CompletedTask;
                return Result.Ok(jobId);
            }
            catch (Exception e)
            {
                Log.Error(e, $"{nameof(LoadCatalog)} Error");
                return Result.Failure<string>(e.Message);
            }
        }
    }
}
