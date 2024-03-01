using Application.Teachers.Commands.CourseCommand;
using Domain.Entities;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using FluentAssertions;
using static Application.IntegrationTest.TestHelper;
using Core.Exceptions;

namespace Application.IntegrationTest.Teachers.Courses.Commands
{
    public class DeleteCourseCommandTest
    {
        [Test]
        public async Task ShouldRequireValidCourseId()
        {
            var teacherId = Guid.NewGuid();
            var command = new DeleteCourseCommand(teacherId, Guid.NewGuid());

            await FluentActions.Invoking(() =>
                SendAsync(command)).Should().ThrowAsync<ValidationFailureException>();
        }

        [Test]
        public async Task ShouldDeleteCourse()
        {
            var teacherId = Guid.NewGuid();
            var course = CreateCourse(teacherId);

            await AddAsync(course);

            await SendAsync(new DeleteCourseCommand(course.TeacherId, course.Id));

            var item = await FindAsync<Course>(course.Id);
            item.Should().BeNull();
        }

        Course CreateCourse(Guid teacherId)
        {
            return new Course()
            {
                Id = Guid.NewGuid(),
                Name = "C# Training",
                LearningLanguage = "Tajik",
                TeacherId = teacherId
            };
        }
    }
}