using System;

namespace Dwapi.Bot.Core.Domain.Catalogs.Dtos
{
    public class SiteDto
    {
        public string Docket { get; set; }
        public string Store { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public Guid FacilityId { get; set; }
    }
}
