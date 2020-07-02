using System;
using System.Threading.Tasks;
using Dwapi.Bot.SharedKernel.Enums;
using Dwapi.Bot.SharedKernel.Interfaces.Data;

namespace Dwapi.Bot.Core.Domain.Indices
{
    public interface IBlockStageRepository : IRepository<BlockStage, string>
    {
        Task<BlockStage> GetBlockStage(ScanLevel level);
        Task InitBlock(ScanLevel level);
        Task UpdateBlock(ScanLevel level);
    }
}
