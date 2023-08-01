using Application.Teachers.Commands.TestCommand;
using Domain.Entities;
using NUnit.Framework;
using System;
using FluentAssertions;
using static Application.IntegrationTest.TestHelper;
using System.Threading.Tasks;
using Application.Teachers.Commands.ProjectExerciseCommand.CreateProjectExercise;

namespace Application.IntegrationTest.Teachers.ProjectExercises.Commands
{
    public class CreateProjectExerciseCommandTest
    {
        [Test]
        public async Task CreateProjectExerciseForTheme_AsyncTest()
        {
            await RunAsTeacherAsync();
            var teacher = await GetAuthenticatedUser<Teacher>();

            var course = CreateCourse(teacher.Id);
            await AddAsync(course);

            var theme = CreateTheme(course.Id, DateTime.Now);
            await AddAsync(theme);

            var cretateprojectExerciseCommand = CreateProjectExercise(teacher.Id, "Test 1", 10, theme.Id);
            var proExerciseId = await SendAsync(cretateprojectExerciseCommand);

            var createDTO = await FindAsync<ProjectExercise>(proExerciseId);

            createDTO.Name.Should().Be(cretateprojectExerciseCommand.Exercise.Name);
            createDTO.Rating.Should().Be(cretateprojectExerciseCommand.Exercise.Rating);
            createDTO.Description.Should().Be(cretateprojectExerciseCommand.Exercise.Description);
        }

        #region TestData 
        CreateProjectExerciseCommand CreateProjectExercise(Guid teacherId, string name, int rate, Guid themId)
        {
            return new CreateProjectExerciseCommand(teacherId, new CreateProjectExerciseCommandDTO()
            {
                ThemeId = themId,
                Rating = rate,
                Name = name,
                Description = "For test"
            });
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
