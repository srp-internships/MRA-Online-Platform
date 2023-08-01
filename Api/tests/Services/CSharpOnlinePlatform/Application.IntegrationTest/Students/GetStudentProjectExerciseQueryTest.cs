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
            await RunAsStudentAsync();
            var student = await GetAuthenticatedUser<Student>();
            var command = new GetStudentProjectExerciseQuery(Guid.NewGuid(), student.Id);

            ValidationFailureException validationFailureException = Assert.ThrowsAsync<ValidationFailureException>(() => SendAsync(command));
            var notFoundExceptionShown = IsErrorExists("ThemeId", "Тема не найдена.", validationFailureException);

            notFoundExceptionShown.Should().BeTrue();
        }

        [Test]
        public async Task GetStudentProjectExerciseQuery_NotExistingStudentId_NotFoundException()
        {
            await RunAsTeacherAsync();
            var teacher = await GetAuthenticatedUser<Teacher>();
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
            var teacher = await GetAuthenticatedUser<Teacher>();
            var course = new Course() { Id = Guid.NewGuid(), LearningLanguage = "en", Name = "C# Basics", TeacherId = teacher.Id };
            await AddAsync(course);
            var theme = new Theme() { Id = Guid.NewGuid(), Content = "Content", Name = "Name", CourseId = course.Id };
            await AddAsync(theme);
            var projectExercise = new ProjectExercise() { Id = Guid.NewGuid(), Description = "Description", Name = "Name", Rating = 5, ThemeId = theme.Id };
            await AddAsync(projectExercise);
            await RunAsStudentAsync();
            var student = await GetAuthenticatedUser<Student>();
            var studentCourse = new StudentCourse() { Id = Guid.NewGuid(), CourseId = course.Id, StudentId = student.Id };
            await AddAsync(studentCourse);
            var studentProjectExcercise = new StudentCourseProjectExercise() { Id = Guid.NewGuid(), Comment = "Comment", ProjectExerciseId = projectExercise.Id, StudentCourseId = studentCourse.Id };
            await AddAsync(studentProjectExcercise);
            var command = new GetStudentProjectExerciseQuery(theme.Id, student.Id);

            var listOfStudentProjectExercise = await SendAsync(command);

            listOfStudentProjectExercise.Should().NotBeEmpty();
        }

    }
}
