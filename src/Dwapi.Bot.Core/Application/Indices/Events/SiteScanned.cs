using MediatR;

namespace Dwapi.Bot.Core.Application.Indices.Events
{
    public class SiteScanned:INotification
    {
        public int SiteCode { get; set; }
        public int Count { get; set; }

        public int TotalCount { get; set; }

        public SiteScanned(int siteCode, int count, int totalCount)
        {
            SiteCode = siteCode;
            Count = count;
            TotalCount = totalCount;
        }
    }
}
