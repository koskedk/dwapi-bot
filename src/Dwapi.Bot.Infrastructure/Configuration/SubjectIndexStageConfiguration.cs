using System.Linq;
using Dwapi.Bot.Core.Domain.Indices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Z.Dapper.Plus;

namespace Dwapi.Bot.Infrastructure.Configuration
{
    public class SubjectIndexStageConfiguration: IEntityTypeConfiguration<SubjectIndexStage>
    {
        public void Configure(EntityTypeBuilder<SubjectIndexStage> modelBuilder)
        {
            if(DapperPlusManager.MapperCache.Keys.Any(x=>x.EndsWith(nameof(SubjectIndexStage))))
                return;

            DapperPlusManager.Entity<SubjectIndexStage>()
                .Table(nameof(BotContext.SubjectIndexStages))
                .Key(x => x.Id);

            modelBuilder.HasKey(f => f.Id);
        }
    }
}
