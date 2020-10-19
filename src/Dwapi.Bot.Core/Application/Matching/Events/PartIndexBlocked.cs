using System;
using System.Threading;
using System.Threading.Tasks;
using Dwapi.Bot.SharedKernel.Enums;
using MediatR;
using Serilog;

namespace Dwapi.Bot.Core.Application.Matching.Events
{
    public class PartIndexBlocked : INotification
    {
        public ScanLevel Level { get; set; }
        public int Count { get; }
        public string JobId { get; }
        public DateTime Date => DateTime.Now;
        public PartIndexBlocked(int count, string jobId,ScanLevel level )
        {
            Count = count;
            Level = level;
            JobId = jobId;
        }

        public override string ToString()
        {
            return $"{nameof(IndexBlocked)}:{Count} blocks(s) | {JobId} {Date:ddMMMyy hh:mm:ss} ";
        }
    }
    public class PartIndexBlockedEventHandler : INotificationHandler<PartIndexBlocked>
    {
        public Task Handle(PartIndexBlocked notification, CancellationToken cancellationToken)
        {
            Log.Debug(new string('*',40));
            Log.Debug($"{notification.Count}");
            Log.Debug(new string('*',40));
            return Task.CompletedTask;
        }
    }


}
