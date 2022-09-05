using System.ComponentModel.DataAnnotations;

namespace BusinessLogic.DataTransferObjects.User
{
    public class UserForAuthDto
    {
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
    }
}