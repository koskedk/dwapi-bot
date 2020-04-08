using System.Linq;
using Dwapi.Bot.Core.Domain.Indices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Z.Dapper.Plus;

namespace Dwapi.Bot.Infrastructure.Data
{
    public class PatientIndexInterSiteScoreConfiguration: IEntityTypeConfiguration<PatientIndexInterSiteScore>
    {
        public void Configure(EntityTypeBuilder<PatientIndexInterSiteScore> modelBuilder)
        {
            if(DapperPlusManager.MapperCache.Keys.Any(x=>x.EndsWith(nameof(PatientIndexInterSiteScore))))
                return;

            DapperPlusManager.Entity<PatientIndexInterSiteScore>()
                .Table(nameof(BotContext.InterSiteScores))
                .Key(x => x.Id);

            modelBuilder.HasKey(f => f.Id);
        }
    }
}
