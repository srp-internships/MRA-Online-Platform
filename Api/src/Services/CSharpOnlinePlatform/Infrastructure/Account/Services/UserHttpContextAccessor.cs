using Application.Account.Services;
using Microsoft.AspNetCore.Http;

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
                //todo
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
