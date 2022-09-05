using System.ComponentModel.DataAnnotations;

namespace BusinessLogic.DataTransferObjects.User
{
    public class TokensForRefreshDto
    {
        [Required]
        public string AccessToken { get; set; }
        [Required]
        public string RefreshToken { get; set; }
    }
}