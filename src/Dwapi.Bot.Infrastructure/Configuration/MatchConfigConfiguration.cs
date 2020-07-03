using System.Linq;
using Dwapi.Bot.Core.Domain.Configs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Z.Dapper.Plus;

namespace Dwapi.Bot.Infrastructure.Configuration
{
    public class MatchConfigConfiguration: IEntityTypeConfiguration<MatchConfig>
    {
        public void Configure(EntityTypeBuilder<MatchConfig> modelBuilder)
        {
            if(DapperPlusManager.MapperCache.Keys.Any(x=>x.EndsWith(nameof(MatchConfig))))
                return;

            DapperPlusManager.Entity<MatchConfig>()
                .Table(nameof(BotContext.MatchConfigs))
                .Key(x => x.Id);

            modelBuilder.HasKey(f => f.Id);
        }
    }
}