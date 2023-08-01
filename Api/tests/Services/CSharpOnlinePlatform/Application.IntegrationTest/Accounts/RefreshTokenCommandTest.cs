using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Account.Commands;
using Application.Admin.Commands.StudentCommand;
using FluentAssertions;
using NUnit.Framework;
using static Application.IntegrationTest.TestHelper;

namespace Application.IntegrationTest.Accounts
{
    public class RefreshTokenCommandTest
    {
        [Test]
        public async Task RefreshTokenCommand_CorrectData_Tokens()
        {
            var testStudent = GetStudentCommand("danielqweqwe@gmail.com", "Pw12345@");
            await SendAsync(testStudent);
            var login = new LoginUserCommand()
            {
                Email = testStudent.Email,
                Password = testStudent.Password
            };
            var loginResult = await SendAsync(login);
            var command = new RefreshTokenCommand() { Token = loginResult };

            var refreshTokenResult = await SendAsync(command);

            refreshTokenResult.AccessToken.Should().NotBeEmpty();
            refreshTokenResult.AccessToken.Should().NotBeEmpty();
        }

        CreateStudentCommand GetStudentCommand(string email, string password)
        {
            return new CreateStudentCommand()
            {
                FirstName = $"Daniel {email}",
                LastName = $"Glick {email}",
                Address = "PA, Lancaster",
                BirthDate = System.DateTime.Today,
                PhoneNumber = "992927770000",
                City = "Khujand",
                Country = "Tajikistan",
                Email = email,
                Occupation = "student",
                Password = password,
                Region = "Sogd",
                CourseName = "C# for beginners"
            };
        }
    }
}
