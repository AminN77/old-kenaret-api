using System.ComponentModel.DataAnnotations;

namespace BusinessLogic.DataTransferObjects.User
{
    public class UserForOtpAuthDto : UserForPhoneAuthDto
    {
        [Required]
        [MaxLength(6)]
        [MinLength(5)]
        public string Code { get; set; }
    }
}