using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace BusinessLogic.DataTransferObjects.User
{
    public class UserForUpdateDto : UserForManipulationDto
    {
        [Required(ErrorMessage = "Username is a required field.")]
        [MaxLength(60, ErrorMessage = "Maximum length for the Username is 60 characters.")]
        public string Username { get; set; }

        public IFormFile AvatarFile { get; set; }
    }
}