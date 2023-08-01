using Application.Teachers.Commands.ExerciseCommand;
using Core.Exceptions;
using Core.ValidationsBehaviours;
using Domain.Entities;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using System;
using System.ComponentModel.Design;
using System.Threading.Tasks;
using System.Xml.Linq;
using static Application.IntegrationTest.TestHelper;

namespace Application.IntegrationTest.Teachers.Exercises.Commands
{
    public class CreateExerciseCommandTest
    {
        [Test]
        public async Task ShouldRequireValidExerciseId()
        {
            var exerciseDTO = CreateExercise(Guid.NewGuid());

            var command = new CreateExerciseCommand(exerciseDTO, Guid.NewGuid());

            await FluentActions.Invoking(() =>
                SendAsync(command)).Should().ThrowAsync<ValidationFailureException>();
        }

        [Test]
        public async Task ShouldCreateExerciseTest()
        {
            await RunAsTeacherAsync();
            var teacher = await GetAuthenticatedUser<Teacher>();

            var theme = CreateCourseWithTheme(teacher);
            await AddAsync(theme);

            var exercise = CreateExercise(theme.Id);

            var newExercise = new CreateExerciseCommand(exercise, teacher.Id);
            var exerciseId = await SendAsync(newExercise);

            var dataBaseExercise = await FindAsync<Exercise>(exerciseId);

            dataBaseExercise.Should().NotBeNull();
            dataBaseExercise!.ThemeId.Should().Be(exercise.ThemeId);
            dataBaseExercise.Name.Should().Be(exercise.Name);
            dataBaseExercise.Template.Should().Be(exercise.Template);
            dataBaseExercise.Test.Should().Be(exercise.Test);
            dataBaseExercise.Description.Should().Be(exercise.Description);
        }

        [TestCase("", "Template", "Test", "Description", "ExerciseDTO.Name", "Exercise DT O Name")]
        [TestCase("Name", "", "Test", "Description", "ExerciseDTO.Template", "Exercise DT O Template")]
        [TestCase("Name", "Template", "", "Description", "ExerciseDTO.Test", "Exercise DT O Test")]
        [TestCase("Name", "Template", "Test", "", "ExerciseDTO.Description", "Exercise DT O Description")]
        public async Task CreateExerciseCommand_EmptyParameters_NotEmptyException(string name, string template, string test, string description, string propertyName, string exceptionMessage)
        {
            await RunAsTeacherAsync();
            var teacher = await GetAuthenticatedUser<Teacher>();
            var themeId = await AddTheme(teacher.Id);
            var commandDTO = new CreateExerciseCommandDTO()
            {
                Name = name,
                Rating = 1,
                Template = template,
                Test = test,
                Description = description,
                ThemeId = themeId
            };

            var command = new CreateExerciseCommand(commandDTO, teacher.Id);
            ValidationFailureException validationFailureException = Assert.ThrowsAsync<ValidationFailureException>(() => SendAsync(command));
            var notEmpmtyExceptionShown = IsErrorExists(propertyName, ValidationMessages.GetNotEmptyMessage(exceptionMessage), validationFailureException);

            notEmpmtyExceptionShown.Should().BeTrue();
        }

        [Test]
        public async Task CreateExerciseCommand_GreatRating_GreatRatingException()
        {
            await RunAsTeacherAsync();
            var teacher = await GetAuthenticatedUser<Teacher>();
            var themeId = await AddTheme(teacher.Id);
            var commandDTO = new CreateExerciseCommandDTO()
            {
                Name = "Name",
                Template = "Template",
                Test = "Test",
                Rating = 101,
                Description = "Description",
                ThemeId = themeId
            };

            var command = new CreateExerciseCommand(commandDTO, teacher.Id);
            ValidationFailureException validationFailureException = Assert.ThrowsAsync<ValidationFailureException>(() => SendAsync(command));
            var notEmpmtyExceptionShown = IsErrorExists("ExerciseDTO.Rating", ValidationMessages.GetLessThanOrEqualToMessage("Exercise DT O Rating", 10), validationFailureException);

            notEmpmtyExceptionShown.Should().BeTrue();
        }

        [Test]
        public async Task CreateExerciseCommand_LessRating_LessRatinException()
        {
            await RunAsTeacherAsync();
            var teacher = await GetAuthenticatedUser<Teacher>();
            var themeId = await AddTheme(teacher.Id);
            var commandDTO = new CreateExerciseCommandDTO()
            {
                Name = "Name",
                Template = "Template",
                Test = "Test",
                Rating = -101,
                Description = "Description",
                ThemeId = themeId
            };

            var command = new CreateExerciseCommand(commandDTO, teacher.Id);
            ValidationFailureException validationFailureException = Assert.ThrowsAsync<ValidationFailureException>(() => SendAsync(command));
            var notEmpmtyExceptionShown = IsErrorExists("ExerciseDTO.Rating", ValidationMessages.GetGreaterThanOrEqualToMessage("Exercise DT O Rating", 0), validationFailureException);

            notEmpmtyExceptionShown.Should().BeTrue();
        }

        [Test]
        public async Task CreateExerciseCommand_NotExistingThemeId_ForbiddenException()
        {
            await RunAsTeacherAsync();
            var teacher = await GetAuthenticatedUser<Teacher>();
            var commandDTO = new CreateExerciseCommandDTO()
            {
                Name = "Name",
                Template = "Template",
                Test = "Test",
                Rating = 1,
                Description = "Description",
                ThemeId = Guid.NewGuid()
            };

            var command = new CreateExerciseCommand(commandDTO, teacher.Id);
            ValidationFailureException validationFailureException = Assert.ThrowsAsync<ValidationFailureException>(() => SendAsync(command));
            var forbiddenException = IsErrorExists("ExerciseDTO.ThemeId", "Ваш доступ ограничен.", validationFailureException);

            forbiddenException.Should().BeTrue();
        }

        Theme CreateCourseWithTheme(Teacher teacher)
        {
            var course = new Course() { LearningLanguage = "TAJIK", Name = "Javascript", TeacherId = teacher.Id };
            return new Theme() { Course = course, Name = "Welcome to JS", Content = "Javascript is cool", StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(5) };
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

        async Task<Guid> AddTheme(Guid teacherId)
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
            return theme.Id;
        }
    }
}
