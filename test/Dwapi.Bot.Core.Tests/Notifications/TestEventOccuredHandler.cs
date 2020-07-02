using System.Threading;
using System.Threading.Tasks;
using Dwapi.Bot.Core.Application.Common.Events;
using Dwapi.Bot.Core.Application.Indices.Commands;
using MediatR;
using Serilog;

namespace Dwapi.Bot.Core.Tests.Notifications
{
    public class TestEventOccuredHandler:INotificationHandler<EventOccured>
    {

        public Task Handle(EventOccured notification, CancellationToken cancellationToken)
        {
            Log.Debug($"{notification.Event} {notification.Total}" );
            return Task.CompletedTask;
        }
    }
}
