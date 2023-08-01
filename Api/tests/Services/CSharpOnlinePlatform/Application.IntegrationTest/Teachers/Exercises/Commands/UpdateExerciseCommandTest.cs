using Application.Teachers.Commands.CourseCommand;
using Application.Teachers.Commands.ExerciseCommand;
using Application.Teachers.Commands.ThemeCommands;
using Core.Exceptions;
using Core.ValidationsBehaviours;
using Domain.Entities;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using System.Xml.Linq;
using static Application.IntegrationTest.TestHelper;

namespace Application.IntegrationTest.Teachers.Exercises.Commands
{
    public class UpdateExerciseCommandTest
    {
        [Test]
        public async Task ShouldRequireValidExerciseId()
        {
            var exercise = UpdateExerciseDTO(Guid.NewGuid());
            exercise.Name = "Boolean";
            exercise.Description = "New Description";

            var command = new UpdateExerciseCommand(exercise, Guid.NewGuid());

            await FluentActions.Invoking(() =>
                SendAsync(command)).Should().ThrowAsync<ValidationFailureException>();
        }

        [Test]
        public async Task ShouldUpdateExercise()
        {
            await RunAsTeacherAsync();
            var teacher = await GetAuthenticatedUser<Teacher>();
            var courseId = await SendAsync(new CreateCourseCommand(teacher.Id, "Python", "English"));
            var themeId = await SendAsync(new CreateThemeCommand(teacher.Id, "if Statements", "Python if statement is one of the most commonly used conditional statements in programming languages.", new DateTime(2022, 07, 15), new DateTime(2022, 08, 20), courseId));
            var exercise = CreateExercise(themeId);
            var newExercise = new CreateExerciseCommand(exercise, teacher.Id);
            var exerciseId = await SendAsync(newExercise);
            exercise.Name = "Boolean";
            exercise.Description = "New Description";
            exercise.Template = "New Template";
            var updateExerciseDTO = UpdateExerciseDTO(exerciseId);
            var updateExercise = new UpdateExerciseCommand(updateExerciseDTO, teacher.Id);

            await SendAsync(updateExercise);

            var dataBaseCourse = await FindAsync<Exercise>(exerciseId);

            dataBaseCourse.Should().NotBeNull();
            dataBaseCourse!.Id.Should().Be(updateExercise.ExerciseDTO.Id);
            dataBaseCourse.Name.Should().Be(updateExercise.ExerciseDTO.Name);
            dataBaseCourse.Test.Should().Be(updateExercise.ExerciseDTO.Test);
            dataBaseCourse.Template.Should().Be(updateExercise.ExerciseDTO.Template);
            dataBaseCourse.Description.Should().Be(updateExercise.ExerciseDTO.Description);
        }

        [TestCase("", "Template", "Test", "Description", "ExerciseDTO.Name", "Exercise DT O Name")]
        [TestCase("Name", "", "Test", "Description", "ExerciseDTO.Template", "Exercise DT O Template")]
        [TestCase("Name", "Template", "", "Description", "ExerciseDTO.Test", "Exercise DT O Test")]
        [TestCase("Name", "Template", "Test", "", "ExerciseDTO.Description", "Exercise DT O Description")]
        public async Task UpdateExerciseCommand_EmptyName_NotEmptyException(string name, string template, string test, string description, string propertyName, string exceptionMessage)
        {
            await RunAsTeacherAsync();
            var teacher = await GetAuthenticatedUser<Teacher>();
            var exerciseId = await AddExercise(teacher.Id);
            var commandDTO = new UpdateExerciseCommandDTO()
            {
                Id = exerciseId,
                Name = name,
                Rating = 2,
                Template = template,
                Test = test,
                Description = description,
            };

            var command = new UpdateExerciseCommand(commandDTO, teacher.Id);
            ValidationFailureException validationFailureException = Assert.ThrowsAsync<ValidationFailureException>(() => SendAsync(command));
            var notEmpmtyExceptionShown = IsErrorExists(propertyName, ValidationMessages.GetNotEmptyMessage(exceptionMessage), validationFailureException);

            notEmpmtyExceptionShown.Should().BeTrue();
        }

        [Test]
        public async Task UpdateExerciseCommand_GreatRating_GreatRatingException()
        {
            await RunAsTeacherAsync();
            var teacher = await GetAuthenticatedUser<Teacher>();
            var exerciseId = await AddExercise(teacher.Id);
            var commandDTO = new UpdateExerciseCommandDTO()
            {
                Id = exerciseId,
                Name = "Name",
                Template = "Template",
                Test = "Test",
                Rating = 101,
                Description = "Description",
            };

            var command = new UpdateExerciseCommand(commandDTO, teacher.Id);
            ValidationFailureException validationFailureException = Assert.ThrowsAsync<ValidationFailureException>(() => SendAsync(command));
            var notEmpmtyExceptionShown = IsErrorExists("ExerciseDTO.Rating", ValidationMessages.GetLessThanOrEqualToMessage("Exercise DT O Rating", 10), validationFailureException);

            notEmpmtyExceptionShown.Should().BeTrue();
        }

        [Test]
        public async Task UpdateExerciseCommand_LessRating_LessRatingException()
        {
            await RunAsTeacherAsync();
            var teacher = await GetAuthenticatedUser<Teacher>();
            var exerciseId = await AddExercise(teacher.Id);
            var commandDTO = new UpdateExerciseCommandDTO()
            {
                Id = exerciseId,
                Name = "Name",
                Template = "Template",
                Test = "Test",
                Rating = -101,
                Description = "Description",
            };

            var command = new UpdateExerciseCommand(commandDTO, teacher.Id);
            ValidationFailureException validationFailureException = Assert.ThrowsAsync<ValidationFailureException>(() => SendAsync(command));
            var notEmpmtyExceptionShown = IsErrorExists("ExerciseDTO.Rating", ValidationMessages.GetGreaterThanOrEqualToMessage("Exercise DT O Rating", 0), validationFailureException);

            notEmpmtyExceptionShown.Should().BeTrue();
        }

        public CreateExerciseCommandDTO CreateExercise(Guid themeId)
        {
            return new CreateExerciseCommandDTO()
            {
                Name = "Integer",
                Rating = 5,
                Template = "A distanceLis given in centimeters. Find the amount of full meters of this distance (1m=1000cm). Use the operator of integer division.",
                Test = "Test",
                Description = "Description",
                ThemeId = themeId
            };
        }

        public UpdateExerciseCommandDTO UpdateExerciseDTO(Guid exerciseId)
        {
            return new UpdateExerciseCommandDTO()
            {
                Id = exerciseId,
                Name = "Integer",
                Rating = 5,
                Template = "A distanceLis given in centimeters. Find the amount of full meters of this distance (1m=1000cm). Use the operator of integer division.",
                Test = "Test",
                Description = "Description"
            };
        }

        async Task<Guid> AddExercise(Guid teacherId)
        {
            var course = new Course()
            {
                Id = Guid.NewGuid(),
                Name = "C# Training",
                LearningLanguage = "Tajik",
                TeacherId = teacherId
            };
            await AddAsync(course);
            var theme = new Theme()
            {
                Name = "Welcome to C#",
                Content = "C# is the best",
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddMonths(1),
                Id = Guid.NewGuid(),
                CourseId = course.Id,
            };
            await AddAsync(theme);
            var exercise = new Exercise() { Id = Guid.NewGuid(), Description = "", Name = "", Rating = 5, Template = "", Test = "", ThemeId = theme.Id, };
            await AddAsync(exercise);
            return exercise.Id;
        }
    }
}
