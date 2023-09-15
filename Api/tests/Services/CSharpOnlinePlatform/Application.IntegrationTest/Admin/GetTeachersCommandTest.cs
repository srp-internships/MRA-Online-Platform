using System;
using System.Threading.Tasks;
using Application.Admin.Commands.TeacherCommand;
using Domain.Entities;
using FluentAssertions;
using NUnit.Framework;
using static Application.IntegrationTest.TestHelper;

namespace Application.IntegrationTest.Admin
{
    public class GetTeachersCommandTest
    {
        [Test]
        public async Task GetTeachersCommand_ListOfTeachers()
        {
            var teacher = new Teacher() { Id = Guid.NewGuid() };
            await AddAsync(teacher);
            var command = new GetTeachersCommand();

            var listOfTeachers = await SendAsync(command);

            listOfTeachers.Should().NotBeEmpty();
        }
    }
}
