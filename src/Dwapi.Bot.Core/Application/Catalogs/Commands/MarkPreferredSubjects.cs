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
    public class MarkPreferredSubjects: IRequest<Result>
    {
        public string LoadJobId { get;  }

        public MarkPreferredSubjects(string loadJobId)
        {
            LoadJobId = loadJobId;
        }
    }

    public class MarkPreferredSubjectsHandler : IRequestHandler<MarkPreferredSubjects, Result>
    {
        private readonly IDocketReader _reader;
        private readonly ISiteRepository _repository;

        public MarkPreferredSubjectsHandler(IDocketReader reader, ISiteRepository repository)
        {
            _reader = reader;
            _repository = repository;
        }

        public async Task<Result> Handle(MarkPreferredSubjects request, CancellationToken cancellationToken)
        {
            Log.Debug("Marking Subjects...");
            string jobId = string.Empty;

            try
            {
                if(string.IsNullOrWhiteSpace(request.LoadJobId))
                    return Result.Ok();

                var siteCatalog = _repository.GetAll<Site, Guid>().ToList();

                foreach (var site in siteCatalog)
                {
                    // Set Preferred Subjects

                    var preferredList = new List<Guid>();

                    var subjects = _repository.GetAll<Subject, Guid>().Where(x => x.SiteId == site.Id).ToList();

                    foreach (var s in subjects.GroupBy(x => x.PatientPk))
                    {
                        var preferredId = s.OrderByDescending(x => x.Created).First().PatientId;
                        foreach (var ps in s)
                            ps.AssignPreferred(preferredId);
                        preferredList.Add(preferredId);
                    }

                    if (preferredList.Any())
                    {
                        await _repository.Update<Subject, Guid>(subjects);

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
