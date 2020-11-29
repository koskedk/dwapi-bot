using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Dwapi.Bot.Core.Domain.Catalogs;
using Dwapi.Bot.Core.Domain.Catalogs.Dtos;
using Dwapi.Bot.Core.Domain.Indices.Dto;
using Dwapi.Bot.Core.Domain.Readers;
using Dwapi.Bot.Infrastructure.Configuration;
using Dwapi.Bot.SharedKernel.Common;
using Microsoft.Data.Sqlite;

namespace Dwapi.Bot.Infrastructure.Data
{
    public class DocketReader : IDocketReader
    {
        private readonly SourceConfiguration _sourceConfiguration;
        public DataSourceInfo SourceInfo { get; }

        public DocketReader(DataSourceInfo sourceInfo)
        {
            SourceInfo = sourceInfo;
            _sourceConfiguration = new SourceConfiguration();
        }

        public async Task<int> GetRecordCount()
        {
            int count = 0;
            var sql = @"

SELECT
      Count(p.Id)
FROM
     PatientExtract p inner join
     (
         SELECT FacilityId,PatientPID,COUNT(Id) AS PCount
         FROM PatientExtract
         GROUP BY FacilityId, PatientPID
         HAVING (COUNT(*) > 1)
     ) d on p.PatientPID = d.PatientPID and p.FacilityId = d.FacilityId inner join
         Facility f on f.Id = p.FacilityId
";
            if (SourceInfo.DbType == SharedKernel.Enums.DbType.SQLite)
                sql = sql.Replace("ISNULL", "IFNULL");
            using (var con = GetConnection())
            {
                con.Open();
                count = await con.ExecuteScalarAsync<int>(sql,commandTimeout:0);
            }

            return count;
        }
        public async Task<int> GetRecordCount(int siteCode)
        {
            int count = 0;
            var sql = @"

SELECT
      Count(p.Id)
FROM
     PatientExtract p inner join
     (
         SELECT FacilityId,PatientPID,COUNT(Id) AS PCount
         FROM PatientExtract
         GROUP BY FacilityId, PatientPID
         HAVING (COUNT(*) > 1)
     ) d on p.PatientPID = d.PatientPID and p.FacilityId = d.FacilityId inner join
         Facility f on f.Id = p.FacilityId
WHERE f.Code=@siteCode

";
            if (SourceInfo.DbType == SharedKernel.Enums.DbType.SQLite)
                sql = sql.Replace("ISNULL", "IFNULL");
            using (var con = GetConnection())
            {
                con.Open();
                count = await con.ExecuteScalarAsync<int>(sql,new {siteCode},commandTimeout:0);
            }
            return count;
        }

        public async Task<IEnumerable<Site>> GetSites()
        {
            var store = GetConnection().Database;

            var sql = $@"
                            SELECT
                                  DISTINCT 'NDWH' Docket,'{store}' Store, p.FacilityId,f.Code,f.Name
                            FROM
                                 PatientExtract p inner join
                                 (
                                     SELECT FacilityId,PatientPID,COUNT(Id) AS PCount
                                     FROM PatientExtract
                                     GROUP BY FacilityId, PatientPID
                                     HAVING (COUNT(*) > 1)
                                 ) d on p.PatientPID = d.PatientPID and p.FacilityId = d.FacilityId inner join
                                     Facility f on f.Id = p.FacilityId ";

            if (SourceInfo.DbType == SharedKernel.Enums.DbType.SQLite)
                sql = sql.Replace("ISNULL", "IFNULL");

            return await  GetConnection().QueryAsync<Site>(sql,commandTimeout:0);
        }

