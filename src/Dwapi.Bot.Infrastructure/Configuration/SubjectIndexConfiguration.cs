using System.Linq;
using Dwapi.Bot.Core.Domain.Indices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Z.Dapper.Plus;

namespace Dwapi.Bot.Infrastructure.Configuration
{
    public class SubjectIndexConfiguration: IEntityTypeConfiguration<SubjectIndex>
    {
        public void Configure(EntityTypeBuilder<SubjectIndex> modelBuilder)
        {
            if(DapperPlusManager.MapperCache.Keys.Any(x=>x.EndsWith(nameof(SubjectIndex))))
                return;

            DapperPlusManager.Entity<SubjectIndex>()
                .Table(nameof(BotContext.SubjectIndices))
                .Key(x => x.Id);

            modelBuilder.HasKey(f => f.Id);
        }
    }
}
