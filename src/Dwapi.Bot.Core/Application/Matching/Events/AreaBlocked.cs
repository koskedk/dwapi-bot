using System.Threading;
using System.Threading.Tasks;
using Dwapi.Bot.Core.Domain.Indices.Dto;
using MediatR;
using Serilog;

namespace Dwapi.Bot.Core.Application.Matching.Events
{
    public class AreaBlocked:INotification
    {
        public SubjectBlockDto SubjectBlock { get;  }

        public AreaBlocked(SubjectBlockDto subjectBlock)
        {
            SubjectBlock = subjectBlock;
        }
    }

    public class AreaBlockedEventHandler : INotificationHandler<AreaBlocked>
    {
        public Task Handle(AreaBlocked notification, CancellationToken cancellationToken)
        {
            Log.Debug(new string('-',40));
            Log.Debug($"{nameof(AreaBlocked)}:{notification.SubjectBlock}");
            Log.Debug(new string('-',40));
            return Task.CompletedTask;
        }
    }
}
