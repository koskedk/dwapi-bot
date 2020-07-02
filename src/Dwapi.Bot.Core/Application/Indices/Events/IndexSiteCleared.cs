using System.Threading;
using System.Threading.Tasks;
using Dwapi.Bot.Core.Domain.Indices.Dto;
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
