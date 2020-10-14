using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dwapi.Bot.Core.Domain.Indices.Dto;
using Dwapi.Bot.SharedKernel.Enums;
using Dwapi.Bot.SharedKernel.Interfaces.Data;

namespace Dwapi.Bot.Core.Domain.Indices
{
    public interface ISubjectIndexRepository : IRepository<SubjectIndex, Guid>
    {
        Task<IEnumerable<SubjectIndex>> GetAllSubjects(int page = 1, int pageSize = 500);
        Task<int> GetRecordCount();
        Task<int> GetRecordCount(ScanLevel level, string code);
        Task<int> GetRecordCount(ScanLevel level, Guid blockId);
        Task<List<SubjectIndex>> Read(int page, int pageSize);
        Task<List<SubjectIndex>> Read(int page, int pageSize, ScanLevel level, string code);
        Task<IEnumerable<SubjectIndex>> Read(int page, int pageSize, ScanLevel level, Guid blockId);
        Task<int> GetBlockRecordCount( ScanLevel level);
        Task<int> GetBlockRecordCount(SubjectIndex subject, ScanLevel level);
        Task<List<SubjectIndex>> ReadBlock(int page, int pageSize, SubjectIndex subject, ScanLevel level);
        Task InitClear();
        Task Clear();
        Task Clear(int siteCode);
        Task Clear(int siteCode,ScanLevel level);
        Task CreateOrUpdate(IEnumerable<SubjectIndex> indices);

        Task CreateOrUpdateScores(IEnumerable<SubjectIndexScore> scores);
        Task CreateOrUpdateStages(IEnumerable<SubjectIndexStage> stages);
        Task<IEnumerable<SubjectSiteDto>> GetSubjectSiteDtos();
        Task<IEnumerable<SubjectBlockDto>> GetSubjectInterSiteBlockDtos();
        Task<IEnumerable<SubjectBlockDto>> GetSubjectSiteBlockDtos();
        Task BlockInterSiteSubjects(SubjectBlockDto blockDto);
        Task BlockSiteSubjects(SubjectBlockDto blockDto);
        Task<IEnumerable<Guid>> GetSiteBlocks(ScanStatus status = ScanStatus.Pending);
        Task<IEnumerable<Guid>> GetInterSiteBlocks(ScanStatus status = ScanStatus.Pending);
        Task UpdateScan(Guid notificationId, ScanLevel notificationLevel,ScanStatus status);
    }
}
