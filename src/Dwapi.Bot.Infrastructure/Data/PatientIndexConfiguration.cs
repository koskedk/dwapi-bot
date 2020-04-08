using System.Linq;
using Dwapi.Bot.Core.Domain.Indices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Z.Dapper.Plus;

namespace Dwapi.Bot.Infrastructure.Data
{
    public class PatientIndexConfiguration: IEntityTypeConfiguration<PatientIndex>
    {
        public void Configure(EntityTypeBuilder<PatientIndex> modelBuilder)
        {
            if(DapperPlusManager.MapperCache.Keys.Any(x=>x.EndsWith(nameof(PatientIndex))))
                return;

            DapperPlusManager.Entity<PatientIndex>()
                .Table(nameof(BotContext.PatientIndices))
                .Key(x => x.Id);
        }
    }
}
