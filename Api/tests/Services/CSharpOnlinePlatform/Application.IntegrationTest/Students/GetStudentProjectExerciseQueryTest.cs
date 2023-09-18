using System;
using System.Threading.Tasks;
using Application.Students.Queries;
using Core.Exceptions;
using Domain.Entities;
using FluentAssertions;
using NUnit.Framework;
using static Application.IntegrationTest.TestHelper;

namespace Application.IntegrationTest.Students
{
    public class GetStudentProjectExerciseQueryTest
    {
        [Test]
        public async Task GetStudentProjectExerciseQuery_NotExistingThemeId_NotFoundException()
        {
            var studentId = Guid.NewGuid();
            var command = new GetStudentProjectExerciseQuery(Guid.NewGuid(), studentId);

            ValidationFailureException validationFailureException = Assert.ThrowsAsync<ValidationFailureException>(() => SendAsync(command));
            var notFoundExceptionShown = IsErrorExists("ThemeId", "Тема не найдена.", validationFailureException);

            notFoundExceptionShown.Should().BeTrue();
        }

        [Test]
        public async Task GetStudentProjectExerciseQuery_NotExistingStudentId_NotFoundException()
        {
            await RunAsTeacherAsync();
            var teacherId = Guid.NewGuid();
            var theme = new Theme()
            {
                Id = Guid.NewGuid(),
                Content = "Content",
                Name = "Name"
            };
            await AddAsync(theme);

            var command = new GetStudentProjectExerciseQuery(theme.Id, Guid.NewGuid());

            ValidationFailureException validationFailureException = Assert.ThrowsAsync<ValidationFailureException>(() => SendAsync(command));
            var notFoundExceptionShown = IsErrorExists("StudentId", "Студент не найден.", validationFailureException);

            notFoundExceptionShown.Should().BeTrue();
        }

        [Test]
        public async Task GetStudentProjectExerciseQuery_CorrectData_ListOfStudentProjectExercise()
        {
            await RunAsTeacherAsync();
            var teacherId = Guid.NewGuid();
            var course = new Course() { Id = Guid.NewGuid(), LearningLanguage = "en", Name = "C# Basics", TeacherId = teacherId };
            await AddAsync(course);
            var theme = new Theme() { Id = Guid.NewGuid(), Content = "Content", Name = "Name", CourseId = course.Id };
            await AddAsync(theme);
            var projectExercise = new ProjectExercise() { Id = Guid.NewGuid(), Description = "Description", Name = "Name", Rating = 5, ThemeId = theme.Id };
            await AddAsync(projectExercise);
            await RunAsStudentAsync();
            var studentId = Guid.NewGuid();
            var studentCourse = new StudentCourse() { Id = Guid.NewGuid(), CourseId = course.Id, StudentId = studentId };
            await AddAsync(studentCourse);
            var studentProjectExcercise = new StudentCourseProjectExercise() { Id = Guid.NewGuid(), Comment = "Comment", ProjectExerciseId = projectExercise.Id, StudentCourseId = studentCourse.Id };
            await AddAsync(studentProjectExcercise);
            var command = new GetStudentProjectExerciseQuery(theme.Id, studentId);

            var listOfStudentProjectExercise = await SendAsync(command);

            listOfStudentProjectExercise.Should().NotBeEmpty();
        }

    }
}
