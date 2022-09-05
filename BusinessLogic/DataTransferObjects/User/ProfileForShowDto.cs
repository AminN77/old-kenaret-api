using System;

namespace BusinessLogic.DataTransferObjects.User
{
    public class ProfileForShowDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Avatar { get; set; }
    } 
}