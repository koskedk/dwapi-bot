namespace Dwapi.Bot.Core.Domain.Indices.Dto
{
    public class SubjectSiteDto
    {
        public int SiteCode { get; set; }
        public string FacilityName { get; set; }

        public override string ToString()
        {
            return $"{SiteCode},{FacilityName}";
        }
    }
}
