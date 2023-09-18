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
    public class UpdateCourseCommandTest
    {
        [Test]
        public async Task ShouldRequireValidCourseId()
        {
            var command = new UpdateCourseCommand(Guid.NewGuid(), Guid.NewGuid(), "C#", "Tajik");

            await FluentActions.Invoking(() =>
                SendAsync(command)).Should().ThrowAsync<ValidationFailureException>();
        }

        [Test]
        public async Task ShouldUpdateCourse()
        {
            var teacherId = Guid.NewGuid();

            var courseId = await SendAsync(new CreateCourseCommand(teacherId, "C# Advanced", "Tajik"));

            var updateCourse = new UpdateCourseCommand(teacherId, courseId, "C# Beginner", "Tajik");

            await SendAsync(updateCourse);

            var dataBaseCourse = await FindAsync<Course>(courseId);

            dataBaseCourse.Should().NotBeNull();
            dataBaseCourse!.Id.Should().Be(updateCourse.CourseId);
            dataBaseCourse!.TeacherId.Should().Be(updateCourse.TeacherId);
            dataBaseCourse.Name.Should().Be(updateCourse.CourseName);
            dataBaseCourse.LearningLanguage.Should().Be(updateCourse.CourseLanguage);
        }

        [Test]
        public async Task UpdateCourseCommand_EmptyCourseName_NotEmptyException()
        {
            var teacherId = Guid.NewGuid();
            var course = new Course()
            {
                Id = Guid.NewGuid(),
                LearningLanguage = "Language",
                Name = "Name",
                TeacherId = teacherId
            };
            await AddAsync(course);
            var commandDTO = new UpdateCourseCommandDTO()
            {
                Id = course.Id,
                Name = string.Empty,
                CourseLanguage = "CourseLanguage"
            };
            var command = new UpdateCourseCommand(teacherId, course.Id, commandDTO.Name, commandDTO.CourseLanguage);
            ValidationFailureException validationFailureException = Assert.ThrowsAsync<ValidationFailureException>(() => SendAsync(command));
            var notEmptyExceptionShown = IsErrorExists("CourseName", ValidationMessages.GetNotEmptyMessage("Course Name"), validationFailureException);
            notEmptyExceptionShown.Should().BeTrue();
        }

        [Test]
        public async Task UpdateCourseCommand_EmptyCourseLanguage_NotEmptyException()
        {
            var teacherId = Guid.NewGuid();
            var course = new Course()
            {
                Id = Guid.NewGuid(),
                LearningLanguage = "Language",
                Name = "Name",
                TeacherId = teacherId
            };
            await AddAsync(course);
            var commandDTO = new UpdateCourseCommandDTO()
            {
                Id = course.Id,
                Name = "Name",
                CourseLanguage = string.Empty
            };
            var command = new UpdateCourseCommand(teacherId, course.Id, commandDTO.Name, commandDTO.CourseLanguage);
            ValidationFailureException validationFailureException = Assert.ThrowsAsync<ValidationFailureException>(() => SendAsync(command));
            var notEmptyExceptionShown = IsErrorExists("CourseLanguage", ValidationMessages.GetNotEmptyMessage("Course Language"), validationFailureException);
            notEmptyExceptionShown.Should().BeTrue();
        }
    }
}
