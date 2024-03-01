namespace Application.Account.Services
{
    public interface IUserHttpContextAccessor
    {
        Guid GetUserId();
        string GetEmail();
        string GetUserName();
    }
}
