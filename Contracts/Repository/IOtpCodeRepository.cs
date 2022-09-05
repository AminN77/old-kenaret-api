using System;
using System.Threading.Tasks;
using Domain.Entities;

namespace Contracts.Repository
{
    public interface IOtpCodeRepository
    {
        void CreateOtpCode(OtpCode otpCode);
        Task CreateOtpCodeAsync(OtpCode otpCode);
        Task<OtpCode> GetOtpCodeByValue(string value, bool trackChanges);
    }
}