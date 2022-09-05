using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class OtpCode
    {
        [Key]
        public Guid Id { get; set; }

        [MaxLength(6)]
        [MinLength(5)]
        public string? Value { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime ExpirationDate { get; set; }

        public bool IsValid { get; set; }

        public bool IsExpired => DateTime.UtcNow > ExpirationDate ? true : false;

        public OtpCode()
        {

        }
    }
}