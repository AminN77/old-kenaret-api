using System.Threading.Tasks;

namespace Contracts.OtpHandler
{
    public interface IOtpHandler
    {
        Task<bool> SendOtpCodeAsync(string receptor, string code);
    }
}