using System;
using System.ComponentModel.DataAnnotations.Schema;
using BusinessLogic.DataTransferObjects.Link;

namespace BusinessLogic.DataTransferObjects.LiveStream
{
    public class LiveStreamForCreationDto : LiveStreamForManipulationDto
    {
        public LinkForCreationDto[] Links { get; set; }
    }
}