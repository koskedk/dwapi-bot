using System.Threading;
using System.Threading.Tasks;
using Dwapi.Bot.Core.Application.Indices.Events;
using MediatR;
using Serilog;

namespace Dwapi.Bot.Core.Tests.Notifications
{
    public class TestEventIndexSiteClearedHandler : INotificationHandler<IndexSiteCleared>
    {
        public Task Handle(IndexSiteCleared notification, CancellationToken cancellationToken)
        {
            Log.Debug($"Cleared >> {notification.SubjectSite}");
            return Task.CompletedTask;
        }
    }
}