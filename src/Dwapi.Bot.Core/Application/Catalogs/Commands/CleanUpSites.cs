using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Dwapi.Bot.Core.Domain.Catalogs;
using Dwapi.Bot.Core.Domain.Readers;
using MediatR;
using Serilog;

namespace Dwapi.Bot.Core.Application.Catalogs.Commands
{

    public class CleanUpSites : IRequest<Result>
    {
        public int[] SiteCodes { get; private set; } = new int[] { };

        public CleanUpSites()
        {
        }

        public CleanUpSites(int[] siteCodes)
        {
            SiteCodes = siteCodes;
        }
    }

    public class CleanUpSitesHandler : IRequestHandler<CleanUpSites, Result>
    {
        private readonly IDocketReader _reader;
        private readonly ISiteRepository _repository;
        private readonly IMediator _mediator;
        public CleanUpSitesHandler(IDocketReader reader, ISiteRepository repository, IMediator mediator)
        {
            _reader = reader;
            _repository = repository;
            _mediator = mediator;
        }

        // TODO: Cleaup sites review
        public async Task<Result> Handle(CleanUpSites request, CancellationToken cancellationToken)
        {
            Log.Debug("loading Catalog...");
            string jobId = string.Empty;

            try
            {
                // Get Sites
                var sites = request.SiteCodes.Any()
                    ? _repository.GetAll<Site, Guid>(x => request.SiteCodes.Contains(x.Code))
                    : _repository.GetAll<Site, Guid>();

                // Clear Sites

                foreach (var site in sites)
                {
                    // Cleanup Extracts

                    var siteSubjects = _repository.GetAll<Subject, Guid>().Where(x => x.SiteId == site.Id).ToList();

                    foreach (var subjectsGroup in siteSubjects.GroupBy(x => x.PreferredPatientId))
                    {
                        var groupPatientIds = subjectsGroup.Select(x => x.PatientId).ToList();

                        var subjectExtracts = _repository.GetExtracts(site.Id,groupPatientIds )
                            .ToList();

                        if (subjectExtracts.Any())
                        {
                            foreach (var extractsGroup in subjectExtracts.GroupBy(x => x.Extract))
                            {
                                // set  Candidate

                                var candidateExtract = extractsGroup.ToList().FirstOrDefault(x => x.CandidatePatientId.HasValue);

                                if (null != candidateExtract)
                                {
                                    await _reader.CleanExtract(extractsGroup.Key,candidateExtract.CandidatePatientId.Value,  groupPatientIds,
                                        extractsGroup.Select(x => x.ExtractId).ToList());
                                }
                            }
                        }

                        // Cleanup Patients

                        await _reader.CleanSubject(groupPatientIds);
                    }

                }


                if (request.SiteCodes.Any())
                    await _repository.Clear(request.SiteCodes);
                else
                    await _repository.Clear();

                // Load Sites

                var loadedSites = sites.ToList();

                if (loadedSites.Any())
                    await _repository.Create<Site, Guid>(loadedSites);

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
