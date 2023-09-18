using Application.Teachers.Commands.CourseCommand;
using Application.Teachers.Commands.ThemeCommands;
using Core.Exceptions;
using Core.ValidationsBehaviours;
using Domain.Entities;
using FluentAssertions;
using FluentAssertions.Equivalency.Tracing;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using static Application.IntegrationTest.TestHelper;

namespace Application.IntegrationTest.Teachers.Themes.Commands
{
    public class UpdateThemeCommandTest
    {
        [Test]
        public async Task ShouldRequireValidThemeId()
        {
            var command = new UpdateThemeCommand(Guid.NewGuid(), "Collection",
                "In C#, collection represents group of objects.", new DateTime(2022, 07, 15),
                new DateTime(2022, 07, 22), Guid.NewGuid());

            await FluentActions.Invoking(() =>
                SendAsync(command)).Should().ThrowAsync<ValidationFailureException>();
        }

        [Test]
        public async Task ShouldUpdateTheme()
        {
            var teacherId = Guid.NewGuid();
            var courseId = await SendAsync(new CreateCourseCommand(teacherId, "Python", "English"));

            var themeId = await SendAsync(new CreateThemeCommand(teacherId, "if Statements",
                "Python if statement is one of the most commonly used conditional statements in programming languages.",
                new DateTime(2022, 07, 15), new DateTime(2022, 08, 20), courseId));

            var updateTheme = new UpdateThemeCommand(themeId, "if Statements",
                "Python if statement is one of the most commonly used conditional statements in programming languages.",
                new DateTime(2022, 07, 30), new DateTime(2022, 08, 5), teacherId);

            await SendAsync(updateTheme);

            var dataBaseCourse = await FindAsync<Theme>(themeId);

            dataBaseCourse.Should().NotBeNull();
            dataBaseCourse!.Id.Should().Be((Guid)updateTheme.ThemeId);
            dataBaseCourse.Name.Should().Be(updateTheme.Name);
            dataBaseCourse.Content.Should().Be(updateTheme.Content);
            dataBaseCourse.StartDate.Should().Be(updateTheme.StartDate);
            dataBaseCourse.EndDate.Should().Be(updateTheme.EndDate);
        }

        [Test]
        public async Task UpdateThemeCommand_EmptyName_NotEmptyException()
        {
            var teacherId = Guid.NewGuid();
            var themeId = await GetTheme(teacherId);
            var commandDTO = new UpdateThemeCommandDTO()
            {
                Id = themeId,
                Name = string.Empty,
                Content = "Content",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(7)
            };
            var command = new UpdateThemeCommand(commandDTO.Id, commandDTO.Name, commandDTO.Content,
                commandDTO.StartDate, commandDTO.EndDate, teacherId);

            ValidationFailureException validationFailureException =
                Assert.ThrowsAsync<ValidationFailureException>(() => SendAsync(command));
            var notEmptyExceptionShown = IsErrorExists("Name", ValidationMessages.GetNotEmptyMessage("Name"),
                validationFailureException);

            notEmptyExceptionShown.Should().BeTrue();
        }

        [Test]
        public async Task UpdateThemeCommand_EmptyContent_NotEmptyException()
        {
            var teacherId = Guid.NewGuid();
            var themeId = await GetTheme(teacherId);
            var commandDTO = new UpdateThemeCommandDTO()
            {
                Id = themeId,
                Name = "Name",
                Content = string.Empty,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(7)
            };
            var command = new UpdateThemeCommand(commandDTO.Id, commandDTO.Name, commandDTO.Content,
                commandDTO.StartDate, commandDTO.EndDate, teacherId);

            ValidationFailureException validationFailureException =
                Assert.ThrowsAsync<ValidationFailureException>(() => SendAsync(command));
            var notEmptyExceptionShown = IsErrorExists("Content", ValidationMessages.GetNotEmptyMessage("Content"),
                validationFailureException);

            notEmptyExceptionShown.Should().BeTrue();
        }

        [Test]
        public async Task UpdateThemeCommand_LessEndDate_NotLessException()
        {
            var teacherId = Guid.NewGuid();
            var themeId = await GetTheme(teacherId);
            var commandDTO = new UpdateThemeCommandDTO()
            {
                Id = themeId,
                Name = "Name",
                Content = "Content",
                StartDate = DateTime.Now.AddDays(2),
                EndDate = DateTime.Now.AddDays(1)
            };
            var command = new UpdateThemeCommand(commandDTO.Id, commandDTO.Name, commandDTO.Content,
                commandDTO.StartDate, commandDTO.EndDate, teacherId);

            ValidationFailureException validationFailureException =
                Assert.ThrowsAsync<ValidationFailureException>(() => SendAsync(command));
            var notEmptyExceptionShown = IsErrorExists("EndDate",
                ValidationMessages.GetGreaterThanMessage(commandDTO.StartDate.ToShortDateString()),
                validationFailureException);

            notEmptyExceptionShown.Should().BeTrue();
        }

        [Test]
        public async Task UpdateThemeCommand_NotExistingTeacherId_NotFoundException()
        {
            var teacherId = Guid.NewGuid();
            var themeId = await GetTheme(teacherId);
            var commandDTO = new UpdateThemeCommandDTO()
            {
                Id = themeId,
                Name = "Name",
                Content = "Content",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(1)
            };
            var command = new UpdateThemeCommand(commandDTO.Id, commandDTO.Name, commandDTO.Content,
                commandDTO.StartDate, commandDTO.EndDate, Guid.NewGuid());

            ValidationFailureException validationFailureException =
                Assert.ThrowsAsync<ValidationFailureException>(() => SendAsync(command));
            var notFoundException = IsErrorExists("TeacherId", "Ваш доступ ограничен.", validationFailureException);

            notFoundException.Should().BeTrue();
        }

        async Task<Guid> GetTheme(Guid teacherId)
        {
            var course = new Course()
            {
                Id = Guid.NewGuid(),
                Name = "C# Training",
                LearningLanguage = "Tajik",
                TeacherId = teacherId
            };
            await AddAsync(course);

            var theme = new Theme()
            {
                CourseId = course.Id, Name = "Welcome to JS", Content = "Javascript is cool", StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(5)
            };
            await AddAsync(theme);
            return theme.Id;
        }
    }
}