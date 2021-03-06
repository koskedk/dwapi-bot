using Dwapi.Bot.Core.Application.Indices.Events;
using Dwapi.Bot.Core.Application.Matching.Events;
using MediatR;

namespace Dwapi.Bot.Core.Application.Workflows
{
    public interface IScanWorkflow:
        INotificationHandler<IndexCleared>,
        INotificationHandler<IndexRefreshed>,
        INotificationHandler<IndexBlocked>,
        INotificationHandler<IndexScanned>
    {

    }
}
