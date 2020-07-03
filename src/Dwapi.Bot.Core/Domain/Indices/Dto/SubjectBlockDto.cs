namespace Dwapi.Bot.Core.Domain.Indices.Dto
{
    public class SubjectBlockDto
    {
        public int BirthYear { get; set; }
        public string Gender { get; set; }
        public int SiteCode { get; set; }
        public int BlockCount { get; set; }

        public override string ToString()
        {
            return $"{BirthYear},{Gender},{BlockCount},{(SiteCode>0?SiteCode.ToString():"")}";
        }
    }
}
