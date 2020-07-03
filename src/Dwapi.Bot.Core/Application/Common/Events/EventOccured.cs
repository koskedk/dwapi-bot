using MediatR;

namespace Dwapi.Bot.Core.Application.Common.Events
{
    public class EventOccured:INotification
    {
        public string Event { get; }
        public string EventCode { get; }
        public int Progress  { get; }
        public long Count { get; }
        public long Total { get; }

        public EventOccured(string eventCode,string @event)
        {
            EventCode = eventCode;
            Event = @event;
        }

        public EventOccured(string eventCode,string @event, long total):this(eventCode,@event)
        {
            Total = total;
        }

        public EventOccured(string eventCode,string @event,  int progress):this(eventCode,@event)
        {
            Progress = progress;
        }

        public EventOccured(string eventCode,string @event,  long count, long total):this(eventCode,@event)
        {
            Count = count;
            Total = total;
        }
    }
}
