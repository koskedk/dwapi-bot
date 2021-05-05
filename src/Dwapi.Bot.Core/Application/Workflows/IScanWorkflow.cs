using Dwapi.Bot.Core.Application.Indices.Events;
using Dwapi.Bot.Core.Application.Matching.Events;
using MediatR;

namespace Dwapi.Bot.Core.Application.Workflows
{
    public interface IScanWorkflow:
        INotificationHandler<IndexCleared>,
        INotificationHandler<IndexRefreshed>,
        INotificationHandler<PartIndexBlocked>,
        INotificationHandler<IndexBlocked>,
        INotificationHandler<IndexScanned>
    {

    }

    public interface IClearDwhWorkflow:
        INotificationHandler<SitesLoaded>,
        INotificationHandler<IndexRefreshed>,
        INotificationHandler<PartIndexBlocked>,
        INotificationHandler<IndexBlocked>,
        INotificationHandler<IndexScanned>
    {

    }
}
