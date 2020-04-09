using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dwapi.Bot.SharedKernel.Enums;
using Dwapi.Bot.SharedKernel.Interfaces.Data;

namespace Dwapi.Bot.Core.Domain.Indices
{
    public interface ISubjectIndexRepository:IRepository<SubjectIndex,Guid>
    {
        int PageCount(int batchSize, long totalRecords);
        Task<int> GetRecordCount();
        Task<List<SubjectIndex>> Read(int page, int pageSize,int? siteCode=null);

        Task<int> GetBlockRecordCount(SubjectIndex subject,ScanLevel level);
        Task<List<SubjectIndex>> ReadBlock(int page, int pageSize,SubjectIndex subject,ScanLevel level);
        Task Clear();
        Task CreateOrUpdate(IEnumerable<SubjectIndex> indices);
        Task CreateOrUpdate(IEnumerable<SubjectIndexScore> scores);
        Task CreateOrUpdate(IEnumerable<SubjectIndexStage> stages);
    }
}
