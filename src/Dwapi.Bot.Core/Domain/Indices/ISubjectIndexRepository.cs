using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dwapi.Bot.Core.Domain.Indices.Dto;
using Dwapi.Bot.SharedKernel.Enums;
using Dwapi.Bot.SharedKernel.Interfaces.Data;

namespace Dwapi.Bot.Core.Domain.Indices
{
    public interface ISubjectIndexRepository:IRepository<SubjectIndex,Guid>
    {
        Task<IEnumerable<SubjectIndex>> GetAllSubjects(int page=1, int pageSize=500);
        Task<int> GetRecordCount();
        Task<int> GetRecordCount(ScanLevel level,string code);
        Task<List<SubjectIndex>> Read(int page, int pageSize);
        Task<List<SubjectIndex>> Read(int page, int pageSize, ScanLevel level, string code);
        Task<int> GetBlockRecordCount(SubjectIndex subject,ScanLevel level);
        Task<List<SubjectIndex>> ReadBlock(int page, int pageSize,SubjectIndex subject,ScanLevel level);
        Task Clear();
        Task CreateOrUpdate(IEnumerable<SubjectIndex> indices);
        Task CreateOrUpdateScores(IEnumerable<SubjectIndexScore> scores);
        Task CreateOrUpdateStages(IEnumerable<SubjectIndexStage> stages);
        Task<IEnumerable<SubjectSiteDto>> GetSubjectSiteDtos();
    }
}
