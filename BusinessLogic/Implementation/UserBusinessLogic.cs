using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using BusinessLogic.Contracts;
using BusinessLogic.DataTransferObjects.User;
using Contracts.Authentication;
using Contracts.FileHandler;
using Contracts.LoggerManager;
using Contracts.Mapper;
using Contracts.OtpHandler;
using Contracts.Repository;
using Domain.Entities;
using Domain.Events.UserEvents;

namespace BusinessLogic.Implementation
{
    public class UserBusinessLogic : IUserBusinessLogic
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _loggerManager;
        private readonly IJwtTokenCreator _jwtTokenCreator;
        private readonly IOtpHandler _otpHandler;
        private readonly IAvatarHandler _avatarHandler;

        public UserBusinessLogic(IRepositoryManager repositoryManager, IMapper mapper, IJwtTokenCreator jwtTokenCreator,
            ILoggerManager loggerManager, IOtpHandler otpHandler, IAvatarHandler avatarHandler)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
            _jwtTokenCreator = jwtTokenCreator;
            _loggerManager = loggerManager;
            _otpHandler = otpHandler;
            _avatarHandler = avatarHandler;
        }

        public event IUserBusinessLogic.UserEventHandler UserEventOccured;

        public async Task<IBusinessLogicResult<TokensForShowDto>> RegisterUserAsync(
            UserForRegistrationDto userForRegistrationDto, string userName, Guid requestId)
        {
            var user = await _repositoryManager.User.GetUserByUserNameAsync(userName, trackChanges: true);
            if (user.IsRegisterCompleted)
                return new BusinessLogicResult<TokensForShowDto>()
                { Success = false, Error = BusinessLogicErrors.User.UserAlreadyRegistered };

            await _mapper.MapAsync(userForRegistrationDto, user);
            user.CreateDateTime = DateTime.UtcNow;
            user.IsRegisterCompleted = true;
            var tokens = CreatePairTokens(user);
            user.RefreshToken = tokens.RefreshToken;
            user.RefreshTokenExpireTime = DateTime.UtcNow.AddDays(7);
            await _repositoryManager.SaveAsync();
            var newEvent = new UserLoginStepsEventArgs
            {
                User = user,
                Step = UserLoginSteps.Register,
                LastStep = DateTime.UtcNow
            };
            OnDomainEventOccured(newEvent, requestId);
            return new BusinessLogicResult<TokensForShowDto> { Success = true, Error = null, Result = tokens };
        }

        public async Task<IBusinessLogicResult<OtpForShowDto>> PhoneNumberAuthenticationAsync(
            UserForPhoneAuthDto userForPhoneAuthDto, Guid requestId)
        {
            var otp = new OtpCode()
            {
                Id = Guid.NewGuid(),
                Value = new Random().Next(10001, 99999).ToString(),
                CreateDate = DateTime.UtcNow,
                ExpirationDate = DateTime.UtcNow.Add(TimeSpan.FromSeconds(60)),
                IsValid = true
            };

            await _repositoryManager.OtpCode.CreateOtpCodeAsync(otp);
            await _repositoryManager.SaveAsync();
            // var otpRes = await _otpHandler.SendOtpCodeAsync(userForPhoneAuthDto.PhoneNumber, otp.Value);
            // if (!otpRes)
            //     _loggerManager.LogError("otp api failed");

            var user = await _repositoryManager.User.GetUserByPhoneNumberAsync(userForPhoneAuthDto.PhoneNumber,
                trackChanges: false);
            if (user is null)
            {
                user = new User()
                {
                    Id = Guid.NewGuid(),
                    Username = new Random().Next(1001, 9999) + new Random().Next(10000001, 99999999).ToString(),
                    IsRegisterCompleted = false,
                    CreateDateTime = DateTime.UtcNow,
                    PhoneNumber = userForPhoneAuthDto.PhoneNumber,
                    FirstName = "New",
                    LastName = "User"
                };

                await _repositoryManager.User.CreateUserAsync(user);
                await _repositoryManager.SaveAsync();
            }

            var otpForShow = new OtpForShowDto()
            {
                Code = otp.Value,
                ExpirationTime = otp.ExpirationDate,
                PhoneNumber = userForPhoneAuthDto.PhoneNumber,
                IsRegisterCompleted = user.IsRegisterCompleted
            };

            var newEvent = new UserLoginStepsEventArgs
            {
                User = user,
                Step = UserLoginSteps.PhoneNumberAuthentication,
                LastStep = DateTime.UtcNow
            };
            OnDomainEventOccured(newEvent, requestId);
            return new BusinessLogicResult<OtpForShowDto> { Success = true, Result = otpForShow };
        }

        public async Task<IBusinessLogicResult<TokensForShowDto>> OtpCodeAuthenticationAsync(
            UserForOtpAuthDto userForOtpAuthDto, Guid requestId)
        {
            var otpForInvalidation = await
                _repositoryManager.OtpCode.GetOtpCodeByValue(userForOtpAuthDto.Code, trackChanges: true);

            var errors = OtpCodeValidation(otpForInvalidation);
            if (errors is not null)
            {
                var err = new BusinessLogicResult<TokensForShowDto>()
                {
                    Error = errors.Error,
                    Result = null,
                    Success = false
                };
                return err;
            }


            otpForInvalidation.IsValid = false;
            await _repositoryManager.SaveAsync();

            var user = await _repositoryManager.User.GetUserByPhoneNumberAsync(userForOtpAuthDto.PhoneNumber,
                trackChanges: true);

            var time = DateTime.UtcNow;
            var logoutTime = time - user.RefreshTokenExpireTime;
            if (logoutTime > TimeSpan.FromDays(7))
            {
                var newEvent = new UserLoggedInEventArgs()
                {
                    User = user,
                    DateTimeSpentFromLastLogin = logoutTime
                };

                OnDomainEventOccured(newEvent, requestId);
            }

            user.LastLoginDateTime = time;
            var tokens = CreatePairTokens(user);
            user.RefreshToken = tokens.RefreshToken;
            user.RefreshTokenExpireTime = DateTime.UtcNow.AddDays(_jwtTokenCreator.GetRefreshTokenExTime());

            await _repositoryManager.SaveAsync();

            var newEvent2 = new UserLoginStepsEventArgs
            {
                User = user,
                Step = UserLoginSteps.OtpCodeAuthentication,
                LastStep = DateTime.UtcNow
            };
            OnDomainEventOccured(newEvent2, requestId);

            return new BusinessLogicResult<TokensForShowDto>() { Success = true, Result = tokens };
        }

        public async Task<IBusinessLogicResult<TokensForShowDto>> RefreshAsync(TokensForRefreshDto tokensForRefreshDto, Guid requestId)
        {
            var accessToken = tokensForRefreshDto.AccessToken;
            var refreshToken = tokensForRefreshDto.RefreshToken;
            var principal = _jwtTokenCreator.GetPrincipalFromExpiredToken(accessToken);
            var username = principal.Identity.Name; //this is mapped to the Name claim by default
            var user = await _repositoryManager.User.GetUserByUserNameAsync(username, trackChanges: true);
            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpireTime <= DateTime.Now)
            {
                return new BusinessLogicResult<TokensForShowDto>
                {
                    Error = BusinessLogicErrors.User.UserDoesNotExistOrRefreshTokenIsInvalid,
                    Result = null,
                    Success = false
                };
            }

            var tokens = CreatePairTokens(user);
            user.RefreshToken = tokens.RefreshToken;
            user.RefreshTokenExpireTime = DateTime.UtcNow.AddDays(7);
            await _repositoryManager.SaveAsync();
            return new BusinessLogicResult<TokensForShowDto>() { Success = true, Result = tokens };
        }

        public async Task<IBusinessLogicResult<TokensForShowDto>> UpdateUserAsync(UserForUpdateDto userForUpdateDto,
            string userName, Guid requestId)
        {
            var user = await _repositoryManager.User.GetUserByUserNameAsync(userName, trackChanges: true);
            if (user is null)
                return new BusinessLogicResult<TokensForShowDto>()
                { Success = false, Error = BusinessLogicErrors.User.UserDoesNotExist, Result = null };

            var newEvent = new UserUpdatedEventArgs { Before = user };
            if (!userName.Equals(userForUpdateDto.Username))
                if (!await IsUsernameAccessible(userForUpdateDto.Username))
                    return new BusinessLogicResult<TokensForShowDto>()
                    { Success = false, Error = BusinessLogicErrors.User.UsernameNotAvailable };

            await _mapper.MapAsync(userForUpdateDto, user);
            if (userForUpdateDto.AvatarFile is not null)
            {
                user.Avatar = await _avatarHandler.SaveFileAsync(userForUpdateDto.AvatarFile);
            }

            await _repositoryManager.SaveAsync();

            var tokens = CreatePairTokens(user);

            newEvent.After = user;
            OnDomainEventOccured(newEvent, requestId);

            return new BusinessLogicResult<TokensForShowDto> { Success = true, Result = tokens };
        }

        public async Task<IBusinessLogicResult<UserForShowDto>> GetUserAsync(string userName, Guid requestId)
        {
            var user = await _repositoryManager.User.GetUserByUserNameAsync(userName, trackChanges: false);
            if (user is null)
            {
                return new BusinessLogicResult<UserForShowDto>()
                { Success = false, Error = BusinessLogicErrors.User.UserDoesNotExist };
            }

            var userForShow = await _mapper.MapAsync(user, new UserForShowDto());
            return new BusinessLogicResult<UserForShowDto>() { Success = true, Result = userForShow };
        }


        public async Task<IBusinessLogicResult<ProfileForShowDto>> GetProfileInfoAsync(Guid userId, Guid requestId)
        {
            var user = await _repositoryManager.User.GetUserByIdAsync(userId, false);
            if (user is null)
            {
                return new BusinessLogicResult<ProfileForShowDto>
                {
                    Error = BusinessLogicErrors.User.UserDoesNotExist,
                    Success = false
                };
            }

            var userDto = await _mapper.MapAsync(user, new ProfileForShowDto());
            return new BusinessLogicResult<ProfileForShowDto>
            {
                Success = true,
                Result = userDto
            };
        }

        #region privates

        private TokensForShowDto CreatePairTokens(User user)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, user.Username),
                new(ClaimTypes.Role, user.IsAdmin ? "Admin" : "User"),
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            };
            var accessToken = _jwtTokenCreator.CreateAccessToken(claims);
            var refreshToken = _jwtTokenCreator.CreateRefreshToken();
            return new TokensForShowDto() { AccessToken = accessToken, RefreshToken = refreshToken };
        }


        private IBusinessLogicResult OtpCodeValidation(OtpCode otpCode)
        {
            if (otpCode is null)
                return new BusinessLogicResult() { Success = false, Error = BusinessLogicErrors.User.OtpCodeDoesNotExist };

            if (otpCode.IsExpired)
                return new BusinessLogicResult() { Success = false, Error = BusinessLogicErrors.User.OtpCodeIsExpired };

            return !otpCode.IsValid ? new BusinessLogicResult() { Success = false, Error = BusinessLogicErrors.User.OtpCodeIsInvalid } : null;
        }

        private async Task<bool> IsUsernameAccessible(string username)
        {
            var user = await _repositoryManager.User.GetUserByUserNameAsync(username, trackChanges: false);
            return user is null;
        }


        #endregion


        #region protecteds

        protected virtual void OnDomainEventOccured(EventArgs e, Guid requestId)
        {
            UserEventOccured?.Invoke(this, e, requestId);
        }


        #endregion
    }
}