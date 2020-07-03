using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Dwapi.Bot.Core.Domain.Indices;
using Dwapi.Bot.SharedKernel.Enums;
using Z.Dapper.Plus;

namespace Dwapi.Bot.Infrastructure.Data
{
    public class BlockStageRepository : BaseRepository<BlockStage, string>, IBlockStageRepository
    {
        public BlockStageRepository(BotContext context) : base(context)
        {
        }

        public async Task<BlockStage> GetBlockStage(ScanLevel level)
        {
            BlockStage stage = null;
            var id = $"{level}";

            var sql = $@"
              select * from {nameof(BotContext.BlockStages)} 
              where {nameof(BlockStage.Id)}=@id";

            using (var cn = GetConnectionOnly())
            {
                if (cn.State != ConnectionState.Open)
                    cn.Open();
                var results = await cn.QueryAsync<BlockStage>(sql, new {id});
                var blockStages = results.ToList();
                if (blockStages.Any())
                    stage = blockStages.FirstOrDefault();
            }

            return stage;
        }

        public async Task InitBlock(ScanLevel level)
        {
            var sql =
                $@" SELECT 
                        COUNT (DISTINCT {nameof(SubjectIndex.SiteBlockId)}) 
                    FROM {nameof(BotContext.SubjectIndices)} 
                    WHERE  {nameof(SubjectIndex.SiteBlockId)} IS NOT NULL ";

            if (level==ScanLevel.InterSite)
                sql = sql.Replace(nameof(SubjectIndex.SiteBlockId),nameof(SubjectIndex.InterSiteBlockId));


            using (var cn = GetConnectionOnly())
            {
                if (cn.State != ConnectionState.Open)
                    cn.Open();

                var count = await cn.ExecuteScalarAsync<int>(sql);

                await cn.BulkActionAsync(x=>
                    x.BulkMerge(new BlockStage(level, count)));
            }
        }

        public async Task UpdateBlock(ScanLevel level)
        {
            var id = $"{level}";

            var compsql =
                $@" SELECT 
                        COUNT (DISTINCT {nameof(SubjectIndex.SiteBlockId)}) 
                    FROM {nameof(BotContext.SubjectIndices)} 
                    WHERE  {nameof(SubjectIndex.SiteBlockStatus)}=@status";

            if (level == ScanLevel.InterSite)
                compsql = compsql
                    .Replace(nameof(SubjectIndex.SiteBlockId), nameof(SubjectIndex.InterSiteBlockId))
                    .Replace(nameof(SubjectIndex.SiteBlockStatus), nameof(SubjectIndex.InterSiteBlockStatus));


            var sql = $@"
              update {nameof(BotContext.BlockStages)} 
              set {nameof(BlockStage.Completed)}=@completed
              where {nameof(BlockStage.Id)}=@id";

            using (var cn = GetConnectionOnly())
            {
                if (cn.State != ConnectionState.Open)
                    cn.Open();

                var completed =
                    await cn.ExecuteScalarAsync<int>(compsql, new {status = ScanStatus.Scanned});

                await cn.ExecuteAsync(sql, new {id, completed});
            }

        }
    }
}
