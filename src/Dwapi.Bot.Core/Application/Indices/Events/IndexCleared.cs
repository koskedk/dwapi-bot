using System;
using System.Threading;
using System.Threading.Tasks;
using Dwapi.Bot.SharedKernel.Enums;
using MediatR;
using Serilog;

namespace Dwapi.Bot.Core.Application.Indices.Events
{
    public class IndexCleared : INotification
    {
        public ScanLevel Level { get; }
        public int TotalSites { get; }
        public string JobId { get; }
        public string Dataset { get; set; }
        public DateTime Date => DateTime.Now;

        public IndexCleared(int totalSites, string jobId, ScanLevel level, string dataset)
        {
            TotalSites = totalSites;
            JobId = jobId;
            Level = level;
            Dataset = dataset;
        }

        public override string ToString()
        {
            return $"{nameof(IndexCleared)}:{TotalSites} site(s) | {JobId} {Date:ddMMMyy hh:mm:ss} ";
        }
    }

    public class IndexClearedEventHandler : INotificationHandler<IndexCleared>
    {
        public Task Handle(IndexCleared notification, CancellationToken cancellationToken)
        {
            Log.Debug(new string('*',40));
            Log.Debug(notification.ToString());
            Log.Debug(new string('*',40));
            return Task.CompletedTask;
        }
    }
}
