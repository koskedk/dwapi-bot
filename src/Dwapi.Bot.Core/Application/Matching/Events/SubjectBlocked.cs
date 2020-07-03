using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Serilog;

namespace Dwapi.Bot.Core.Application.Matching.Events
{
    public class SubjectBlocked : INotification
    {
        public int Count { get; }
        public SubjectBlocked(int count)
        {
            Count = count;
        }
    }
    public class SubjectBlockedEventHandler : INotificationHandler<SubjectBlocked>
    {
        public Task Handle(SubjectBlocked notification, CancellationToken cancellationToken)
        {
            Log.Debug(new string('*',40));
            Log.Debug($"{nameof(SubjectBlocked)}:{notification.Count} block(s)");
            Log.Debug(new string('*',40));
            return Task.CompletedTask;
        }
    }
}
