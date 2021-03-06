using System.Threading;
using System.Threading.Tasks;
using Dwapi.Bot.Core.Domain.Indices.Dto;
using MediatR;
using Serilog;

namespace Dwapi.Bot.Core.Application.Matching.Events
{
    public class IndexSiteBlocked:INotification
    {
        public SubjectBlockDto SubjectBlock { get;  }

        public IndexSiteBlocked(SubjectBlockDto subjectBlock)
        {
            SubjectBlock = subjectBlock;
        }
    }

    public class AreaBlockedEventHandler : INotificationHandler<IndexSiteBlocked>
    {
        public Task Handle(IndexSiteBlocked notification, CancellationToken cancellationToken)
        {
            Log.Debug(new string('-',40));
            Log.Debug($"{nameof(IndexSiteBlocked)}:{notification.SubjectBlock}");
            Log.Debug(new string('-',40));
            return Task.CompletedTask;
        }
    }
}
