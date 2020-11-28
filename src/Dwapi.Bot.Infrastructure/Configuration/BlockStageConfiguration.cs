using System.Linq;
using Dwapi.Bot.Core.Domain.Indices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Z.Dapper.Plus;

namespace Dwapi.Bot.Infrastructure.Configuration
{
    public class BlockStageConfiguration: IEntityTypeConfiguration<BlockStage>
    {
        public void Configure(EntityTypeBuilder<BlockStage> modelBuilder)
        {
            if(DapperPlusManager.MapperCache.Keys.Any(x=>x.EndsWith(nameof(BlockStage))))
                return;

            DapperPlusManager.Entity<BlockStage>()
                .Table(nameof(BotContext.BlockStages))
                .Key(x => x.Id);

            modelBuilder.HasKey(f => f.Id);
        }
    }
}
