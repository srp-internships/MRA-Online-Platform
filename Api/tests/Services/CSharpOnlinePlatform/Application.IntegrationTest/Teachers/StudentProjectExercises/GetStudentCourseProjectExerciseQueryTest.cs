using Application.Admin.Commands.TeacherCommand;
using Application.Students.Commands;
using Application.Teachers.Commands.CourseCommand;
using Application.Teachers.Commands.ProjectExerciseCommand.CreateProjectExercise;
using Application.Teachers.Commands.ThemeCommands;
using Application.Teachers.Queries.StudentCourseProjectExerciseQuery;
using Core.Exceptions;
using Domain.Entities;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.IO;
using System.Threading.Tasks;
using static Application.IntegrationTest.TestHelper;

namespace Application.IntegrationTest.Teachers.StudentProjectExercises
{
    public class GetStudentCourseProjectExerciseQueryTest
    {
        [Test]
        public async Task GetStudentCourseProjectExerciseQuery_NotExistingProjectExerciseId_NotFoundException()
        {
            var projectExerciseId = await GetProjectExerciseId();
            var teacher = await GetAuthenticatedUser<Teacher>();

            var getStudentProjectExercise = new GetStudentCourseProjectExerciseQuery(Guid.NewGuid(), teacher.Id);
            ValidationFailureException validationException = Assert.ThrowsAsync<ValidationFailureException>
                (() => SendAsync(getStudentProjectExercise));
            var projectExerciseNotFoundExceptionShown = IsErrorExists("ProjectExerciseId", "Проект не найден.", validationException);

            projectExerciseNotFoundExceptionShown.Should().BeTrue();
        }

        [Test]
        public async Task GetStudentCourseProjectExerciseQuery_NotExistingTeacherId_NotFoundException()
        {
            var projectExerciseId = await GetProjectExerciseId();

            var getStudentProjectExercise =
                new GetStudentCourseProjectExerciseQuery(projectExerciseId, Guid.NewGuid());
            ValidationFailureException validationException = Assert.ThrowsAsync<ValidationFailureException>
                (() => SendAsync(getStudentProjectExercise));
            var projectExerciseNotFoundExceptionShown = IsErrorExists("TeacherId", "Учитель не найден.", validationException);

            projectExerciseNotFoundExceptionShown.Should().BeTrue();
        }

        // [ Test]
        // public async Task GetStudentCourseProjectExerciseQuery_WrongTeacherId_NotFoundException()
        // {
        //     var projectExerciseId = await GetProjectExerciseId();
        //     // await SendAsync(GetTeacherCommand());
        //     var anotherTeacher = await GetAsync<Teacher>(t => t.Email == "test1999@mail.ru");
        //
        //     var getStudentProjectExercise =
        //         new GetStudentCourseProjectExerciseQuery(projectExerciseId, anotherTeacher.Id);
        //     ValidationFailureException validationException = Assert.ThrowsAsync<ValidationFailureException>
        //         (() => SendAsync(getStudentProjectExercise));
        //     var projectExerciseNotFoundExceptionShown = IsErrorExists("TeacherId", "Отказано в доступе.", validationException);
        //
        //     projectExerciseNotFoundExceptionShown.Should().BeTrue();
        // }

        [Test]
        public async Task GetStudentCourseProjectExerciseQuery_NoStudentProjectExerciseUploads_EmptyList()
        {
            var projectExerciseId = await GetProjectExerciseId();
            var teacher = await GetAuthenticatedUser<Teacher>();

            var getStudentProjectExercise =
                new GetStudentCourseProjectExerciseQuery(projectExerciseId, teacher.Id);
            var listOfStudentProjectExercises = await SendAsync(getStudentProjectExercise);

            listOfStudentProjectExercises.Should().BeEmpty();
        }

        [Test]
        public async Task GetStudentCourseProjectExerciseQuery_CorrectData_NotEmptyList()
        {
            await RunAsTeacherAsync();
            var teacher = await GetAuthenticatedUser<Teacher>();
            var courseId = await AddCourse();
            var themeId = await AddTheme(courseId);
            var projectExerciseId = await AddProjectExcercise(themeId);
            await AddStudentProjectexercise(courseId, projectExerciseId);

            var getStudentProjectExercise = new GetStudentCourseProjectExerciseQuery(projectExerciseId, teacher.Id);
            var listOfStudentProjectExercises = await SendAsync(getStudentProjectExercise);

            listOfStudentProjectExercises.Should().NotBeEmpty();
        }


        // CreateTeacherCommand GetTeacherCommand()
        // {
        //     return new CreateTeacherCommand()
        //     {
        //         FirstName = "FirstName",
        //         LastName = "Lastname",
        //         DateOfBirth = DateTime.Now,
        //         Email = "test1999@mail.ru",
        //         Password = "Abcd1234)",
        //         PhoneNumber = "123456789",
        //         Country = "Tojikiston",
        //         Region = "Mintaqa",
        //         City = "Shahr",
        //         Address = "Suroga",
        //     };
        // }

        async Task AddStudentProjectexercise(Guid courseId, Guid projectExerciseId)
        {
            await RunAsStudentAsync();
            var student = await GetAuthenticatedUser<Student>();
            var studentCourse = new StudentCourse()
            {
                Id = Guid.NewGuid(),
                CourseId = courseId,
                StudentId = student.Id,
            };
            await AddAsync(studentCourse);
            var newStudentCourseProjectExercise = new StudentCourseProjectExercise
            {
                ProjectExerciseId = projectExerciseId,
                StudentCourseId = studentCourse.Id,
                Status = Status.WaitForTeacher,
                LinkToProject = "",
                Date = DateTime.Now,
                Comment = ""
            };
            await AddAsync(newStudentCourseProjectExercise);
        }

        async Task<Guid> AddCourse()
        {
            await RunAsTeacherAsync();
            var teacher = await GetAuthenticatedUser<Teacher>();

            var course = new Course()
            {
                Id = Guid.NewGuid(),
                LearningLanguage = "Tajik",
                Name = "C# Basics",
                TeacherId = teacher.Id
            };
            await AddAsync(course);
            return course.Id;
        }

        async Task<Guid> AddTheme(Guid courseId)
        {
            var theme = new Theme()
            {
                Id = Guid.NewGuid(),
                Name = "TestingTheme",
                Content = "Content",
                CourseId = courseId,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(1)
            };
            await AddAsync(theme);
            return theme.Id;
        }

        async Task<Guid> AddProjectExcercise(Guid themeId)
        {
            var projectExercise = new ProjectExercise()
            {
                Id = Guid.NewGuid(),
                Description = "Description",
                Name = "Name",
                Rating = 5,
                ThemeId = themeId
            };
            await AddAsync(projectExercise);
            return projectExercise.Id;
        }

        async Task<Guid> GetProjectExerciseId()
        {
            var courseId = await AddCourse();
            var themeId = await AddTheme(courseId);
            var projectExerciseId = await AddProjectExcercise(themeId);
            return projectExerciseId;
        }
    }
}