        public async Task<IEnumerable<Site>> GetSites(int[] siteCodes)
        {
            var store = GetConnection().Database;

            var sql = $@"
                            SELECT
                                  DISTINCT 'NDWH' Docket,'{store}' Store, p.FacilityId,f.Code,f.Name
                            FROM
                                 PatientExtract p inner join
                                 (
                                     SELECT FacilityId,PatientPID,COUNT(Id) AS PCount
                                     FROM PatientExtract
                                     GROUP BY FacilityId, PatientPID
                                     HAVING (COUNT(*) > 1)
                                 ) d on p.PatientPID = d.PatientPID and p.FacilityId = d.FacilityId inner join
                                     Facility f on f.Id = p.FacilityId 
                          WHERE f.Code IN @siteCodes";

            if (SourceInfo.DbType == SharedKernel.Enums.DbType.SQLite)
                sql = sql.Replace("ISNULL", "IFNULL");

            return await  GetConnection().QueryAsync<Site>(sql,new {siteCodes},commandTimeout:0);;
        }

        public async Task<IEnumerable<Subject>> GetSubjects(Guid facilityId)
        {
            var sql = $@"
                           SELECT
                                   'PatientExtract' Extract,p.PatientPID PatientPk,Id PatientId,p.Created
                            FROM
                                 PatientExtract p inner join
                                 (
                                     SELECT FacilityId,PatientPID,COUNT(Id) AS PCount
                                     FROM PatientExtract
                                     GROUP BY FacilityId, PatientPID
                                     HAVING (COUNT(*) > 1)
                                 ) d on p.PatientPID = d.PatientPID and p.FacilityId = d.FacilityId 
                           WHERE 
                                 p.FacilityId=@facilityId";

            if (SourceInfo.DbType == SharedKernel.Enums.DbType.SQLite)
                sql = sql.Replace("ISNULL", "IFNULL");

            return await GetConnection().QueryAsync<Subject>(sql, new {facilityId}, commandTimeout: 0);
        }

        public async Task<IEnumerable<SubjectExtract>> GetSubjectExtracts(List<Guid> subjectIds)
        {
            var list=new List<SubjectExtract>();

            var extracts=new List<string>
            {
                "PatientArtExtract",
                "PatientBaselinesExtract",
                "PatientStatusExtract",
                "PatientAdverseEventExtract",
                "PatientArtExtract",
                "PatientPharmacyExtract",
                "PatientLaboratoryExtract",
                "PatientVisitExtract"
            };

            foreach (var extract in extracts)
            {
                var sql = $@"
                            select 
                                '{extract}' Extract,PatientId,Id ExtractId,Created 
                            from 
                                 {extract}
                            where 
                                  PatientId in @subjectIds";

                if (SourceInfo.DbType == SharedKernel.Enums.DbType.SQLite)
                    sql = sql.Replace("ISNULL", "IFNULL");

                var  recs=await GetConnection().QueryAsync<SubjectExtract>(sql,new {subjectIds},commandTimeout:0);
                list.AddRange(recs);
            }
            return list;
        }

        public async Task CleanExtract(string extract,Guid candidatePatientId, List<Guid> patientIds, List<Guid> extractIds)
        {
            var sql = $"UPDATE {extract} SET PatientId=@candidatePatientId WHERE PatientId IN @patientIds";

            var delSql = $"DELETE FROM {extract} WHERE Id IN @extractIds";


            using (var con = GetConnection())
            {
                con.Open();
                await con.ExecuteAsync(sql, new {candidatePatientId,patientIds,extractIds}, commandTimeout: 0);
                await con.ExecuteAsync(delSql, new {candidatePatientId,patientIds,extractIds}, commandTimeout: 0);
            }
        }

        public async Task CleanSubject(List<Guid> patientIds)
        {
            var sql = $@"DELETE FROM PatientExtract WHERE Id IN @patientIds";

            using (var con = GetConnection())
            {
                con.Open();
                await con.ExecuteAsync(sql, new {patientIds}, commandTimeout: 0);
            }
        }

        private IDbConnection GetConnection()
        {
            var connectionString = SourceInfo.Connection;

            if (SourceInfo.DbType == SharedKernel.Enums.DbType.MsSQL)
                return new System.Data.SqlClient.SqlConnection(connectionString);

            if (SourceInfo.DbType == SharedKernel.Enums.DbType.SQLite)
                return new SqliteConnection(connectionString);

            return null;
        }
    }
}
