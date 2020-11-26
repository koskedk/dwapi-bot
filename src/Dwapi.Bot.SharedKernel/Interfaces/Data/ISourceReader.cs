using System.Threading.Tasks;
using Dwapi.Bot.SharedKernel.Common;

namespace Dwapi.Bot.SharedKernel.Interfaces.Data
{
    public interface ISourceReader
    {
        DataSourceInfo SourceInfo { get; }
        Task<int> GetRecordCount();
        Task<int> GetRecordCount(int siteCode);
    }
}
