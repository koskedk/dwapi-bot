using System.Threading;
using System.Threading.Tasks;
using Dwapi.Bot.Core.Domain.Indices.Dto;
using MediatR;
using Serilog;

namespace Dwapi.Bot.Core.Application.Indices.Events
{
    public class IndexSiteRefreshed : INotification
    {
        public SubjectSiteDto Site { get;  }

        public int RecordCount { get;  }

        public IndexSiteRefreshed(SubjectSiteDto site, int recordCount)
        {
            Site = site;
            RecordCount = recordCount;
        }
    }

    public class IndexSiteRefreshedEventHandler : INotificationHandler<IndexSiteRefreshed>
    {
        public Task Handle(IndexSiteRefreshed notification, CancellationToken cancellationToken)
        {
            Log.Debug(new string('-',40));
            Log.Debug($"{nameof(IndexSiteRefreshed)}:{notification.Site} ({notification.RecordCount}) records");
            Log.Debug(new string('-',40));
            return Task.CompletedTask;
        }
    }
}
