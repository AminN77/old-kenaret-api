using System;
using Domain.Entities;

namespace Domain.Events.LiveStreamEvents
{
    public class LiveStreamUpdatedEventArgs : EventArgs
    {
        public LiveStream BeforeChange { get; set; }
        public LiveStream AfterChange { get; set; }
    }
}