using System;
using System.Threading;
using System.Threading.Tasks;
using Dwapi.Bot.Core.Domain.Indices;
using Dwapi.Bot.SharedKernel.Enums;
using MediatR;

namespace Dwapi.Bot.Core.Application.Matching.Events
{
    public class BlockScanned : INotification
    {
        public Guid Id { get; }
        public ScanLevel Level { get; }

        public ScanStatus Status { get; }

        public BlockScanned(Guid id, ScanLevel level, ScanStatus status)
        {
            Id = id;
            Level = level;
            Status = status;
        }
    }

    public class BlockScannedHandler:INotificationHandler<BlockScanned>
    {
        private readonly ISubjectIndexRepository _repository;
        private readonly IBlockStageRepository _blockStageRepository;

        public BlockScannedHandler(ISubjectIndexRepository repository, IBlockStageRepository blockStageRepository)
        {
            _repository = repository;
            _blockStageRepository = blockStageRepository;
        }

        public async Task Handle(BlockScanned notification, CancellationToken cancellationToken)
        {
            await _repository.UpdateScan(notification.Id, notification.Level,notification.Status);
            await _blockStageRepository.UpdateBlock(notification.Level);
        }
    }
}
