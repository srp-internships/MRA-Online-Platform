using Application.Account.DTO;
using Application.JWTToken;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace Application.Account.Commands
{
    public class RefreshTokenCommand : IRequest<TokenDTO>
    {
        public TokenDTO Token { get; set; }
    }

    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, TokenDTO>
    {
        private readonly UserManager<User> _userManager;
        private readonly IJWTManagerRepository _jwtManager;
        private readonly IConfiguration iconfiguration;

        public RefreshTokenCommandHandler(UserManager<User> userManager, IJWTManagerRepository jwtManager, IConfiguration iconfiguration)
        {
            _userManager = userManager;
            _jwtManager = jwtManager;
            this.iconfiguration = iconfiguration;
        }

        public async Task<TokenDTO> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var principal = _jwtManager.GetPrincipalFromExpiredToken(request.Token.AccessToken);
            var username = principal.Identity?.Name;
            var user = await _userManager.FindByEmailAsync(username);
            await _userManager.RemoveAuthenticationTokenAsync(user, iconfiguration[ApplicationConstants.JWT_TOKEN_PROVIDER], iconfiguration[ApplicationConstants.JWT_TOKEN_NAME]);
            var newAccessToken = _jwtManager.GenerateToken(principal.Claims);
            var newRefreshToken = await _userManager.GenerateUserTokenAsync(user, iconfiguration[ApplicationConstants.JWT_TOKEN_PROVIDER], iconfiguration[ApplicationConstants.JWT_TOKEN_NAME]);
            await _userManager.SetAuthenticationTokenAsync(user, iconfiguration[ApplicationConstants.JWT_TOKEN_PROVIDER], iconfiguration[ApplicationConstants.JWT_TOKEN_NAME], newRefreshToken);
            return new TokenDTO
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };
        }
    }
}
