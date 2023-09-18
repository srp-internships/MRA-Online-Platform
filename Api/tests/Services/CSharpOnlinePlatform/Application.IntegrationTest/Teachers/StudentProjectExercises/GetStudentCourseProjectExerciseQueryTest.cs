using Application.Teachers.Queries.StudentCourseProjectExerciseQuery;
using Core.Exceptions;
using Domain.Entities;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using static Application.IntegrationTest.TestHelper;

namespace Application.IntegrationTest.Teachers.StudentProjectExercises
{
    public class GetStudentCourseProjectExerciseQueryTest
    {
        [Test]
        public void GetStudentCourseProjectExerciseQuery_NotExistingProjectExerciseId_NotFoundException()
        {
            var teacherId = Guid.NewGuid();
            var getStudentProjectExercise = new GetStudentCourseProjectExerciseQuery(Guid.NewGuid(), teacherId);
            ValidationFailureException validationException = Assert.ThrowsAsync<ValidationFailureException>
                (() => SendAsync(getStudentProjectExercise));
            var projectExerciseNotFoundExceptionShown =
                IsErrorExists("ProjectExerciseId", "Проект не найден.", validationException);

            projectExerciseNotFoundExceptionShown.Should().BeTrue();
        }
        

        [Test]
        public async Task GetStudentCourseProjectExerciseQuery_NoStudentProjectExerciseUploads_EmptyList()
        {
            var projectExerciseId = await GetProjectExerciseId();
            var teacherId = Guid.NewGuid();
            var getStudentProjectExercise =
                new GetStudentCourseProjectExerciseQuery(projectExerciseId, teacherId);
            var listOfStudentProjectExercises = await SendAsync(getStudentProjectExercise);

            listOfStudentProjectExercises.Should().BeEmpty();
        }

        [Test]
        public async Task GetStudentCourseProjectExerciseQuery_CorrectData_NotEmptyList()
        {
            await RunAsTeacherAsync();
            var courseId = await AddCourse();
            var teacherId = Guid.NewGuid();
            var themeId = await AddTheme(courseId);
            var projectExerciseId = await AddProjectExcercise(themeId);
            await AddStudentProjectexercise(courseId, projectExerciseId);

            var getStudentProjectExercise = new GetStudentCourseProjectExerciseQuery(projectExerciseId, teacherId);
            var listOfStudentProjectExercises = await SendAsync(getStudentProjectExercise);

            listOfStudentProjectExercises.Should().NotBeEmpty();
        }


        async Task AddStudentProjectexercise(Guid courseId, Guid projectExerciseId)
        {
            var studentId = Guid.NewGuid();
            var studentCourse = new StudentCourse()
            {
                Id = Guid.NewGuid(),
                CourseId = courseId,
                StudentId = studentId,
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
            var teacherId = Guid.NewGuid();

            var course = new Course()
            {
                Id = Guid.NewGuid(),
                LearningLanguage = "Tajik",
                Name = "C# Basics",
                TeacherId = teacherId
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