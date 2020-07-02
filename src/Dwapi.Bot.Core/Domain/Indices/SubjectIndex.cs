using System;
using System.Collections.Generic;
using Dwapi.Bot.SharedKernel.Model;

namespace Dwapi.Bot.Core.Domain.Indices
{
    public class SubjectIndex:Entity<Guid>
    {
        public Guid MpiId { get; set; }
        public int PatientPk { get; set; }
        public int SiteCode { get; set; }
        public string FacilityName { get; set; }
        public string Serial { get; set; }
        public string Gender { get; set; }
        public DateTime? DOB { get; set; }
        public string PatientID { get; set; }

        public string dmFirstName { get; set; }
        public string dmMiddleName { get; set; }
        public string dmLastName { get; set; }

        public string sxFirstName { get; set; }
        public string sxLastName { get; set; }
        public string sxMiddleName { get; set; }

        public string sxPKValue { get; set; }
        public string dmPKValue { get; set; }
        public string sxdmPKValue { get; set; }
        public string sxPKValueDoB { get; set; }
        public string dmPKValueDoB { get; set; }
        /// <summary>
        /// The PKV
        /// </summary>
        public string sxdmPKValueDoB { get; set; }
        public Guid? FacilityId { get; set; }
        public int RowId { get; set; }
        public Guid? SiteBlockId { get; set; }
        public Guid? InterSiteBlockId { get; set; }
        public ICollection<SubjectIndexScore> IndexScores { get; set; }=new List<SubjectIndexScore>();
        public ICollection<SubjectIndexStage> IndexStages { get; set; }=new List<SubjectIndexStage>();

        public override string ToString()
        {
            return $"{SiteCode}|{Serial}|{sxdmPKValueDoB}";
        }
    }
}
