using System;
using System.ComponentModel.DataAnnotations.Schema;
using Dwapi.Bot.SharedKernel.Model;

namespace Dwapi.Bot.Core.Domain.Catalogs
{
    public class Subject:Entity<Guid>
    {
        public string Extract { get; set; }
        public int PatientPk { get; set; }
        public Guid PatientId { get; set; }
        public Guid? PreferredPatientId { get; set; }
        public Guid SiteId { get; set; }
        public DateTime Created { get; set; }

        [NotMapped]
        public bool IsPreffed =>
            PreferredPatientId.HasValue && PreferredPatientId.Value == PatientId;
        public void AssignPreferred(Guid patientId)
        {
            PreferredPatientId = patientId;
        }
    }
}
