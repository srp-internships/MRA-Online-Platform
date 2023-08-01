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
            await RunAsTeacherAsync();
            var teacher = await GetAuthenticatedUser<Teacher>();

            var courseId = await SendAsync(new CreateCourseCommand(teacher.Id, "Python", "English"));

            var themeId = await SendAsync(new CreateThemeCommand(teacher.Id, "if Statements", "Python if statement is one of the most commonly used conditional statements in programming languages.", new DateTime(2022, 07, 15), new DateTime(2022, 08, 20), courseId));

            var exercise = CreateExercise(themeId); 

            var exercise1 = new CreateExerciseCommand(exercise, teacher.Id);
            await SendAsync(exercise1);
            
            exercise.Name = "Long";

            var exercise2 = new CreateExerciseCommand(exercise, teacher.Id);
            await SendAsync(exercise2);

            var getExercise = new GetExercisesTeacherQuery(themeId, teacher.Id);

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
