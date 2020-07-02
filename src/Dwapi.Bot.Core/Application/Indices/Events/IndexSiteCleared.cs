using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dwapi.Bot.Core.Application.Matching.Events;
using Dwapi.Bot.Core.Domain.Indices;
using Dwapi.Bot.Core.Domain.Indices.Dto;
using Dwapi.Bot.SharedKernel.Utility;
using MediatR;
using Serilog;

namespace Dwapi.Bot.Core.Application.Indices.Events
{
    public class IndexSiteCleared:INotification
    {
        public int TotalSites  { get;  }
        public SubjectSiteDto SubjectSite { get;  }

        public IndexSiteCleared(SubjectSiteDto subjectSite, int totalSites)
        {
            SubjectSite = subjectSite;
            TotalSites = totalSites;
        }
    }
    public class IndexSiteClearedHandler:INotificationHandler<IndexSiteCleared>
    {
        private readonly ISubjectIndexRepository _repository;

        public IndexSiteClearedHandler(ISubjectIndexRepository repository, IBlockStageRepository blockStageRepository)
        {
            _repository = repository;
        }

        public async Task Handle(IndexSiteCleared notification, CancellationToken cancellationToken)
        {
            var remainingSites = await _repository.GetSubjectSiteDtos();
            Log.Debug(
                $"Clearing {Custom.GetPerc((notification.TotalSites - remainingSites.Count()), notification.TotalSites)}");
        }
    }
}
