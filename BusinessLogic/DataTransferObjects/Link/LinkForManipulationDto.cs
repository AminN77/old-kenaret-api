using System.ComponentModel.DataAnnotations;

namespace BusinessLogic.DataTransferObjects.Link
{
    public class LinkForManipulationDto
    {
        [Required]
        [MaxLength(400)]
        public string Description { get; set; }
        [Required]
        public string Url { get; set; }
    }
}