using Application.Account.Services;

namespace Application.IntegrationTest
{
    public static class TestEmailSandbox
    {
        public static string Body { get; set; }
        public static string Email { get; set; }
    }

    public class TestEmailSenderService : IEmailSenderService
    {
        public bool SendEmail(string emailTo, string body, string subject)
        {
            TestEmailSandbox.Email = emailTo;
            TestEmailSandbox.Body = body;
            return true;
        }
    }
}
