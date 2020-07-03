using System;
using System.Threading;
using System.Threading.Tasks;
using Dwapi.Bot.SharedKernel.Enums;
using MediatR;
using Serilog;

namespace Dwapi.Bot.Core.Application.Matching.Events
{
    public class IndexScanned : INotification
    {
        public ScanLevel Level { get; set; }
        public int Count { get; }
        public string JobId { get; }
        public DateTime Date => DateTime.Now;
        public IndexScanned(int count, string jobId,ScanLevel level )
        {
            Count = count;
            Level = level;
            JobId = jobId;
        }

        public override string ToString()
        {
            return $"{nameof(IndexScanned)}:{Count} blocks(s) | {JobId} {Date:ddMMMyy hh:mm:ss} ";
        }
    }
    public class IndexScannedEventHandler : INotificationHandler<IndexScanned>
    {
        public Task Handle(IndexScanned notification, CancellationToken cancellationToken)
        {
            Log.Debug(new string('*',40));
            Log.Debug($"{notification.Count}");
            Log.Debug(new string('*',40));
            return Task.CompletedTask;
        }
    }
}
