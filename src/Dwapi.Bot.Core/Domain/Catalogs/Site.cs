using System;
using System.Collections.Generic;
using Dwapi.Bot.SharedKernel.Enums;
using Dwapi.Bot.SharedKernel.Model;

namespace Dwapi.Bot.Core.Domain.Catalogs
{
    public class Site : Entity<Guid>
    {
        public string Docket { get; set; }
        public string Store { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public Guid FacilityId { get; set; }
        public ICollection<Subject> Subjects { get; set; } = new List<Subject>();
        public ICollection<SubjectExtract> SubjectExtracts { get; set; } = new List<SubjectExtract>();
        public CleanerStatus Status { get; set; }
    }
}
