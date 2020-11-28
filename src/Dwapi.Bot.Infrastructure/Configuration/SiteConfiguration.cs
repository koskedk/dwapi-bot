using System.Linq;
using Dwapi.Bot.Core.Domain.Catalogs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Z.Dapper.Plus;

namespace Dwapi.Bot.Infrastructure.Configuration
{
    public class SiteConfiguration: IEntityTypeConfiguration<Site>
    {
        public void Configure(EntityTypeBuilder<Site> modelBuilder)
        {
            if(DapperPlusManager.MapperCache.Keys.Any(x=>x.EndsWith(nameof(Site))))
                return;

            DapperPlusManager.Entity<Site>()
                .Table(nameof(BotCleanerContext.Sites))
                .Key(x => x.Id);

            modelBuilder.HasKey(f => f.Id);
        }
    }
}