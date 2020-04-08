using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Dwapi.Bot.Core.Utility;

namespace Dwapi.Bot.Core.Domain.Indices
{
    public class PatientIndex
    {
        public Guid Id { get;  set; }
        private Guid MpiId { get; set; }
        public int PatientPk { get; set; }
        public int SiteCode { get; set; }
        public string FacilityName { get; set; }
        public string Serial { get; set; }
        public string Gender { get; set; }
        public DateTime DOB { get; set; }
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
        public string sxdmPKValueDoB { get; set; }
        public Guid FacilityId { get; set; }
        public ICollection<PatientIndexSiteScore> SiteScores { get; set; }=new List<PatientIndexSiteScore>();
        public ICollection<PatientIndexInterSiteScore> InterSiteScores { get; set; }=new List<PatientIndexInterSiteScore>();

        public PatientIndex()
        {
            Id = LiveGuid.NewGuid();
        }
    }
}
