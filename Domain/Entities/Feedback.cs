using System;

namespace Domain.Entities
{
    public class Feedback
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
        public DateTime CreateDateTime { get; set; }
    }
}