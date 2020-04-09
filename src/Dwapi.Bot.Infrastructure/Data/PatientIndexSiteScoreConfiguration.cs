using System.Linq;
using Dwapi.Bot.Core.Domain.Indices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Z.Dapper.Plus;

namespace Dwapi.Bot.Infrastructure.Data
{
    public class PatientIndexSiteScoreConfiguration: IEntityTypeConfiguration<PatientIndexSiteScore>
    {
        public void Configure(EntityTypeBuilder<PatientIndexSiteScore> modelBuilder)
        {
            if(DapperPlusManager.MapperCache.Keys.Any(x=>x.EndsWith(nameof(PatientIndexSiteScore))))
                return;

            DapperPlusManager.Entity<PatientIndexSiteScore>()
                .Table(nameof(BotContext.SiteScores))
                .Key(x => x.Id);

            modelBuilder.HasKey(f => f.Id);
        }
    }
}
