using Application.Account.Commands;
using Application.Admin.Commands.StudentCommand;
using NUnit.Framework;
using System.Threading.Tasks;
using FluentAssertions;
using static Application.IntegrationTest.TestHelper;

namespace Application.IntegrationTest.Accounts
{
    public class ChangePasswordTest
    {
        [Test]
        public async Task ChangePassword_PasswordTestAsync()
        {
            var testStudent = GetStudentCommand("daniel@gmail.com", "Pw12345@");
            await SendAsync(testStudent);

            var login = new LoginUserCommand()
            {
                Email = testStudent.Email,
                Password = testStudent.Password
            };
            var passwordResult = await SendAsync(login);

            Assert.That(passwordResult.AccessToken, Is.Not.Empty);
            Assert.That(passwordResult.RefreshToken, Is.Not.Empty);

            ChangePasswordCommand changePassword = new ChangePasswordCommand();
            changePassword.Email = testStudent.Email;
            changePassword.CurrentPassword = testStudent.Password;
            changePassword.NewPassword = "Test-1234";

            var change = await SendAsync(changePassword);
            Assert.True(change.Succeeded);

            login = new LoginUserCommand()
            {
                Email = testStudent.Email,
                Password = "Test-1234"
            };
            passwordResult = await SendAsync(login);

            Assert.That(passwordResult.AccessToken, Is.Not.Empty);
            Assert.That(passwordResult.RefreshToken, Is.Not.Empty);
        }

        [Test]
        public async Task ChangePassword_SuggestUserPasswordTestAsync()
        {
            var testStudent = GetStudentCommand("vali@mail.ru", "Pass1234$");
            await SendAsync(testStudent);

            var login = new LoginUserCommand()
            {
                Email = testStudent.Email,
                Password = testStudent.Password
            };
            var passwordResult = await SendAsync(login);

            passwordResult.IsPasswordChanged.Should().BeFalse();

            ChangePasswordCommand changePassword = new ChangePasswordCommand();
            changePassword.Email = testStudent.Email;
            changePassword.CurrentPassword = testStudent.Password;
            changePassword.NewPassword = "Test-1234";

            var change = await SendAsync(changePassword);
            change.Succeeded.Should().BeTrue();

            login = new LoginUserCommand()
            {
                Email = testStudent.Email,
                Password = "Test-1234"
            };
            passwordResult = await SendAsync(login);

            passwordResult.IsPasswordChanged.Should().BeTrue();
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
