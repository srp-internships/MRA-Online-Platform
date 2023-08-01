using Application.Account.Services;
using Microsoft.Extensions.Configuration;
using Azure.Communication.Email.Models;
using Azure.Communication.Email;
using Microsoft.Extensions.Logging;
using Application;

namespace Infrastructure.Account.Services
{
    public class EmailSenderService : IEmailSenderService
    {
        private readonly IConfiguration _configuration;
        private readonly EmailClient client;
        private readonly ILogger _logger;
        public EmailSenderService(IConfiguration configuration, ILogger<EmailSenderService> logger)
        {
            _configuration = configuration;
            var connectionString = _configuration[ApplicationConstants.AZURE_EMAIL_CONNECTION]; // Find your Communication Services resource in the Azure portal
            client = new EmailClient(connectionString);
            _logger = logger;
        }

        public bool SendEmail(string emailTo, string body, string subject)
        {
            // Create the email content
            var emailContent = new EmailContent(subject);
            emailContent.Html = body;

            // Create the recipient list
            var emailRecipients = new EmailRecipients(
                new List<EmailAddress>
                {
                    new EmailAddress(emailTo)
                });

            // Create the EmailMessage
            var emailMessage = new EmailMessage(
                sender: _configuration[ApplicationConstants.AZURE_EMAIL_SENDER],
                emailContent,
                emailRecipients);

            try
            {
                client.Send(emailMessage);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error on sending email");
                return false;
            }
        }
    }
}
