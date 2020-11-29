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
    public class MarkPreferredExtracts: IRequest<Result>
    {
        public string LoadJobId { get;  }

        public MarkPreferredExtracts(string loadJobId)
        {
            LoadJobId = loadJobId;
        }
    }

    public class MarkPreferredExtractsHandler : IRequestHandler<MarkPreferredExtracts, Result>
    {
        private readonly IDocketReader _reader;
        private readonly ISiteRepository _repository;

        public MarkPreferredExtractsHandler(IDocketReader reader, ISiteRepository repository)
        {
            _reader = reader;
            _repository = repository;
        }
        // TODO: set preferred extracts
        public async Task<Result> Handle(MarkPreferredExtracts request, CancellationToken cancellationToken)
        {
            Log.Debug("Marking Subject Extracts...");
            string jobId = string.Empty;

            try
            {
                if(string.IsNullOrWhiteSpace(request.LoadJobId))
                    return Result.Ok();

                var siteCatalog = _repository.GetAll<Site, Guid>().ToList();

                foreach (var site in siteCatalog)
                {
                    // Get Subjects

                    var siteSubjects = _repository.GetAll<Subject, Guid>().Where(x => x.SiteId == site.Id).ToList();


                    foreach (var subjectsGroup in siteSubjects.GroupBy(x => x.PreferredPatientId))
                    {
                        //Get extracts for this group

                        var groupPatientIds = subjectsGroup.Select(x => x.PatientId).ToList();

                        var subjectExtracts = _repository
                            .GetExtracts(site.Id,groupPatientIds ).ToList();

                        if (subjectExtracts.Any())
                        {

                            foreach (var extractsGroup in subjectExtracts.GroupBy(x=>x.Extract))
                            {

                                var candidateExtracts = extractsGroup.Where(x => x.PatientId == subjectsGroup.Key).ToList();

                                if (candidateExtracts.Any())
                                {
                                    candidateExtracts.ForEach(x => x.CandidatePatientId = subjectsGroup.Key);
                                }
                                else
                                {
                                    var latestExtract = extractsGroup.OrderByDescending(x => x.Created).First();

                                    candidateExtracts = extractsGroup
                                        .Where(x => x.PatientId == latestExtract.PatientId).ToList();

                                    candidateExtracts.ForEach(x => x.CandidatePatientId = subjectsGroup.Key);
                                }

                                if (candidateExtracts.Any())
                                {
                                    await _repository.Update<SubjectExtract, Guid>(candidateExtracts);
                                }
                            }
                        }
                    }
                }

                return Result.Ok();
            }
            catch (Exception e)
            {
                Log.Error(e, $"{nameof(LoadSites)} Error");
                return Result.Failure<string>(e.Message);
            }
        }
    }
}
