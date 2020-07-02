using System.Threading;
using System.Threading.Tasks;
using Dwapi.Bot.Core.Application.Indices.Events;
using MediatR;
using Serilog;

namespace Dwapi.Bot.Core.Tests.Notifications
{
    public class TestEventIndexRefreshedHandler : INotificationHandler<IndexRefreshed>
    {

        public Task Handle(IndexRefreshed notification, CancellationToken cancellationToken)
        {
            Log.Debug($"Referesh >> {notification.Site}");
            Log.Debug($"     {notification.SiteCount}|{notification.TotalSiteCount}");
            Log.Debug($"              [{notification.Count}|{notification.TotalCount}]");
            return Task.CompletedTask;
        }
    }
}