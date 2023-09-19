using Application.Account.Services;
using Microsoft.AspNetCore.Http;
using Mra.Shared.Common.Constants;

namespace Infrastructure.Account.Services;

public class UserHttpContextAccessor : IUserHttpContextAccessor
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserHttpContextAccessor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid GetUserId()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        if (user != null)
        {
            var idClaim = user.FindFirst(ClaimTypes.Id);
            if (idClaim != null && Guid.TryParse(idClaim.Value, out Guid id))
                return id;
        }

        return Guid.Empty;
    }

    public string GetEmail()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        if (user != null)
        {
            var emailClaim = user.FindFirst(ClaimTypes.Email);
            return emailClaim?.Value ?? "";
        }

        return "";
    }

    public string GetUserName()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        if (user != null)
        {
            var emailClaim = user.FindFirst(ClaimTypes.Username);
            return emailClaim?.Value ?? "";
        }

        return "";
    }
}