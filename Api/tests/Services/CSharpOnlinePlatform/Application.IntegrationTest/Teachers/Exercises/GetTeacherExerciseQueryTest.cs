using Application.Teachers.Commands.CourseCommand;
using Application.Teachers.Commands.ExerciseCommand;
using Application.Teachers.Commands.ThemeCommands;
using Application.Teachers.Queries.ExerciseQuery;
using Domain.Entities;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using static Application.IntegrationTest.TestHelper;

namespace Application.IntegrationTest.Teachers.Exercises
{
    public class GetTeacherExerciseQueryTest
    {
        [Test]
        public async Task GetExercises_ShouldReturnExercisesFromDataBaseTest()
        {
            var teacherId = Guid.NewGuid();

            var courseId = await SendAsync(new CreateCourseCommand(teacherId, "Python", "English"));

            var themeId = await SendAsync(new CreateThemeCommand(teacherId, "if Statements",
                "Python if statement is one of the most commonly used conditional statements in programming languages.",
                new DateTime(2022, 07, 15), new DateTime(2022, 08, 20), courseId));

            var exercise = CreateExercise(themeId);

            var exercise1 = new CreateExerciseCommand(exercise, teacherId);
            await SendAsync(exercise1);

            exercise.Name = "Long";

            var exercise2 = new CreateExerciseCommand(exercise, teacherId);
            await SendAsync(exercise2);

            var getExercise = new GetExercisesTeacherQuery(themeId, teacherId);

            var exerciseDto = await SendAsync(getExercise);
            Assert.That(exerciseDto.Count, Is.EqualTo(2));
        }

        public CreateExerciseCommandDTO CreateExercise(Guid themeId)
        {
            return new CreateExerciseCommandDTO()
            {
                Name = "If statement",
                Rating = 3,
                Template = "Template",
                Test = "Test",
                Description = "Description",
                ThemeId = themeId
            };
        }
    }
}