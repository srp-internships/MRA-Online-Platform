using Application.Teachers.Commands.TestCommand;
using Domain.Entities;
using NUnit.Framework;
using System;
using FluentAssertions;
using static Application.IntegrationTest.TestHelper;
using System.Threading.Tasks;

namespace Application.IntegrationTest.Teachers.Tests.Command
{
    public class UpdateTestCommandTest
    {
        [Test]
        public async Task UpdateTest_AsyncTest()
        {
            await RunAsTeacherAsync();
            var teacher = await GetAuthenticatedUser<Teacher>();

            var course = CreateCourse(teacher.Id);
            await AddAsync(course);

            var theme = CreateTheme(course.Id, DateTime.Now);
            await AddAsync(theme);

            var createTest = CreateTest("Test 1", 8, theme.Id);
            await AddAsync(createTest);

            var updateTestCommand = new UpdateTestCommand()
            {
                Id = createTest.Id,
                Name = "Change",
                Description = "Change test",
                Rating = 9,
            };
            await SendAsync(updateTestCommand);

            var updateDTO = await FindAsync<Test>(createTest.Id);

            updateDTO.Name.Should().Be(updateTestCommand.Name);
            updateDTO.Description.Should().Be(updateTestCommand.Description);
            updateDTO.Rating.Should().Be(updateTestCommand.Rating);
        }

        #region TestData 
        Test CreateTest(string name, int rate, Guid themId)
        {
            return new Test()
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
