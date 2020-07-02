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
        public SubjectSiteDto SubjectSite { get;  }

        public IndexSiteCleared(SubjectSiteDto subjectSite)
        {
            SubjectSite = subjectSite;
        }
    }

    public class IndexSiteClearedEventHandler : INotificationHandler<IndexSiteCleared>
    {
        public Task Handle(IndexSiteCleared notification, CancellationToken cancellationToken)
        {
            Log.Debug($"{nameof(IndexSiteCleared)}:{notification.SubjectSite}");
            return Task.CompletedTask;
        }
    }
}
