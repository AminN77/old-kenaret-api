using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class LiveStream
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string MainQuestion { get; set; }
        [ForeignKey("Streamer")]
        public Guid StreamerId { get; set; }
        [ForeignKey("World")]
        public Guid WorldId { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public bool IsFinished { get; set; }
        public int GuestsCount { get; set; }
        public virtual User Streamer { get; set; }
        public virtual World World { get; set; }
        public LiveStream()
        {
            
        }
    }
}
