using System.Linq;
using Dwapi.Bot.Core.Domain.Catalogs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Z.Dapper.Plus;

namespace Dwapi.Bot.Infrastructure.Configuration
{
    public class SubjectExtractConfiguration: IEntityTypeConfiguration<SubjectExtract>
    {
        public void Configure(EntityTypeBuilder<SubjectExtract> modelBuilder)
        {
            if(DapperPlusManager.MapperCache.Keys.Any(x=>x.EndsWith(nameof(SubjectExtract))))
                return;

            DapperPlusManager.Entity<SubjectExtract>()
                .Table(nameof(BotCleanerContext.Extracts))
                .Key(x => x.Id);

            modelBuilder.HasKey(f => f.Id);
        }
    }
}