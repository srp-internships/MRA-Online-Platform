using Application.Account.Services;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace Application.Account.Commands
{
    public class ForgotPasswordCommand : IRequest<bool>
    {
        public string Email { get; set; }
    }

    public class ForgotPasswordComandHandler : IRequestHandler<ForgotPasswordCommand, bool>
    {
        readonly IEmailSenderService _emailService;
        readonly UserManager<User> _userManager;
        readonly IConfiguration _configuration;

        public ForgotPasswordComandHandler(UserManager<User> userManager, IEmailSenderService emailService, IConfiguration configuration)
        {
            _userManager = userManager;
            _emailService = emailService;
            _configuration = configuration;
        }

        public async Task<bool> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            string body = GetBody(user.Email, resetToken);
            string subject = _configuration[ApplicationConstants.RESET_EMAIL_SUBJECT];
            return _emailService.SendEmail(request.Email, body, subject);
        }

        string GetBody(string emailTo, string resetToken)
        {
            var link = $"{_configuration[ApplicationConstants.RESET_LINK]}?token={resetToken}&email={emailTo}".Replace("+", "%2B");
            return $"<html><head></head><body>{_configuration[ApplicationConstants.RESET_MESSAGE]}</br><a href=\"{link}\">Восстановить</a></body></html>";
        }
    }
}
