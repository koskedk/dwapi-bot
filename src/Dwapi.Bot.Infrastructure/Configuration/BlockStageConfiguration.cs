using System.Linq;
using Dwapi.Bot.Core.Domain.Catalogs;
using Dwapi.Bot.Core.Domain.Indices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Z.Dapper.Plus;

namespace Dwapi.Bot.Infrastructure.Configuration
{
    public class BlockStageConfiguration: IEntityTypeConfiguration<BlockStage>
    {
        public void Configure(EntityTypeBuilder<BlockStage> modelBuilder)
        {
            if(DapperPlusManager.MapperCache.Keys.Any(x=>x.EndsWith(nameof(BlockStage))))
                return;

            DapperPlusManager.Entity<BlockStage>()
                .Table(nameof(BotContext.BlockStages))
                .Key(x => x.Id);

            modelBuilder.HasKey(f => f.Id);
        }
    }


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
