using System;
using Domain.Entities;

namespace Domain.Events.UserEvents
{
    public class UserLoginStepsEventArgs :  EventArgs

    {
        public User User { get; set; }
        public UserLoginSteps Step { get; set; }
        public DateTime LastStep { get; set; }
    }

    public enum UserLoginSteps
    {
        PhoneNumberAuthentication = 1,
        OtpCodeAuthentication = 2,
        Register = 3
    }
}