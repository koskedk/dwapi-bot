using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Serilog;

namespace Dwapi.Bot.Core.Application.Indices.Events
{
    public class IndexRefreshed:INotification
    {
        public int Count { get; }

        public IndexRefreshed(int count)
        {
            Count = count;
        }
    }

    public class IndexRefreshedEventHandler : INotificationHandler<IndexRefreshed>
    {
        public Task Handle(IndexRefreshed notification, CancellationToken cancellationToken)
        {
            Log.Debug(new string('*',40));
            Log.Debug($"{nameof(IndexRefreshed)}:{notification.Count} site(s)");
            Log.Debug(new string('*',40));
            return Task.CompletedTask;
        }
    }
}
