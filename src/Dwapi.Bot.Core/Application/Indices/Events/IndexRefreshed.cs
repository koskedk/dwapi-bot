using System;
using System.Threading;
using System.Threading.Tasks;
using Dwapi.Bot.SharedKernel.Enums;
using MediatR;
using Serilog;

namespace Dwapi.Bot.Core.Application.Indices.Events
{
    public class IndexRefreshed:INotification
    {
        public ScanLevel Level { get; }
        public int Count { get; }
        public string JobId { get; }
        public DateTime Date => DateTime.Now;

        public IndexRefreshed(int count, string jobId, ScanLevel level)
        {
            Count = count;
            JobId = jobId;
            Level = level;
        }

        public override string ToString()
        {
            return $"{nameof(IndexCleared)}:{Count} site(s) | {JobId} {Date:ddMMMyy hh:mm:ss} ";
        }
    }

    public class IndexRefreshedEventHandler : INotificationHandler<IndexRefreshed>
    {
        public Task Handle(IndexRefreshed notification, CancellationToken cancellationToken)
        {
            Log.Debug(new string('*',40));
            Log.Debug($"{notification}");
            Log.Debug(new string('*',40));
            return Task.CompletedTask;
        }
    }
}
