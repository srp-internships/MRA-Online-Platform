using System.Security.Claims;

namespace Application.JWTToken
{
    public interface IJWTManagerRepository
    {
        string GenerateToken(IEnumerable<Claim> claims);
        string GenerateRefreshToken(IEnumerable<Claim> claims);
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
