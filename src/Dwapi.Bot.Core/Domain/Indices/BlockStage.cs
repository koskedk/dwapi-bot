using System;
using System.ComponentModel.DataAnnotations.Schema;
using Dwapi.Bot.SharedKernel.Enums;
using Dwapi.Bot.SharedKernel.Model;
using Dwapi.Bot.SharedKernel.Utility;

namespace Dwapi.Bot.Core.Domain.Indices
{
    public class BlockStage : Entity<string>
    {
        public ScanLevel Level { get; set; }
        public int BlockCount { get; set; }
        public int Completed { get; set; }
        public DateTime LastUpdate { get; set; }=DateTime.Now;
        [NotMapped]
        public int Progress => Custom.GetPerc( Completed,BlockCount);

        public BlockStage()
        {
        }

        public BlockStage(ScanLevel level, int blockCount)
        {
            Id = $"{level}";
            Level = level;
            BlockCount = blockCount;
            Completed = 0;
        }

        public override string ToString()
        {
            return $"{Id}:{Progress}%  ({Completed}/{BlockCount})";
        }
    }
}
