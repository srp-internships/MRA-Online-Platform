using Application;
using Application.JWTToken;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.JWTToken
{
    public class JWTManagerRepository : IJWTManagerRepository
    {
        private readonly IConfiguration iconfiguration;
        public JWTManagerRepository(IConfiguration iconfiguration)
        {
            this.iconfiguration = iconfiguration;
        }
        public string GenerateToken(IEnumerable<Claim> claims)
        {
            return GenerateJWTTokens(claims);
        }

        public string GenerateRefreshToken(IEnumerable<Claim> claims)
        {
            return GenerateJWTTokens(claims);
        }

        public string GenerateJWTTokens(IEnumerable<Claim> claims)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(iconfiguration[ApplicationConstants.JWT_SECRET]));
            _ = int.TryParse(iconfiguration[ApplicationConstants.JWT_TOKEN_VALID_IN_MINUTES], out int tokenValidityInMinutes);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = iconfiguration[ApplicationConstants.JWT_VALID_ISSUER],
                Audience = iconfiguration[ApplicationConstants.JWT_VALID_AUDIENCE],
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMinutes(tokenValidityInMinutes),
                SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(iconfiguration[ApplicationConstants.JWT_SECRET])),
                ClockSkew = TimeSpan.Zero
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            JwtSecurityToken jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Недействительный токен.");
            }
            return principal;
        }
    }
}
