namespace BusinessLogic
{
    public static class BusinessLogicErrors
    {
        public static class Link
        {
            public const string LinkDoesNotExist = "لینک مورد نظر وجود ندارد.";
        } 
        
        public static class LiveStream
        {
            public const string LiveStreamDoesNotExist = "استریم مورد نظر وجود ندارد.";
            public const string ParticipantDoesNotExist = "بیننده ی مورد نظر وجود ندارد.";
        } 
        
        public static class User
        {
            public const string UserDoesNotExist = "کاربر مورد نظر وجود ندارد.";
            public const string UserAlreadyRegistered = "کاربر قبلا ثبت نام شده است.";
            public const string UserDoesNotExistOrRefreshTokenIsInvalid = "کاربر مورد نظر وجود ندارد یا توکن معتبر نمی باشد.";
            public const string UsernameNotAvailable = "نام کاربری قابل استفاده نمی باشد.";
            public const string OtpCodeDoesNotExist = "کد اعتبارسنجی موجود نمی باشد.";
            public const string OtpCodeIsExpired = "کد اعتبارسنجی منقضی شده است. دوباره درخواست دهید.";
            public const string OtpCodeIsInvalid = "کد اعتبارسنجی اشتباه می باشد.";
        } 
    }
}