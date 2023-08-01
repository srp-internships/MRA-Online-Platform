using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Text.Json;

namespace Application.Account.Commands
{
    public class ResetPasswordCommand : IRequest<IdentityResult>
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }

    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, IdentityResult>
    {
        readonly UserManager<User> _userManager;

        public ResetPasswordCommandHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IdentityResult> Handle(ResetPasswordCommand command, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(command.Email);
            user.IsPasswordChanged = true;
            return await _userManager.ResetPasswordAsync(user, command.Token, command.NewPassword);
        }
    }
}
