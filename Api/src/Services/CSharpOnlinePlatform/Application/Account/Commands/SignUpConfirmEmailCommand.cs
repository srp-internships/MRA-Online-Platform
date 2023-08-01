using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace Application.Account.Commands
{
    public class SignUpConfirmEmailCommand : IRequest<IdentityResult>
    {
        public string Email { get; set; }
        public string Token { get; set; }
    }

    public class SignUpConfirmEmailCommandHandler : IRequestHandler<SignUpConfirmEmailCommand, IdentityResult>
    {
        readonly UserManager<User> _userManager;

        public SignUpConfirmEmailCommandHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IdentityResult> Handle(SignUpConfirmEmailCommand command, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(command.Email);

            var codeDecodedBytes = WebEncoders.Base64UrlDecode(command.Token);
            var codeDecoded = Encoding.UTF8.GetString(codeDecodedBytes);

            var result = await _userManager.ConfirmEmailAsync(user, codeDecoded);
            if (result.Succeeded)
            {
                user.EmailConfirmed = true;
                user.IsPasswordChanged = true;
                await _userManager.UpdateAsync(user);
            }
            return result;
        }
    }
}
