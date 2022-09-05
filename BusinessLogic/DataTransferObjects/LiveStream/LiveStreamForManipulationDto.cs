using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities;

namespace BusinessLogic.DataTransferObjects.LiveStream
{
    public class LiveStreamForManipulationDto
    {
        [Required]
        public string Title { get; set; }
        
        [Required]
        public string MainQuestion { get; set; }
        
        [ForeignKey("World")]
        public Guid WorldId { get; set; }
    }
}