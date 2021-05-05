using System;
using System.Threading;
using System.Threading.Tasks;
using Dwapi.Bot.SharedKernel.Enums;
using MediatR;
using Serilog;

namespace Dwapi.Bot.Core.Application.Indices.Events
{
    public class SitesLoaded : INotification
    {
        public int NumberOfSites { get; private set; }
        public DateTime Date => DateTime.Now;
        public string JobId { get; }

        public SitesLoaded(int numberOfSites, string jobId)
        {
            NumberOfSites = numberOfSites;
            JobId = jobId;
        }

        public override string ToString()
        {
            return $"{nameof(SitesLoaded)}:{NumberOfSites} site(s) | {JobId} {Date:ddMMMyy hh:mm:ss} ";
        }
    }

    public class SitesLoadedEventHandler : INotificationHandler<SitesLoaded>
    {
        public Task Handle(SitesLoaded notification, CancellationToken cancellationToken)
        {
            Log.Debug(new string('*',40));
            Log.Debug(notification.ToString());
            Log.Debug(new string('*',40));
            return Task.CompletedTask;
        }
    }
}
