using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Serilog;

namespace Dwapi.Bot.Core.Application.Indices.Events
{
    public class IndexBlocked : INotification
    {
        public int Count { get; }
        public IndexBlocked(int count)
        {
            Count = count;
        }
    }
    public class IndexBlockedEventHandler : INotificationHandler<IndexBlocked>
    {
        public Task Handle(IndexBlocked notification, CancellationToken cancellationToken)
        {
            Log.Debug($"{nameof(IndexBlocked)}:{notification.Count} block(s)");
            return Task.CompletedTask;
        }
    }
}