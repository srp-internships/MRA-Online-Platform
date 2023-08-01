using Application.Account.Commands;
using NUnit.Framework;
using System.Threading.Tasks;
using FluentAssertions;
using static Application.IntegrationTest.TestHelper;
using System;

namespace Application.IntegrationTest.Accounts
{
    public class SingUpCommandTest
    {
        [Test]
        public async Task SingUpUserCommand_StudentTestAsync()
        {
            //arrange
            var signUpCommand = GetSignUpCommand();

            //act
            var signUpDTO = await SendAsync(signUpCommand);

            //assert
            signUpDTO.Should().BeTrue();

            //act
            var conf = new SignUpConfirmEmailCommand()
            {
                Email = TestEmailSandbox.Email,
                Token = GetTokenFromBody(TestEmailSandbox.Body)
            };
            var confDOT = await SendAsync(conf);

            //assert
            confDOT.Succeeded.Should().BeTrue();
        }

        public static string GetTokenFromBody(string body)
        {
            var startIndexOfToken = body.IndexOf("token=") + 6;
            var endIndexOfToken = body.IndexOf("&email");
            return body[startIndexOfToken..endIndexOfToken].Replace("%2B", "+");
        }

        #region Test data
        SignUpCommand GetSignUpCommand()
        {
            return new SignUpCommand()
            {
                FirstName = "StudName",
                LastName = "StudLastName",
                Address = "StudAddress",
                DateOfBirth = System.DateTime.Today,
                PhoneNumber = "992927770000",
                City = "Khujand",
                Country = "Tajikistan",
                Email = "singupstudent@gmail.com",
                Occupation = "student",
                Password = "Pw12345@",
                Region = "Sogd",
                CourseId = new Guid("06DFE727-D631-40A3-A4BA-059EB192D8BA")
            };
        }
        #endregion
    }
}
