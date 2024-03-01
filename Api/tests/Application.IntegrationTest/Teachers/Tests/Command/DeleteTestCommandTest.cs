using Application.Teachers.Commands.TestCommand;
using Domain.Entities;
using NUnit.Framework;
using System;
using FluentAssertions;
using static Application.IntegrationTest.TestHelper;
using System.Threading.Tasks;

namespace Application.IntegrationTest.Teachers.Tests.Command
{
    public class DeleteTestCommandTest
    {
        [Test]
        public async Task DeleteTest_AsyncTest()
        {
            var teacherId = Guid.NewGuid();

            var course = CreateCourse(teacherId);
            await AddAsync(course);

            var theme = CreateTheme(course.Id, DateTime.Now);
            await AddAsync(theme);

            var test = CreateTest("Test 1", 12, theme.Id);
            await AddAsync(test);

            var deleteTestCommand = new DeleteTestCommand(test.Id);
            await SendAsync(deleteTestCommand);

            var deleteDTO = await FindAsync<Test>(test.Id);
            deleteDTO.Should().BeNull();
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
