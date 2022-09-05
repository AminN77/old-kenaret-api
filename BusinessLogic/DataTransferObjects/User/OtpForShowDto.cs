using System;

namespace BusinessLogic.DataTransferObjects.User
{
    public class OtpForShowDto
    {
        public string PhoneNumber { get; set; }
        public bool IsRegisterCompleted { get; set; }
        public string Code { get; set; }
        public DateTime ExpirationTime { get; set; }
    }
}