using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Dwapi.Bot.Core.Application.Indices.Events;
using Dwapi.Bot.Core.Domain.Catalogs;
using Dwapi.Bot.Core.Domain.Readers;
using MediatR;
using Serilog;

namespace Dwapi.Bot.Core.Application.Catalogs.Commands
{

    public class LoadSites: IRequest<Result>
    {
        public bool ClearAll  { get; private set; }
        public int[] SiteCodes { get; private set; }=new int[]{};

        public LoadSites()
        {
        }
        public LoadSites(int[] siteCodes)
        {
            SiteCodes = siteCodes;
            ClearAll = false;
        }
        public LoadSites(int[] siteCodes,bool clearAll)
        {
            SiteCodes = siteCodes;
            ClearAll = clearAll;
        }

    }

    public class LoadSitesHandler : IRequestHandler<LoadSites, Result>
    {
        private readonly IDocketReader _reader;
        private readonly ISiteRepository _repository;
        private readonly IMediator _mediator;
        public LoadSitesHandler(IDocketReader reader, ISiteRepository repository, IMediator mediator)
        {
            _reader = reader;
            _repository = repository;
            _mediator = mediator;
        }

        public async Task<Result> Handle(LoadSites request, CancellationToken cancellationToken)
        {
            Log.Debug("loading Catalog...");
            string jobId = string.Empty;

            try
            {
                // Clear Sites
                if (request.SiteCodes.Any() && !request.ClearAll)
                    await _repository.Clear(request.SiteCodes);
                else
                    await _repository.Clear();

                // Load Sites
                var sites = request.SiteCodes.Any()
                    ? await _reader.GetSites(request.SiteCodes)
                    : await _reader.GetSites();
                var loadedSites = sites.ToList();

                if (loadedSites.Any())
                    await _repository.Create<Site, Guid>(loadedSites);

                await _mediator.Publish(new SitesLoaded(loadedSites.Count, jobId));

                return Result.Ok();
            }
            catch (Exception e)
            {
                Log.Error(e, $"{nameof(LoadSites)} Error");
                return Result.Failure(e.Message);
            }
        }
    }
}
