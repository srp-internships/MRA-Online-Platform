using Application.Account.Services;
using Microsoft.AspNetCore.Http;
using Mra.Shared.Common.Constants;

namespace Infrastructure.Account.Services
{
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
                return Guid.Parse(user.FindFirst(ClaimTypes.Id).Value);
            }
            return Guid.Empty;
        }

        public string GetEmail()
        {
            throw new NotImplementedException();
        }

        public string GetUserName()
        {
            throw new NotImplementedException();
        }
    }
}
