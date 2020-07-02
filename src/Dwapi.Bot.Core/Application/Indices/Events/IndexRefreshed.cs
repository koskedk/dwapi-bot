using Dwapi.Bot.Core.Domain.Indices.Dto;
using MediatR;

namespace Dwapi.Bot.Core.Application.Indices.Events
{
    public class IndexRefreshed:INotification
    {
        public int Count { get; set; }

        public int TotalCount { get; set; }

        public SubjectSiteDto Site  { get; set; }

        public int SiteCount { get; set; }

        public int TotalSiteCount { get; set; }

        public IndexRefreshed(int count, int totalCount)
        {
            Count = count;
            TotalCount = totalCount;
        }
    }
}
