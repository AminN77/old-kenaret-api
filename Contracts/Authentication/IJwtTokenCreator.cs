using System.Collections.Generic;
using System.Security.Claims;
using Domain.Entities;

namespace Contracts.Authentication
{
    public interface IJwtTokenCreator
    {
        string CreateAccessToken(IEnumerable<Claim> claims);
        string CreateRefreshToken();
        double GetRefreshTokenExTime();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
        bool IsAdminToken(string token);
    }
}