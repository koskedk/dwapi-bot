using System;
using System.Threading;
using System.Threading.Tasks;
using Dwapi.Bot.Core.Domain.Indices;
using Dwapi.Bot.SharedKernel.Enums;
using MediatR;

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
}
