using Application.Admin.Commands.TeacherCommand;
using Core.Exceptions;
using Core.ValidationsBehaviours;
using NUnit.Framework;
using System;
using static Application.IntegrationTest.TestHelper;

namespace Application.IntegrationTest.Admin.TeacherCommands
{
    public class CreateTeacherCommandTest
    {
        [Test]
        public void CreateTeacherConnamd_SetAllRequiredFields_DontShowMessage()
        {
            var testTeacher = GetTeacherCommand();
            testTeacher.Email = "test1@mail.ru";
            Assert.DoesNotThrowAsync(() => SendAsync(testTeacher));
        }

        [Test]
        public void CreateTeacherConnamd_FirstNameMustNotBeEmptyTest()
        {
            var testTeacher = GetTeacherCommand();

            testTeacher.FirstName = null;
            ValidationFailureException validationError = Assert.ThrowsAsync<ValidationFailureException>(() => SendAsync(testTeacher));
            var firstNameMustNotBeEmptyErrorWasShown = IsErrorExists("FirstName", ValidationMessages.GetNotEmptyMessage("First Name"), validationError);
            Assert.That(firstNameMustNotBeEmptyErrorWasShown, Is.True);
        }

        [Test]
        public void CreateTeacherConnamd_LastNameMustNotBeEmptyTest()
        {
            var testTeacher = GetTeacherCommand();

            testTeacher.LastName = null;
            ValidationFailureException validationError = Assert.ThrowsAsync<ValidationFailureException>(() => SendAsync(testTeacher));
            var lastNameMustNotBeEmptyErrorWasShown = IsErrorExists("LastName", ValidationMessages.GetNotEmptyMessage("Last Name"), validationError);
            Assert.That(lastNameMustNotBeEmptyErrorWasShown, Is.True);
        }

        [Test]
        public void CreateTeacherCommand_EmailTest()
        {
            var testTeacher = GetTeacherCommand();

            testTeacher.Email = null;
            ValidationFailureException validationError = Assert.ThrowsAsync<ValidationFailureException>(() => SendAsync(testTeacher));
            var messageShown = IsErrorExists("Email", ValidationMessages.GetNotEmptyMessage("Email"), validationError); ;
            Assert.That(messageShown, Is.True);

            testTeacher.Email = "email";
            validationError = Assert.ThrowsAsync<ValidationFailureException>(() => SendAsync(testTeacher));
            messageShown = IsErrorExists("Email", ValidationMessages.GetEmailAddressMessage(), validationError);
            Assert.That(messageShown, Is.True);

            testTeacher.Email = "tursunhuja@mail.ru";
            validationError = Assert.ThrowsAsync<ValidationFailureException>(() => SendAsync(testTeacher));
            messageShown = IsErrorExists("Email", "Электронная почта уже зарегистрирована. Пожалуйста, укажите другой адрес электронной почты.", validationError);
            Assert.That(messageShown, Is.True);

            testTeacher.Email = "anotherTeacher@mail.ru";
            Assert.DoesNotThrowAsync(() => SendAsync(testTeacher));
        }

        [Test]
        public void CreateTeacherCommand_PasswordTest()
        {
            var testTeacher = GetTeacherCommand();

            testTeacher.Password = null;
            ValidationFailureException validationError = Assert.ThrowsAsync<ValidationFailureException>(() => SendAsync(testTeacher));
            var messageShown = IsErrorExists("Password", ValidationMessages.GetNotEmptyMessage("Password"), validationError);
            Assert.That(messageShown, Is.True);

            testTeacher.Password = "Abcd1234!";
            Assert.DoesNotThrowAsync(() => SendAsync(testTeacher));
        }

        [Test]
        public void CreateTeacherCommand_CountryTest()
        {
            var testTeacher = GetTeacherCommand();

            testTeacher.Country = null;
            ValidationFailureException validationError = Assert.ThrowsAsync<ValidationFailureException>(() => SendAsync(testTeacher));
            var messageShown = IsErrorExists("Country", ValidationMessages.GetNotEmptyMessage("Country"), validationError);
            Assert.That(messageShown, Is.True);
        }

        [Test]
        public void CreateTeacherCommand_RegionTest()
        {
            var testTeacher = GetTeacherCommand();

            testTeacher.Region = null;
            ValidationFailureException validationError = Assert.ThrowsAsync<ValidationFailureException>(() => SendAsync(testTeacher));
            var messageShow = IsErrorExists("Region", ValidationMessages.GetNotEmptyMessage("Region"), validationError);
            Assert.That(messageShow, Is.True);
        }

        [Test]
        public void CreateTeacherCommand_CityTest()
        {
            var testTeacher = GetTeacherCommand();

            testTeacher.City = null;
            ValidationFailureException validationError = Assert.ThrowsAsync<ValidationFailureException>(() => SendAsync(testTeacher));
            var messageShow = IsErrorExists("City", ValidationMessages.GetNotEmptyMessage("City"), validationError);
            Assert.That(messageShow, Is.True);
        }

        [Test]
        public void CreateTeacherCommand_AddressTest()
        {
            var testTeacher = GetTeacherCommand();

            testTeacher.Address = null;
            ValidationFailureException validationError = Assert.ThrowsAsync<ValidationFailureException>(() => SendAsync(testTeacher));
            var messageShow = IsErrorExists("Address", ValidationMessages.GetNotEmptyMessage("Address"), validationError);
            Assert.That(messageShow, Is.True);
        }

        [Test]
        public void CreateTeacherCommand_PhoneNumberTest()
        {
            var testTeacher = GetTeacherCommand();

            testTeacher.PhoneNumber = null;
            ValidationFailureException validationError = Assert.ThrowsAsync<ValidationFailureException>(() => SendAsync(testTeacher));
            var messageShow = IsErrorExists("PhoneNumber", ValidationMessages.GetNotEmptyMessage("Phone Number"), validationError);
            Assert.That(messageShow, Is.True);
        }

        CreateTeacherCommand GetTeacherCommand()
        {
            return new CreateTeacherCommand()
            {
                FirstName = "Nozanin",
                LastName = "Abdulloeva",
                DateOfBirth = DateTime.Now,
                Email = "an_shef_1999@mail.ru",
                Password = "Abcd1234)",
                PhoneNumber = "123456789",
                Country = "Tojikiston",
                Region = "Mintaqa",
                City = "Shahr",
                Address = "Suroga",
            };
        }
    }
}
