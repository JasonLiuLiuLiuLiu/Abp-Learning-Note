using Abp.Events.Bus.Entities;

namespace EventCloud.Event
{
    public class EventCancelledEvent : EntityEventData<Event>
    {
        public EventCancelledEvent(Event entity)
            : base(entity)
        {
        }
    }
}