using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Contracts.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Authentication
{
    public class JwtTokenCreator : IJwtTokenCreator
    {
        private readonly IConfiguration _configuration;

        public JwtTokenCreator(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string CreateAccessToken(IEnumerable<Claim> claims)
        {
            var mySecretKey = Encoding.UTF8.GetBytes(_configuration["AuthenticationOptions:SecretKey"]);
            var exTime = _configuration["AuthenticationOptions:AccessTokenExpireTimeInMinutes"];
            if (string.IsNullOrWhiteSpace(exTime))
                exTime = "10";

            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(mySecretKey),
                SecurityAlgorithms.HmacSha256Signature);
            var descriptor = new SecurityTokenDescriptor
            {
                Issuer = _configuration["AuthenticationOptions:Issuer"],
                Audience = _configuration["AuthenticationOptions:Audience"],
                IssuedAt = DateTime.UtcNow,
                NotBefore = DateTime.UtcNow,
                Expires = DateTime.Now.AddMinutes(double.Parse(exTime)),
                SigningCredentials = signingCredentials,
                Subject = new ClaimsIdentity(claims)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(descriptor);
            var accessToken = tokenHandler.WriteToken(securityToken);
            return accessToken;
        }

        public string CreateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            var exTime = _configuration["AuthenticationOptions:RefreshTokenExpireTimeInMinutes"];
            if (string.IsNullOrWhiteSpace(exTime))
                exTime = "7";
            return Convert.ToBase64String(randomNumber);
        }

        public double GetRefreshTokenExTime()
        {
            var exTime = _configuration["AuthenticationOptions:RefreshTokenExpireTimeInMinutes"];
            if (string.IsNullOrWhiteSpace(exTime))
                exTime = "7";
            return double.Parse(exTime);
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ClockSkew = TimeSpan.Zero, // default: 5 min
                RequireSignedTokens = true,

                ValidateIssuerSigningKey = true,
                IssuerSigningKey =
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AuthenticationOptions:SecretKey"])),

                RequireExpirationTime = true,
                ValidateLifetime = false,

                ValidateAudience = true, //default : false
                ValidAudience = _configuration["AuthenticationOptions:Audience"],

                ValidateIssuer = true, //default : false
                ValidIssuer = _configuration["AuthenticationOptions:Issuer"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");
            return principal;
        }

        public bool IsAdminToken(string token)
        {
            var principals = GetPrincipalFromExpiredToken(token);
            var role = principals.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Role);
            return role.Value.Equals("Admin");
        }
    }
}