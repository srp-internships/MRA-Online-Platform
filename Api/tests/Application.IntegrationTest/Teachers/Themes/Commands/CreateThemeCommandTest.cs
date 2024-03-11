using Application.Teachers.Commands.CourseCommand;
using Application.Teachers.Commands.ThemeCommands;
using Core.Exceptions;
using Core.ValidationsBehaviours;
using Domain.Entities;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using static Application.IntegrationTest.TestHelper;

namespace Application.IntegrationTest.Teachers.Themes.Commands
{
    public class CreateThemeCommandTest
    {
        [Test]
        public async Task ShouldRequireValidThemeId()
        {
            var command = new CreateThemeCommand(Guid.NewGuid(), "Collection", "In C#, collection represents group of objects.", new DateTime(2022, 07, 15), new DateTime(2022, 07, 22), Guid.NewGuid());

            await FluentActions.Invoking(() =>
                SendAsync(command)).Should().ThrowAsync<ValidationFailureException>();
        }

        [Test]
        public async Task ShouldCreateThemeTest()
        {
            var teacherId = Guid.NewGuid();

            var courseId = await SendAsync(new CreateCourseCommand(teacherId, "C# Advanced", "English"));

            var theme = new CreateThemeCommand(teacherId, "Collection", "In C#, collection represents group of objects.", new DateTime(2022, 07, 15), new DateTime(2022, 07, 22), courseId);
            var themeId = await SendAsync(theme);

            var dataBaseTheme = await FindAsync<Theme>(themeId);

            dataBaseTheme.Should().NotBeNull();
            dataBaseTheme!.CourseId.Should().Be(theme.CourseId);
            dataBaseTheme.Name.Should().Be(theme.Name);
            dataBaseTheme.Content.Should().Be(theme.Content);
            dataBaseTheme.StartDate.Should().Be(theme.StartDate);
            dataBaseTheme.EndDate.Should().Be(theme.EndDate);
        }

        [Test]
        public async Task CreateThemeCommand_EmptyName_NotEmptyException()
        {
            var teacherId = Guid.NewGuid();
            var course = new Course()
            {
                Id = Guid.NewGuid(),
                Name = "C# Training",
                LearningLanguage = "Tajik",
                TeacherId = teacherId
            };
            await AddAsync(course);
            var commandDTO = new CreateThemeCommandDTO()
            {
                Name = string.Empty,
                Content = "Content",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(7),
                CourseId = course.Id
            };
            var command = new CreateThemeCommand(teacherId, commandDTO.Name, commandDTO.Content, commandDTO.StartDate, commandDTO.EndDate, course.Id);

            ValidationFailureException validationFailureException = Assert.ThrowsAsync<ValidationFailureException>(() => SendAsync(command));
            var notEmptyExceptionShown = IsErrorExists("Name", ValidationMessages.GetNotEmptyMessage("Name"), validationFailureException);

            notEmptyExceptionShown.Should().BeTrue();
        }

        [Test]
        public async Task CreateThemeCommand_EmptyContent_NotEmptyException()
        {
            var teacherId = Guid.NewGuid();
            var course = new Course()
            {
                Id = Guid.NewGuid(),
                Name = "C# Training",
                LearningLanguage = "Tajik",
                TeacherId = teacherId
            };
            await AddAsync(course);
            var commandDTO = new CreateThemeCommandDTO()
            {
                Name = "Name",
                Content = string.Empty,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(7),
                CourseId = course.Id
            };
            var command = new CreateThemeCommand(teacherId, commandDTO.Name, commandDTO.Content, commandDTO.StartDate, commandDTO.EndDate, course.Id);

            ValidationFailureException validationFailureException = Assert.ThrowsAsync<ValidationFailureException>(() => SendAsync(command));
            var notEmptyExceptionShown = IsErrorExists("Content", ValidationMessages.GetNotEmptyMessage("Content"), validationFailureException);

            notEmptyExceptionShown.Should().BeTrue();
        }

        [Test]
        public async Task CreateThemeCommand_LessEndDate_NotLessException()
        {
            var teacherId = Guid.NewGuid();
            var course = new Course()
            {
                Id = Guid.NewGuid(),
                Name = "C# Training",
                LearningLanguage = "Tajik",
                TeacherId = teacherId
            };
            await AddAsync(course);
            var commandDTO = new CreateThemeCommandDTO()
            {
                Name = "Name",
                Content = "Content",
                StartDate = DateTime.Now.AddDays(2),
                EndDate = DateTime.Now.AddDays(1),
                CourseId = course.Id
            };
            var command = new CreateThemeCommand(teacherId, commandDTO.Name, commandDTO.Content, commandDTO.StartDate, commandDTO.EndDate, course.Id);

            ValidationFailureException validationFailureException = Assert.ThrowsAsync<ValidationFailureException>(() => SendAsync(command));
            var NotLessException = IsErrorExists("EndDate", ValidationMessages.GetGreaterThanMessage(commandDTO.StartDate.ToShortDateString()), validationFailureException);

            NotLessException.Should().BeTrue();
        }

        [Test]
        public async Task CreateThemeCommand_NotExistingTeacherId_NotFoundException()
        {
            var teacherId = Guid.NewGuid();
            var course = new Course()
            {
                Id = Guid.NewGuid(),
                Name = "C# Training",
                LearningLanguage = "Tajik",
                TeacherId = teacherId
            };
            await AddAsync(course);
            var commandDTO = new CreateThemeCommandDTO()
            {
                Name = "Name",
                Content = "Content",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(7),
                CourseId = course.Id
            };
            var command = new CreateThemeCommand(Guid.NewGuid(), commandDTO.Name, commandDTO.Content, commandDTO.StartDate, commandDTO.EndDate, course.Id);

            ValidationFailureException validationFailureException = Assert.ThrowsAsync<ValidationFailureException>(() => SendAsync(command));
            var notFoundException = IsErrorExists("TeacherId", "Ваш доступ ограничен.", validationFailureException);

            notFoundException.Should().BeTrue();
        }
    }
}
