using System;
using Domain.Entities;

namespace Domain.Events.UserEvents
{
    public class UserUpdatedEventArgs : EventArgs
    {
        public User Before { get; set; }
        public User After { get; set; }
    }
}