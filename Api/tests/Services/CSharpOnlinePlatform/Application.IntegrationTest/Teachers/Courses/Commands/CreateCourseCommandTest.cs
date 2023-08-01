using Application.Teachers.Commands.CourseCommand;
using Core.Exceptions;
using Core.ValidationsBehaviours;
using Domain.Entities;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using static Application.IntegrationTest.TestHelper;

namespace Application.IntegrationTest.Teachers.Courses.Commands
{
    public class CreateCourseCommandTest
    {
        [Test]
        public async Task ShouldRequireValidCourseId()
        {
            var command = new CreateCourseCommand(Guid.NewGuid(), "C# Advanced", "Tajik");

            await FluentActions.Invoking(() =>
                SendAsync(command)).Should().ThrowAsync<ValidationFailureException>();
        }

        [Test]
        public async Task ShouldCreateCourseTest()
        {
            await RunAsTeacherAsync();
            var teacher = await GetAuthenticatedUser<Teacher>();

            var course = new CreateCourseCommand(teacher.Id, "C# Advanced", "English");
            var courseId = await SendAsync(course);
            var dataBaseCourse = await FindAsync<Course>(courseId);

            dataBaseCourse.Should().NotBeNull();
            dataBaseCourse!.TeacherId.Should().Be(course.TeacherId);
            dataBaseCourse.Name.Should().Be(course.Name);
            dataBaseCourse.LearningLanguage.Should().Be(course.CourseLanguage);
        }

        [Test]
        public async Task CreateCourseCommand_EmptyName_NotEmptyException()
        {
            await RunAsTeacherAsync();
            var teacher = await GetAuthenticatedUser<Teacher>();
            var commandDTO = new CreateCourseCommandDTO()
            {
                Name = string.Empty,
                CourseLanguage = "Tajik"
            };

            var command = new CreateCourseCommand(teacher.Id, commandDTO.Name, commandDTO.CourseLanguage);
            ValidationFailureException validationFailureException = Assert.ThrowsAsync<ValidationFailureException>(() => SendAsync(command));
            var notEmptyExceptionShown = IsErrorExists("Name", ValidationMessages.GetNotEmptyMessage("Name"), validationFailureException);
            notEmptyExceptionShown.Should().BeTrue();
        }

        [Test]
        public async Task CreateCourseCommand_EmptyCourseLanguage_NotEmptyException()
        {
            await RunAsTeacherAsync();
            var teacher = await GetAuthenticatedUser<Teacher>();
            var commandDTO = new CreateCourseCommandDTO()
            {
                Name = "C# Basics",
                CourseLanguage = string.Empty
            };

            var command = new CreateCourseCommand(teacher.Id, commandDTO.Name, commandDTO.CourseLanguage);
            ValidationFailureException validationFailureException = Assert.ThrowsAsync<ValidationFailureException>(() => SendAsync(command));
            var notEmptyExceptionShown = IsErrorExists("CourseLanguage", ValidationMessages.GetNotEmptyMessage("Course Language"), validationFailureException);
            notEmptyExceptionShown.Should().BeTrue();
        }
    }
}
