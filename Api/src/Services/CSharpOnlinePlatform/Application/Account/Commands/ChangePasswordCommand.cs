using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Text.Json;

namespace Application.Account.Commands
{
    public class ChangePasswordCommand : IRequest<IdentityResult>
    {
        public string Email { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }

    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, IdentityResult>
    {
        readonly UserManager<User> _userManager;

        public ChangePasswordCommandHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IdentityResult> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            user.IsPasswordChanged = true;
            return await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
        }
    }
}
