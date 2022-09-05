using System;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enums;

namespace Domain.Entities
{
    public class Participants
    {
        public Guid UserId { get; set; }
        public Guid LiveStreamId { get; set; }
        public bool IsConnected { get; set; }
        public ParticipantPermissions Permission { get; set; }

        public DateTime LastStatusChangeDateTime { get; set; }
        public TimeSpan TotalDuration { get; set; }

        public Participants()
        {
            
        }
    }
}