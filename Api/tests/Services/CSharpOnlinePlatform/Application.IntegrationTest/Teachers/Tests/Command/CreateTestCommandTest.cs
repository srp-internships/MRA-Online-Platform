using Application.Teachers.Commands.TestCommand;
using Domain.Entities;
using NUnit.Framework;
using System;
using FluentAssertions;
using static Application.IntegrationTest.TestHelper;
using System.Threading.Tasks;
using Application.Teachers.Queries.TestsQuery;
using System.Linq;

namespace Application.IntegrationTest.Teachers.Tests.Command
{
    public class CreateTestCommandTest
    {
        [Test]
        public async Task CreateTestForTheme_AsyncTest()
        {
            var teacherId = Guid.NewGuid();

            var course = CreateCourse(teacherId);
            await AddAsync(course);

            var theme = CreateTheme(course.Id, DateTime.Now);
            await AddAsync(theme);

            var cretateTestCommand = CreateTestCommand("Test 1", 10, theme.Id);
            var testId = await SendAsync(cretateTestCommand);

            var createDTO = await FindAsync<Test>(testId);

            createDTO.Name.Should().Be(cretateTestCommand.Name);
            createDTO.Rating.Should().Be(cretateTestCommand.Rating);
            createDTO.Description.Should().Be(cretateTestCommand.Description);
        }

        #region TestData

        CreateTestCommand CreateTestCommand(string name, int rate, Guid themId)
        {
            return new CreateTestCommand()
            {
                ThemeId = themId,
                Rating = rate,
                Name = name,
                Description = "For test"
            };
        }

        Theme CreateTheme(Guid courseId, DateTime startDate)
        {
            return new Theme()
            {
                Id = Guid.NewGuid(),
                CourseId = courseId,
                Name = $"Chapter 1",
                Content = "Test",
                StartDate = startDate
            };
        }

        Course CreateCourse(Guid teacherId)
        {
            return new Course()
            {
                Id = Guid.NewGuid(),
                TeacherId = teacherId,
                Name = "C# Basic",
                LearningLanguage = "Tajik"
            };
        }

        #endregion
    }
}