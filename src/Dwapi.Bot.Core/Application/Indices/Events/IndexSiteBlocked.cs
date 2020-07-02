using System.Threading;
using System.Threading.Tasks;
using Dwapi.Bot.Core.Domain.Indices.Dto;
using MediatR;
using Serilog;

namespace Dwapi.Bot.Core.Application.Indices.Events
{
    public class IndexSiteBlocked:INotification
    {
        public SubjectBlockDto SubjectBlock { get;  }

        public IndexSiteBlocked(SubjectBlockDto subjectBlock)
        {
            SubjectBlock = subjectBlock;
        }
    }

    public class IndexSiteBlockedEventHandler : INotificationHandler<IndexSiteBlocked>
    {
        public Task Handle(IndexSiteBlocked notification, CancellationToken cancellationToken)
        {
            Log.Debug($"{nameof(IndexSiteBlocked)}:{notification.SubjectBlock}");
            return Task.CompletedTask;
        }
    }
}