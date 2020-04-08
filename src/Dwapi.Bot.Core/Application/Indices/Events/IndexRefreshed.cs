using MediatR;

namespace Dwapi.Bot.Core.Application.Indices.Events
{
    public class IndexRefreshed:INotification
    {
        public int Count { get; set; }

        public int TotalCount { get; set; }

        public IndexRefreshed(int count, int totalCount)
        {
            Count = count;
            TotalCount = totalCount;
        }
    }
}
