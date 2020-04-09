using MediatR;

namespace Dwapi.Bot.Core.Application.Indices.Events
{
    public class InterSiteScanned:INotification
    {
    
        public int Count { get; set; }

        public int TotalCount { get; set; }

        public InterSiteScanned(int count, int totalCount)
        {
            Count = count;
            TotalCount = totalCount;
        }
    }
}