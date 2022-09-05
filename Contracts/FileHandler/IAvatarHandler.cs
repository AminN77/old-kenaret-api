using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Contracts.FileHandler
{
    public interface IAvatarHandler
    {
        Task<string> SaveFileAsync(IFormFile avatar);
    }
}