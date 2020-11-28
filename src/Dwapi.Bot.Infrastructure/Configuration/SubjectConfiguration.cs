using System.Linq;
using Dwapi.Bot.Core.Domain.Catalogs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Z.Dapper.Plus;

namespace Dwapi.Bot.Infrastructure.Configuration
{
    public class SubjectConfiguration: IEntityTypeConfiguration<Subject>
    {
        public void Configure(EntityTypeBuilder<Subject> modelBuilder)
        {
            if(DapperPlusManager.MapperCache.Keys.Any(x=>x.EndsWith(nameof(Subject))))
                return;

            DapperPlusManager.Entity<Subject>()
                .Table(nameof(BotCleanerContext.Subjects))
                .Key(x => x.Id);

            modelBuilder.HasKey(f => f.Id);
        }
    }
}