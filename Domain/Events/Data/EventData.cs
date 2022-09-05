using System;

namespace Domain.Events
{
    public class EventData
    {
        public Guid EventId { get; set; }
        public string Type { get; set; }
        public string Data { get; set; }
        public string MetaData { get; set; }
    }
}