using Application.Account.Commands;
using Application.Admin.Commands.StudentCommand;
using NUnit.Framework;
using System.Threading.Tasks;
using FluentAssertions;
using static Application.IntegrationTest.TestHelper;

namespace Application.IntegrationTest.Accounts
{
    public class ResetPasswordTest
    {
        [Test]
        public async Task ForgotAndResetPassword_StudentTestAsync()
        {
            var testStudent = CreateStudentCommand("srpacademy@mail.ru", "Jon_98");
            await SendAsync(testStudent);

            var forgotPassword = new ForgotPasswordCommand()
            {
                Email = testStudent.Email
            };
            var forgotResult = await SendAsync(forgotPassword);
            forgotResult.Should().BeTrue();

            var resetPassword = new ResetPasswordCommand()
            {
                Email = TestEmailSandbox.Email,
                Token = SingUpCommandTest.GetTokenFromBody(TestEmailSandbox.Body),
                NewPassword = "John_9898",
            };
            var resetResult = await SendAsync(resetPassword);
            resetResult.Succeeded.Should().BeTrue();

            testStudent.Password = resetPassword.NewPassword;
            await LoginCommand(testStudent);
        }

        private static async Task LoginCommand(CreateStudentCommand testStudent)
        {
            var login = new LoginUserCommand()
            {
                Email = testStudent.Email,
                Password = testStudent.Password
            };
            var loginResult = await SendAsync(login);
            loginResult.AccessToken.Should().NotBeEmpty();
            loginResult.RefreshToken.Should().NotBeEmpty();
        }

        #region Test data
        CreateStudentCommand CreateStudentCommand(string email, string password)
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
        #endregion
    }
}