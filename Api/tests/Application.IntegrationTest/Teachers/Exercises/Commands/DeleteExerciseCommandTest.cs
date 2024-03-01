using Application.Teachers.Commands.ExerciseCommand;
using Core.Exceptions;
using Domain.Entities;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using static Application.IntegrationTest.TestHelper;

namespace Application.IntegrationTest.Teachers.Exercises.Commands
{
    public class DeleteExerciseCommandTest
    {
        [Test]
        public async Task ShouldRequireValidExerciseId()
        {
            var teacherId = Guid.NewGuid();
            var command = new DeleteExerciseCommand(Guid.NewGuid(), teacherId);

            await FluentActions.Invoking(() =>
                SendAsync(command)).Should().ThrowAsync<ValidationFailureException>();
        }

        [Test]
        public async Task ShouldDeleteExercise()
        {
            var teacherId = Guid.NewGuid();

            var course = CreateCourse(teacherId);
            await AddAsync(course);

            var theme = CreateTheme(course);
            await AddAsync(theme);

            var exercise = CreateExercise(theme);
            await AddAsync(exercise);

            await SendAsync(new DeleteExerciseCommand(exercise.Id, teacherId));

            var item = await FindAsync<Exercise>(exercise.Id);
            item.Should().BeNull();
        }

        Course CreateCourse(Guid teacherId)
        {
            return new Course()
            {
                Id = Guid.NewGuid(),
                Name = "C# Training",
                LearningLanguage = "Tajik",
                TeacherId = teacherId
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

        Exercise CreateExercise(Theme theme)
        {
            return new Exercise()
            {
                Id = Guid.NewGuid(),
                Name = "Integer",
                Description =
                    "A distanceLis given in centimeters. Find the amount of full meters of this distance (1m=1000cm). Use the operator of integer division. ",
                Template = "",
                ThemeId = theme.Id,
                Test = "Test"
            };
        }
    }
}