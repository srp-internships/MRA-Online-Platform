using Application.Teachers.Commands.TestCommand;
using Domain.Entities;
using NUnit.Framework;
using System;
using FluentAssertions;
using static Application.IntegrationTest.TestHelper;
using System.Threading.Tasks;
using Application.Teachers.Commands.ProjectExerciseCommand.UpdateProjectExercise;

namespace Application.IntegrationTest.Teachers.ProjectExercises.Commands
{
    public class UpdateProjectExerciseCommandTest
    {
        [Test]
        public async Task UpdateTest_AsyncTest()
        {
            var teacherId = Guid.NewGuid();

            var course = CreateCourse(teacherId);
            await AddAsync(course);

            var theme = CreateTheme(course.Id, DateTime.Now);
            await AddAsync(theme);

            var createProjectExercise = CreateProjectExercise("Test 1", 8, theme.Id);
            await AddAsync(createProjectExercise);

            var updateProjectExerciseCommand = new UpdateProjectExerciseCommand()
            {
                Id = createProjectExercise.Id,
                Name = "Change",
                Description = "Change test",
                Rating = 9,
            };
            await SendAsync(updateProjectExerciseCommand);

            var updateDTO = await FindAsync<ProjectExercise>(createProjectExercise.Id);

            updateDTO.Name.Should().Be(updateProjectExerciseCommand.Name);
            updateDTO.Description.Should().Be(updateProjectExerciseCommand.Description);
            updateDTO.Rating.Should().Be(updateProjectExerciseCommand.Rating);
        }

        #region TestData

        ProjectExercise CreateProjectExercise(string name, int rate, Guid themId)
        {
            return new ProjectExercise()
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