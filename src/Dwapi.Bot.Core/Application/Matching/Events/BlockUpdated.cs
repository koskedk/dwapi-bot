using System;
using System.Threading;
using System.Threading.Tasks;
using Dwapi.Bot.Core.Domain.Indices;
using Dwapi.Bot.SharedKernel.Enums;
using MediatR;
using Serilog;

namespace Dwapi.Bot.Core.Application.Matching.Events
{
    public class BlockUpdated : INotification
    {
        public ScanLevel Level { get; }

        public BlockUpdated(ScanLevel level)
        {
            Level = level;
        }
    }

    public class BlockUpdatedHandler : INotificationHandler<BlockUpdated>
    {
        private IBlockStageRepository _blockStageRepository;

        public BlockUpdatedHandler(IBlockStageRepository blockStageRepository)
        {
            _blockStageRepository = blockStageRepository;
        }

        public async Task Handle(BlockUpdated notification, CancellationToken cancellationToken)
        {
            var stage = await _blockStageRepository.GetBlockStage(notification.Level);
            Log.Debug($"{notification.Level} {stage}");
        }
    }
}
