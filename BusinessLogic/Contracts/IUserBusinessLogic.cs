using System;
using System.Threading.Tasks;
using BusinessLogic.DataTransferObjects.User;

namespace BusinessLogic.Contracts
{
    public interface IUserBusinessLogic
    {
        public delegate void UserEventHandler(object sender, EventArgs args, Guid requestId);
        public event UserEventHandler UserEventOccured;
        Task<IBusinessLogicResult<TokensForShowDto>> RegisterUserAsync(UserForRegistrationDto userForRegistrationDto, string userName, Guid requestId);
        Task<IBusinessLogicResult<OtpForShowDto>> PhoneNumberAuthenticationAsync(UserForPhoneAuthDto userForPhoneAuthDto, Guid requestId);
        Task<IBusinessLogicResult<TokensForShowDto>> OtpCodeAuthenticationAsync(UserForOtpAuthDto userForOtpAuthDto, Guid requestId);
        Task<IBusinessLogicResult<TokensForShowDto>> RefreshAsync(TokensForRefreshDto tokensForRefreshDto, Guid requestId);
        Task<IBusinessLogicResult<TokensForShowDto>> UpdateUserAsync(UserForUpdateDto userForUpdateDto, string userName, Guid requestId);
        Task<IBusinessLogicResult<UserForShowDto>> GetUserAsync(string userName, Guid requestId);
        Task<IBusinessLogicResult<ProfileForShowDto>> GetProfileInfoAsync(Guid userId, Guid requestId);

    }
}