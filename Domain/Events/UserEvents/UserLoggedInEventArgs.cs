using System;
using Domain.Entities;

namespace Domain.Events.UserEvents
{
    public class UserLoggedInEventArgs : EventArgs
    {
        public User User { get; set; }
        public TimeSpan DateTimeSpentFromLastLogin { get; set; }
    }
}