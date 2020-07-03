using System;
using System.Threading;
using System.Threading.Tasks;
using Dwapi.Bot.Core.Domain.Indices;
using Dwapi.Bot.SharedKernel.Enums;
using MediatR;
using Serilog;

namespace Dwapi.Bot.Core.Application.Matching.Events
{
    public class IndexSiteScanned : INotification
    {
        public Guid Id { get; }
        public ScanLevel Level { get; }

        public ScanStatus Status { get; }

        public IndexSiteScanned(Guid id, ScanLevel level, ScanStatus status)
        {
            Id = id;
            Level = level;
            Status = status;
        }
    }

    public class BlockScannedHandler : INotificationHandler<IndexSiteScanned>
    {
        private readonly ISubjectIndexRepository _repository;

        public BlockScannedHandler(ISubjectIndexRepository repository)
        {
            _repository = repository;
        }

        public async Task Handle(IndexSiteScanned notification, CancellationToken cancellationToken)
        {
            Log.Debug(new string('-', 40));
            Log.Debug($"{nameof(IndexSiteScanned)}:{notification.Id}");
            Log.Debug(new string('-', 40));

            await _repository.UpdateScan(notification.Id, notification.Level, notification.Status);
        }
    }
}
