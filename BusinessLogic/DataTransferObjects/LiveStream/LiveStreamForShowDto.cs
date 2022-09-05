using System;
using BusinessLogic.DataTransferObjects.Link;

namespace BusinessLogic.DataTransferObjects.LiveStream
{
    public class LiveStreamForShowDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string MainQuestion { get; set; }
        public string StreamerUsername { get; set; }
        public string StreamerFullName { get; set; }
        public string StreamerAvatarAddress { get; set; }
        public string WorldTitle{ get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public bool IsFinished { get; set; }
        public int GuestsCount { get; set; }
        public LinkForShowDto[] Nodes { get; set; }
    }
}