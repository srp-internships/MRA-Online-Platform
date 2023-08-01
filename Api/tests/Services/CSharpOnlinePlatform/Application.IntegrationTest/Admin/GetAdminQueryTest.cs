using System;
using System.Threading.Tasks;
using Application.Admin.Queries;
using Core.Exceptions;
using FluentAssertions;
using NUnit.Framework;
using static Application.IntegrationTest.TestHelper;

namespace Application.IntegrationTest.Admin
{
    public class GetAdminQueryTest
    {
        [Test]
        public void GetAdminQuery_NotExistingAdminId_NotFoundException()
        {
            var command = new GetAdminQuery(Guid.NewGuid());

            ValidationFailureException validationFailureException = Assert.ThrowsAsync<ValidationFailureException>(() => SendAsync(command));

            var notFoundExceptionShown = IsErrorExists("AdminId", "Админ не найден.", validationFailureException);
            notFoundExceptionShown.Should().BeTrue();
        }

        [Test]
        public async Task GetAdminQuery_CorrectData_Admin()
        {
            var admin = new Domain.Entities.Admin() { Id = Guid.NewGuid(), FirstName = "TestName", LastName = "TestName" };
            await AddAsync(admin);
            var command = new GetAdminQuery(admin.Id);

            var adminFromDB = await SendAsync(command);

            adminFromDB.Id.Should().Be(admin.Id);
        }
    }
}
