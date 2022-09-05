using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class World
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string? Title { get; set; }
        public DateTime CreateDateTime { get; set; }
        public virtual ICollection<LiveStream> LiveStreams { get; set; }
        public World() { }
    }
}
