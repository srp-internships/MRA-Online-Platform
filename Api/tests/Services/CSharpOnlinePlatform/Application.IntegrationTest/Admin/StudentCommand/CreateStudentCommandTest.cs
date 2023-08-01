using Application.Admin.Commands.StudentCommand;
using Core.Exceptions;
using Core.ValidationsBehaviours;
using Domain.Entities;
using FluentAssertions;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;
using static Application.IntegrationTest.TestHelper;

namespace Application.IntegrationTest.Admin.StudentCommand
{
    public class CreateStudentCommandTest
    {
        [Test]
        public async Task CreateStudentCommand_SetAllRequiredFields_DontShowMessage()
        {
            var testStudent = GetStudentCommand();
            testStudent.Email = "test@test.test";
            var studentFromDB = await GetAsync<Student>(s => s.Email == testStudent.Email);
            studentFromDB.Should().BeNull();
            var contact = await GetAsync<Contact>(c => c.User == studentFromDB);
            contact.Should().BeNull();

            studentFromDB = await GetAsync<Student>(s => s.Email == testStudent.Email);
            studentFromDB.Should().BeNull();
            contact = await GetAsync<Contact>(c => c.User == studentFromDB);
            contact.Should().BeNull();

            var studentId = await SendAsync(testStudent);
            FluentActions.Invoking(() => studentId).Should().NotThrow<ValidationFailureException>();

            studentFromDB = await GetAsync<Student>(s => s.Email == testStudent.Email);
            studentFromDB.Should().NotBeNull();
            contact = await GetAsync<Contact>(c => c.User == studentFromDB);
            contact.Should().NotBeNull();
        }

        [Test]
        public void CreateStudentCommand_FirstNameMustNotBeEmptyTest()
        {
            var testStudent = GetStudentCommand();

            testStudent.FirstName = null;
            ValidationFailureException validationError = Assert.ThrowsAsync<ValidationFailureException>(() => SendAsync(testStudent));
            var firstNameMustNotBeEmptyErrorWasShown = IsErrorExists("FirstName", ValidationMessages.GetNotEmptyMessage("First Name"), validationError);

            Assert.That(firstNameMustNotBeEmptyErrorWasShown, Is.True);
        }

        [Test]
        public void CreateStudentCommand_LastNameMustNotBeEmptyTest()
        {
            var testStudent = GetStudentCommand();

            testStudent.LastName = null;
            ValidationFailureException validationError = Assert.ThrowsAsync<ValidationFailureException>(() => SendAsync(testStudent));
            var lastNameMustNotBeEmptyErrorWasShown = IsErrorExists("LastName", ValidationMessages.GetNotEmptyMessage("Last Name"), validationError);

            Assert.That(lastNameMustNotBeEmptyErrorWasShown, Is.True);
        }

        [Test]
        public void CreateStudentCommand_EmailTest()
        {
            var testStudent = GetStudentCommand();

            testStudent.Email = null;
            ValidationFailureException validationError = Assert.ThrowsAsync<ValidationFailureException>(() => SendAsync(testStudent));
            var errorExists = IsErrorExists("Email", ValidationMessages.GetNotEmptyMessage("Email"), validationError);
            Assert.That(errorExists, Is.True);


            testStudent.Email = "email";
            validationError = Assert.ThrowsAsync<ValidationFailureException>(() => SendAsync(testStudent));
            errorExists = IsErrorExists("Email", ValidationMessages.GetEmailAddressMessage(), validationError);
            Assert.That(errorExists, Is.True);

            testStudent.Email = "dilshod@mail.ru";
            validationError = Assert.ThrowsAsync<ValidationFailureException>(() => SendAsync(testStudent));
            errorExists = IsErrorExists("Email", "Электронная почта уже зарегистрирована. Пожалуйста, укажите другой адрес электронной почты.", validationError);
            Assert.That(errorExists, Is.True);

            testStudent.Email = "anotherStudent@mail.ru";
            Assert.DoesNotThrow(() => { _ = SendAsync(testStudent); });
        }

        [Test]
        public async Task CreateStudentCommand_PasswordTestAsync()
        {
            var testStudent = GetStudentCommand();

            testStudent.Password = "";
            ValidationFailureException validationError = Assert.ThrowsAsync<ValidationFailureException>(() => SendAsync(testStudent), "It should throw an exception");
            var messageShownForEmptyPassword = IsErrorExists("Password", ValidationMessages.GetNotEmptyMessage("Password"), validationError);

            Assert.That(messageShownForEmptyPassword, Is.True);

            testStudent.Password = "Pw12345!";
            var studentIdentityResult = await SendAsync(testStudent);
            Assert.False(studentIdentityResult.Errors.Any());
        }

        [Test]
        public void CreateStudentCommand_StudentCourseTest()
        {
            var testStudent = GetStudentCommand();

            testStudent.CourseName = "CSharp";
            ValidationFailureException validationError = Assert.ThrowsAsync<ValidationFailureException>(() => SendAsync(testStudent));
            var courseIsNotExists = IsErrorExists("CourseName", "Данный курс еще не зарегистрирован. Выберите существующий курс.", validationError);

            Assert.That(courseIsNotExists, Is.True);
        }

        CreateStudentCommand GetStudentCommand()
        {
            return new CreateStudentCommand()
            {
                FirstName = "StudName",
                LastName = "StudLastName",
                Address = "StudAddress",
                BirthDate = System.DateTime.Today,
                PhoneNumber = "992927770000",
                City = "Khujand",
                Country = "Tajikistan",
                Email = "teststud@gmail.com",
                Occupation = "student",
                Password = "Pw12345@",
                Region = "Sogd",
                CourseName = "C# for beginners"
            };
        }
    }
}
