using Application.CodeAnalyzer.Commands;
using Application.CodeAnalyzer.DTO;
using Core.Exceptions;
using Core.ValidationsBehaviours;
using Domain.Entities;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using static Application.IntegrationTest.TestHelper;

namespace Application.IntegrationTest.CodeAnalyzer.Commands
{
    public class AnalyzeCodeCommandTests
    {
        [Test]
        public void AnalyzeCodeCommand_ParametersMustNotBeEmptyTest()
        {
            AnalyzeCodeCommand command = new(Guid.NewGuid(), null, GetVersion());

            ValidationFailureException validationError = Assert.ThrowsAsync<ValidationFailureException>(() => SendAsync(command));

            var parametersMustNotBeEmptyErrorWasShown = IsErrorExists("Parameters", ValidationMessages.GetNotNullMessage("Parameters"), validationError);

            Assert.That(parametersMustNotBeEmptyErrorWasShown, Is.True);
        }

        [Test]
        public void AnalyzeCodeCommand_ParametersVersionMustNotBeEmptyTest()
        {
            AnalyzeCodeCommand command = new(Guid.NewGuid(), new(), null);

            ValidationFailureException validationError = Assert.ThrowsAsync<ValidationFailureException>(() => SendAsync(command));

            var parametersMustNotBeEmptyErrorWasShown = IsErrorExists("Version", ValidationMessages.GetNotNullMessage("Version"), validationError);
            Assert.That(parametersMustNotBeEmptyErrorWasShown, Is.True);
        }

        [Test]
        public void AnalyzeCodeCommand_ParametersCodeMustNotBeEmptyTest()
        {
            AnalyzeCodeCommand command = new(Guid.NewGuid(), new(), GetVersion());

            ValidationFailureException validationError = Assert.ThrowsAsync<ValidationFailureException>(() => SendAsync(command));

            var parametersMustNotBeEmptyErrorWasShown = IsErrorExists("Code", ValidationMessages.GetNotEmptyMessage("Code"), validationError);

            Assert.That(parametersMustNotBeEmptyErrorWasShown, Is.False);
        }

        [Test]
        public async Task AnalyzeCodeCommand_EverythingIsValidTest()
        {
            await RunAsStudentAsync();
            var course = CreateCourse();
            await AddAsync(course);
            var theme = CreateTheme(course);
            await AddAsync(theme);
            var exercise = CreateExercise(theme);
            await AddAsync(exercise);
            var student = await GetAuthenticatedUser<Student>();
            var studentCourse = new StudentCourse { CourseId = course.Id, StudentId = student.Id };
            await AddAsync(studentCourse);

            AnalyzeCodeCommand command = new(student.Id, new() { Code = TestCodeAnalyzerService.SuccessCode, Id = exercise.Id }, GetVersion());
            CodeAnalyzeResultDTO codeAnalyzeResult = await SendAsync(command);
            codeAnalyzeResult.Success.Should().BeTrue();

            var studentCourseExercise = await GetAsync<StudentCourseExercise>(s => s.StudentCourseId == studentCourse.Id && s.ExerciseId == exercise.Id);
            studentCourseExercise.Should().NotBeNull();
            studentCourseExercise.Code.Should().Be(command.Parameters.Code);
            studentCourseExercise.Status.Should().Be(Status.Passed);
        }

        [Test]
        public async Task AnalyzeCodeCommand_InvalidCodeTest()
        {
            await RunAsStudentAsync();
            var course = CreateCourse();
            await AddAsync(course);
            var theme = CreateTheme(course);
            await AddAsync(theme);
            var exercise = CreateExercise(theme);
            await AddAsync(exercise);
            var student = await GetAuthenticatedUser<Student>();
            var studentCourse = new StudentCourse { CourseId = course.Id, StudentId = student.Id };
            await AddAsync(studentCourse);

            AnalyzeCodeCommand command = new(student.Id, new() { Code = TestCodeAnalyzerService.FailCode, Id = exercise.Id }, GetVersion());
            CodeAnalyzeResultDTO codeAnalyzeResult = await SendAsync(command);
            codeAnalyzeResult.Success.Should().BeFalse();

            var studentCourseExercise = await GetAsync<StudentCourseExercise>(s => s.StudentCourseId == studentCourse.Id && s.ExerciseId == exercise.Id);
            studentCourseExercise.Should().NotBeNull();
            studentCourseExercise.Code.Should().Be(command.Parameters.Code);
            studentCourseExercise.Status.Should().Be(Status.Failed);

            studentCourseExercise.Date.Date.Should().Be(DateTime.Today);
        }

        [Test]
        public async Task AnalyzeCodeCommand_ExceptionCodeTest()
        {
            //arrange
            await RunAsStudentAsync();
            var course = CreateCourse();
            await AddAsync(course);
            var theme = CreateTheme(course);
            await AddAsync(theme);
            var exercise = CreateExercise(theme);
            await AddAsync(exercise);
            var student = await GetAuthenticatedUser<Student>();
            var studentCourse = new StudentCourse { CourseId = course.Id, StudentId = student.Id };
            await AddAsync(studentCourse);

            //act
            AnalyzeCodeCommand command = new(student.Id, new() { Code = TestCodeAnalyzerService.ExceptionCode, Id = exercise.Id }, GetVersion());
            var codeAnalyzeResult = await SendAsync(command);

            //assert
            codeAnalyzeResult.Success.Should().BeFalse();
            var studCourExerDTO = await GetAsync<StudentCourseExercise>(course.Id);
            studCourExerDTO.Should().BeNull();
        }

        Course CreateCourse()
        {
            return new Course()
            {
                Id = Guid.NewGuid(),
                Name = "C# Training",
                LearningLanguage = "Tajik",
            };
        }

        Theme CreateTheme(Course course)
        {
            return new Theme()
            {
                Name = "Welcome to C#",
                Content = "C# is the best",
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddMonths(1),
                Id = Guid.NewGuid(),
                CourseId = course.Id,
            };
        }

        Exercise CreateExercise(Theme theme)
        {
            return new Exercise()
            {
                Name = "Create hello world console app",
                Description = "Use VS 2022",
                Template = "Template",
                Test = "Test",
                Id = Guid.NewGuid(),
                ThemeId = theme.Id,
            };
        }

        VersionDTO GetVersion()
        {
            return new VersionDTO { Language = Constants.CSHARP_LANGUAGE, Version = Constants.DOTNET_SIX_VERSION };
        }
    }
}
