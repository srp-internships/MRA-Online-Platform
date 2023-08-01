using Application.Teachers.Commands.TestCommand;
using Domain.Entities;
using NUnit.Framework;
using System;
using FluentAssertions;
using static Application.IntegrationTest.TestHelper;
using System.Threading.Tasks;
using Application.Teachers.Commands.ProjectExerciseCommand.DeleteProjectExercise;

namespace Application.IntegrationTest.Teachers.ProjectExercises.Commands
{
    public class DeleteProjectExerciseCommandTest
    {
        [Test]
        public async Task DeleteProjectExercise_AsyncTest()
        {
            await RunAsTeacherAsync();
            var teacher = await GetAuthenticatedUser<Teacher>();

            var course = CreateCourse(teacher.Id);
            await AddAsync(course);

            var theme = CreateTheme(course.Id, DateTime.Now);
            await AddAsync(theme);

            var projectExercise = CreateProjectExercise("Test 1", 12, theme.Id);
            await AddAsync(projectExercise);

            var deleteProjectExerciseCommand = new DeleteProjectExerciseCommand(projectExercise.Id);
            await SendAsync(deleteProjectExerciseCommand);

            var deleteDTO = await FindAsync<ProjectExercise>(projectExercise.Id);
            deleteDTO.Should().BeNull();
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
