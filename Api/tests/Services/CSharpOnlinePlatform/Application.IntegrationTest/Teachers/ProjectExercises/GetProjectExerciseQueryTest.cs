using System;
using System.Threading.Tasks;
using Application.Teachers.Queries.ProjectExerciseQuery;
using Core.Exceptions;
using Domain.Entities;
using FluentAssertions;
using FluentAssertions.Equivalency.Tracing;
using NUnit.Framework;
using static Application.IntegrationTest.TestHelper;

namespace Application.IntegrationTest.Teachers.ProjectExercises
{
    public class GetProjectExerciseQueryTest
    {
        [Test]
        public void GetProjectExerciseQuery_NotExistingThemeId_ForbiddenException()
        {
            var command = new GetProjectExerciseQuery(Guid.NewGuid(), Guid.NewGuid());

            ValidationFailureException validationFailureException = Assert.ThrowsAsync<ValidationFailureException>(() => SendAsync(command));
            var forbiddenExceptionShown = IsErrorExists("ThemeId", "Ваш доступ ограничен.", validationFailureException);

            forbiddenExceptionShown.Should().BeTrue();
        }

        [Test]
        public async Task GetProjectExerciseQuery_CorrectData_ListOfProjectExercise()
        {
            await RunAsTeacherAsync();
            var teacher = await GetAuthenticatedUser<Teacher>();
            var course = new Course() { Id = Guid.NewGuid(), Name = "C# Training", LearningLanguage = "Tajik", TeacherId = teacher.Id };
            await AddAsync(course);
            var theme = new Theme() { Id = Guid.NewGuid(), Content = "Content", Name = "Name", CourseId = course.Id, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(-1) };
            await AddAsync(theme);
            var projectExercise = new ProjectExercise() { Id = Guid.NewGuid(), Name = "Name", Description = "Description", Rating = 1, ThemeId = theme.Id };
            await AddAsync(projectExercise);

            var command = new GetProjectExerciseQuery(teacher.Id, theme.Id);

            var listOfProjectExercise = await SendAsync(command);

            listOfProjectExercise.Should().NotBeEmpty();
        }
    }
}
