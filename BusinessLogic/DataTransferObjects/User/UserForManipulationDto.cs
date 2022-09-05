using System.ComponentModel.DataAnnotations;


namespace BusinessLogic.DataTransferObjects.User
{
    public class UserForManipulationDto
    {
        [Required(ErrorMessage = "FirstName is a required field.")]
        [MaxLength(60, ErrorMessage = "Maximum length for the FirstName is 60 characters.")]
        public string FirstName { get; set; }
        
        [Required(ErrorMessage = "LastName is a required field.")]
        [MaxLength(60, ErrorMessage = "Maximum length for the LastName is 60 characters.")]
        public string LastName { get; set; }
    }
}