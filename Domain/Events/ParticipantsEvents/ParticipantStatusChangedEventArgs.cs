using System;
using Domain.Entities;

namespace Domain.Events.ParticipantsEvents
{
    public class ParticipantStatusChangedEventArgs : EventArgs
    {
        public Participants Participant { get; set; }
        public DateTime JoinDateTime { get; set; }
        public DateTime LeaveDateTime { get; set; }
        public TimeSpan Duration { get; set; }
    }
}