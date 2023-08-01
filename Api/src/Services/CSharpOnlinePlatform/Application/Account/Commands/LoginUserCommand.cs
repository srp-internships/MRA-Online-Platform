using Application.Account.DTO;
using Application.JWTToken;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;

namespace Application.Account.Commands
{
    public class LoginUserCommand : IRequest<TokenDTO>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, TokenDTO>
    {
        private readonly UserManager<User> _userManager;
        private readonly IJWTManagerRepository _jwtManager;
        private readonly IConfiguration iconfiguration;
        public LoginUserCommandHandler(UserManager<User> userManager, IJWTManagerRepository jwtManager, IConfiguration iconfiguration)
        {
            _userManager = userManager;
            _jwtManager = jwtManager;
            this.iconfiguration = iconfiguration;
        }

        public async Task<TokenDTO> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            var userRoles = await _userManager.GetRolesAsync(user);
            var authClaims = new List<Claim>
            {
               new Claim(nameof(User.Id), user.Id.ToString()),
               new Claim(ClaimTypes.Email, user.Email),
               new Claim(ClaimTypes.Name, user.UserName),
            };
            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }
            var accessToken = _jwtManager.GenerateToken(authClaims);
            var refreshToken = await _userManager.GenerateUserTokenAsync(user, iconfiguration[ApplicationConstants.JWT_TOKEN_PROVIDER], iconfiguration[ApplicationConstants.JWT_TOKEN_NAME]);
            await _userManager.SetAuthenticationTokenAsync(user, iconfiguration[ApplicationConstants.JWT_TOKEN_PROVIDER], iconfiguration[ApplicationConstants.JWT_TOKEN_NAME], refreshToken);
            return new TokenDTO
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                IsPasswordChanged = user.IsPasswordChanged
            };
        }
    }
}
