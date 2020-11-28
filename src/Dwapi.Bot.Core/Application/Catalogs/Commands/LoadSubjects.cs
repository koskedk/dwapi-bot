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
    public class LoadSubjects: IRequest<Result>
    {
        public string SiteJobId { get;  }

        public LoadSubjects(string siteJobId)
        {
            SiteJobId = siteJobId;
        }
    }

    public class LoadSubjectsHandler : IRequestHandler<LoadSubjects, Result>
    {
        private readonly IDocketReader _reader;
        private readonly ISiteRepository _repository;
        public LoadSubjectsHandler(IDocketReader reader, ISiteRepository repository)
        {
            _reader = reader;
            _repository = repository;
        }

        public async Task<Result> Handle(LoadSubjects request, CancellationToken cancellationToken)
        {
            Log.Debug("loading Subjects...");
            string jobId = string.Empty;

            try
            {
                if(string.IsNullOrWhiteSpace(request.SiteJobId))
                    return Result.Ok();

                var siteCatalog = _repository.GetAll<Site, Guid>().ToList();

                foreach (var site in siteCatalog)
                {

                    // Load Subjects
                    var subjects =await _reader.GetSubjects(site.FacilityId);
                    var foundSubjects = subjects.ToList();

                    if (foundSubjects.Any())
                    {
                        foundSubjects.ForEach(x => x.SiteId = site.Id);
                        await _repository.Create<Subject, Guid>(foundSubjects);

                        // Load SubjectExtracts
                        var subjectIds = foundSubjects.Select(x => x.PatientId).ToList();
                        var subjectExtracts =await  _reader.GetSubjectExtracts(subjectIds);
                        var foundExtracts = subjectExtracts.ToList();

                        if (foundExtracts.Any())
                        {
                            foundExtracts.ForEach(x => x.SiteId = site.Id);
                            await _repository.Create<SubjectExtract, Guid>(foundExtracts);
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
