using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Serilog;

namespace Dwapi.Bot.Core.Application.Indices.Events
{
    public class IndexCleared : INotification
    {
        public int TotalSites { get; }
        public IndexCleared(int totalSites)
        {
            TotalSites = totalSites;
        }
    }

    public class IndexClearedEventHandler : INotificationHandler<IndexCleared>
    {
        public Task Handle(IndexCleared notification, CancellationToken cancellationToken)
        {
            Log.Debug(new string('*',40));
            Log.Debug($"{nameof(IndexCleared)}:{notification.TotalSites} site(s)");
            Log.Debug(new string('*',40));
            return Task.CompletedTask;
        }
    }
}
