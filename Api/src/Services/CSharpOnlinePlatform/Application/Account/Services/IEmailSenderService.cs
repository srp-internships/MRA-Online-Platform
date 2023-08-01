namespace Application.Account.Services
{
    public interface IEmailSenderService
    {
        public bool SendEmail(string emailTo, string body, string subject);
    }
}
