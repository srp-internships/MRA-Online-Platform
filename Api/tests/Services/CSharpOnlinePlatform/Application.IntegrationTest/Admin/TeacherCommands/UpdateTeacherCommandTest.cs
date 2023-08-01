using Application.Admin.Commands.TeacherCommand;
using Application.Admin.Commands.TeacherCommand.TeacherCRUD;
using Core.Exceptions;
using Domain.Entities;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using static Application.IntegrationTest.TestHelper;

namespace Application.IntegrationTest.Admin.TeacherCommands
{
    public class UpdateTeacherCommandTest
    {
        [Test]
        public async Task UpdateTeacherTest()
        {
            var teacher = GetTeacherCommand();
            teacher.Email = "test3@mail.ru";
            await SendAsync(teacher);
            var testTeacher = await GetAsync<Teacher>(c => c.Email == teacher.Email);
            var updateTeacher = new UpdateTeacherCommand()
            {
                Id = testTeacher.Id,
                FirstName = "Nozanin",
                LastName = "Kholmatzoda",
                DateOfBirth = DateTime.Now,
                Email = "130821bn@gmail.com",
                PhoneNumber = "1234567",
                Country = "Country",
                Region = "Region",
                City = "City",
                Address = "Suroga",
            };

            await SendAsync(updateTeacher);
            var dataBaseTeacher = await FindAsync<Teacher>(testTeacher.Id);


            Assert.NotNull(dataBaseTeacher);
            Assert.That(dataBaseTeacher.Id, Is.EqualTo(updateTeacher.Id));
            Assert.That(dataBaseTeacher.FirstName, Is.EqualTo(updateTeacher.FirstName));
            Assert.That(dataBaseTeacher.LastName, Is.EqualTo(updateTeacher.LastName));
            Assert.That(dataBaseTeacher.Birthdate, Is.EqualTo(updateTeacher.DateOfBirth));
            Assert.That(dataBaseTeacher.Email, Is.EqualTo(updateTeacher.Email));

            var contact = await GetAsync<Contact>(c => c.User == dataBaseTeacher);
            Assert.That(contact.Country, Is.EqualTo(updateTeacher.Country));
            Assert.That(contact.Region, Is.EqualTo(updateTeacher.Region));
            Assert.That(contact.City, Is.EqualTo(updateTeacher.City));
            Assert.That(contact.Address, Is.EqualTo(updateTeacher.Address));
            Assert.That(contact.PhoneNumber, Is.EqualTo(updateTeacher.PhoneNumber));
        }

        [Test]
        public void UpdateTeacher_WithIncorrectIdTest()
        {
            var testTeacher = Guid.NewGuid();
            var updateTeacher = new UpdateTeacherCommand()
            {
                Id = testTeacher,
                FirstName = "Nozanin",
                LastName = "Kholmatzoda",
                DateOfBirth = DateTime.Now,
                Email = "130821bn@gmail.com",
                PhoneNumber = "1234567",
                Country = "Country",
                Region = "Region",
                City = "City",
                Address = "Suroga",
            };
            ValidationFailureException validationError = Assert.ThrowsAsync<ValidationFailureException>(() => SendAsync(updateTeacher));
            var message = IsErrorExists(nameof(updateTeacher.Id), "Ваш доступ ограничен.", validationError);            
            Assert.That(message, Is.True);
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
