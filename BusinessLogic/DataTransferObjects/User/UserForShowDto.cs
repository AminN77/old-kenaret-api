using System;

namespace BusinessLogic.DataTransferObjects.User
{
    public class UserForShowDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Avatar { get; set; }
        public bool IsAdmin { get; set; }
        public DateTime CreateDateTime { get; set; }
        public bool IsRegisterCompleted { get; set; }
    } 
}