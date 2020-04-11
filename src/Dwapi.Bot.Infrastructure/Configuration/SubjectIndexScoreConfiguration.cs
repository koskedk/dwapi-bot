using System.Linq;
using Dwapi.Bot.Core.Domain.Indices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Z.Dapper.Plus;

namespace Dwapi.Bot.Infrastructure.Configuration
{
    public class SubjectIndexScoreConfiguration: IEntityTypeConfiguration<SubjectIndexScore>
    {
        public void Configure(EntityTypeBuilder<SubjectIndexScore> modelBuilder)
        {
            if(DapperPlusManager.MapperCache.Keys.Any(x=>x.EndsWith(nameof(SubjectIndexScore))))
                return;

            DapperPlusManager.Entity<SubjectIndexScore>()
                .Table(nameof(BotContext.SubjectIndexScores))
                .Key(x => x.Id);

            modelBuilder.HasKey(f => f.Id);
        }
    }
}
