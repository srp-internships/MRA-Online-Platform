using Infrastructure.Account.Services;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using System;
using System.Security.Claims;
using ClaimTypes = MRA.Configurations.Common.Constants.ClaimTypes;

namespace Infrastructure.Test.Account.Services
{
    public class UserHttpContextAccessorTests
    {
        [Test]
        public void UserHttpContextAccessor_ShouldReturnEmpty_WhenUserDoesNotExistsTest()
        {
            var httpContextMock = new Mock<HttpContext>();
            var httpContextAccessorMock = MockHttpContextAccessor(httpContextMock.Object);
            var userHttpContextAccessor = new UserHttpContextAccessor(httpContextAccessorMock.Object);

            var userId = userHttpContextAccessor.GetUserId();
            Assert.That(userId, Is.Empty);
        }

        [Test]
        public void UserHttpContextAccessor_ShouldReturnEmpty_WhenUserClaimDoesNotExistsTest()
        {
            var userMock = new Mock<ClaimsPrincipal>();

            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.Setup(s => s.User).Returns(userMock.Object);

            var httpContextAccessorMock = MockHttpContextAccessor(httpContextMock.Object);
            var userHttpContextAccessor = new UserHttpContextAccessor(httpContextAccessorMock.Object);

            var userId = userHttpContextAccessor.GetUserId();
            Assert.That(userId, Is.Empty);
        }

        [Test]
        public void UserHttpContextAccessor_ShouldReturnEmpty_WhenUserClaimIsNotGuidTest()
        {
            var userMock = new Mock<ClaimsPrincipal>();
            userMock.Setup(u => u.FindFirst(ClaimTypes.Id)).Returns(new Claim(ClaimTypes.Id, "Not Guid"));

            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.Setup(s => s.User).Returns(userMock.Object);

            var httpContextAccessorMock = MockHttpContextAccessor(httpContextMock.Object);
            var userHttpContextAccessor = new UserHttpContextAccessor(httpContextAccessorMock.Object);

            var userId = userHttpContextAccessor.GetUserId();
            Assert.That(userId, Is.Empty);
        }

        [Test]
        public void UserHttpContextAccessor_ShouldReturnGuid_WhenUserClaimContainsIdClaimWithGuidValueTest()
        {
            var userGuid = Guid.NewGuid();

            var userMock = new Mock<ClaimsPrincipal>();
            userMock.Setup(u => u.FindFirst(ClaimTypes.Id)).Returns(new Claim(ClaimTypes.Id, userGuid.ToString()));

            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.Setup(s => s.User).Returns(userMock.Object);

            var httpContextAccessorMock = MockHttpContextAccessor(httpContextMock.Object);
            var userHttpContextAccessor = new UserHttpContextAccessor(httpContextAccessorMock.Object);

            var userId = userHttpContextAccessor.GetUserId();
            Assert.That(userId, Is.Not.Null);
            Assert.That(userId, Is.EqualTo(userGuid));
        }

        Mock<IHttpContextAccessor> MockHttpContextAccessor(HttpContext httpContext)
        {
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);
            return httpContextAccessorMock;
        }
    }
}
