using System.Linq;
using Dwapi.Bot.Core.Domain.Readers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Z.Dapper.Plus;

namespace Dwapi.Bot.Infrastructure.Data
{
    public class SourceConfiguration
    {
        public SourceConfiguration()
        {
            if(DapperPlusManager.MapperCache.Keys.Any(x=>x.EndsWith(nameof(MasterPatientIndex))))
                return;

            DapperPlusManager.Entity<MasterPatientIndex>()
                .Table("MasterPatientIndices")
                .Key(x => x.Id);
        }
    }
}
