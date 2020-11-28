using System;

namespace Dwapi.Bot.Core.Domain.Catalogs.Dtos
{
    public class SubjectExtractDto
    {
        public string Extract { get; set; }
        public Guid PatientId { get; set; }
        public Guid? CandidatePatientId { get; set; }
        public Guid ExtractId { get; set; }
        public Guid? PreferredExtractId { get; set; }
        public Guid SiteId { get; set; }
        public DateTime Created { get; set; }
        public int PatientPk { get; set; }
    }
}
