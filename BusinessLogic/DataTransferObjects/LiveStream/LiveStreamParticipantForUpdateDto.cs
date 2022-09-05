using System;

namespace BusinessLogic.DataTransferObjects.LiveStream
{
    public class LiveStreamParticipantForUpdateDto
    {
        public Guid UserId { get; set; }
        public bool IsConnected { get; set; }
    }
}