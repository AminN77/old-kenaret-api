using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string? Username { get; set; }
        [Required]
        public string? FirstName { get; set; }
        [Required]
        public string? LastName { get; set; }
        [Required]
        public string? PhoneNumber { get; set; }
        public string? Avatar { get; set; }
        public bool IsAdmin { get; set; }
        [Required]
        public DateTime CreateDateTime { get; set; }

        public DateTime LastLoginDateTime { get; set; }
        public bool IsRegisterCompleted { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpireTime { get; set; }
        public virtual ICollection<LiveStream> LiveStreams { get; set; }
        public User()
        {

        }
    }
}
