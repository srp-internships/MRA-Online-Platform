using Application.Admin.Commands.TeacherCommand;
using Application.Admin.Commands.TeacherCommand.TeacherCRUD;
using Core.Exceptions;
using Domain.Entities;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using static Application.IntegrationTest.TestHelper;

namespace Application.IntegrationTest.Admin.TeacherCommands
{
    public class DeleteTeacherCommandTest
    {
        [Test]
        public async Task DeleteTeacherTest()
        {
            var teacher = GetTeacherCommand();
            teacher.Email = "test2@mail.ru";
            await SendAsync(teacher);
            var testTeacher = await GetAsync<Teacher>(c => c.Email == teacher.Email);
            var deleteTeacher = new DeleteTeacherCommand(testTeacher.Id);
            await SendAsync(deleteTeacher);
            var dataBaseTeacher = await FindAsync<Teacher>(testTeacher.Id);

            dataBaseTeacher.Should().BeNull();
        }

        [Test]
        public void DeleteTeacher_WithIncorrectIdTest()
        {
            var testTeacher = Guid.NewGuid();
            var deleteTeacher = new DeleteTeacherCommand(testTeacher);

            ValidationFailureException validationError = Assert.ThrowsAsync<ValidationFailureException>(() => SendAsync(deleteTeacher));
            var message = IsErrorExists("TeacherId", "Ваш доступ ограничен.", validationError);            
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
                Address = "address",
            };
        }
    }
}