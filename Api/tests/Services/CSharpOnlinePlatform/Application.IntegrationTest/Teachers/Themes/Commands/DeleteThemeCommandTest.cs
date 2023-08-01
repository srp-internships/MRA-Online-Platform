using Application.Teachers.Commands.ThemeCommands;
using Core.Exceptions;
using Domain.Entities;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using static Application.IntegrationTest.TestHelper;

namespace Application.IntegrationTest.Teachers.Themes.Commands
{
    public class DeleteThemeCommandTest
    {
        [Test]
        public async Task ShouldRequireValidThemeId()
        {
            await RunAsTeacherAsync();
            var teacher = await GetAuthenticatedUser<Teacher>();
            var command = new DeleteThemeCommand(Guid.NewGuid(), teacher.Id);

            await FluentActions.Invoking(() =>
                SendAsync(command)).Should().ThrowAsync<ValidationFailureException>();
        }

        [Test]
        public async Task ShouldDeleteTheme()
        {
            await RunAsTeacherAsync();
            var teacher = await GetAuthenticatedUser<Teacher>();

            var course = CreateCourse(teacher);
            await AddAsync(course);

            var theme = CreateTheme(course);
            await AddAsync(theme);

            await SendAsync(new DeleteThemeCommand(theme.Id, teacher.Id));

            var item = await FindAsync<Course>(theme.Id);
            item.Should().BeNull();
        }

        Course CreateCourse(Teacher teacher)
        {
            return new Course()
            {
                Id = Guid.NewGuid(),
                Name = "C# Training",
                LearningLanguage = "Tajik",
                TeacherId = teacher.Id
            };
        }

        Theme CreateTheme(Course course)
        {
            return new Theme()
            {
                Id = Guid.NewGuid(),
                Name = "Classes",
                Content = "A class is a user-defined blueprint or prototype from which objects are created.",
                StartDate = new DateTime(2022, 07, 25),
                EndDate = new DateTime(2022, 07, 30),
                CourseId = course.Id
            };
        }
    }
}
