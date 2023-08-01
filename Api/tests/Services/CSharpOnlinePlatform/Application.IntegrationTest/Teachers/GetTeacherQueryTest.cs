using Application.Teachers;
using Core.Exceptions;
using Domain.Entities;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;
using static Application.IntegrationTest.TestHelper;

namespace Application.IntegrationTest.Teachers
{
    public class GetTeacherQueryTest
    {
        [Test]
        public async Task ShouldRequireValidTeacherGuid()
        {
            var command = new GetTeacherQuery(Guid.NewGuid());
            await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<ValidationFailureException>();

            await FluentActions.Invoking(() =>
                SendAsync(command)).Should().ThrowAsync<ValidationFailureException>();
        }

        [Test]
        public async Task GetCourse_ShouldReturnCourseOfTeacher()
        {
            await RunAsTeacherAsync();
            var teacher = await GetAuthenticatedUser<Teacher>();
            var course = CreateCourse(teacher);
            await AddAsync(course);
            GetTeacherQuery query = new GetTeacherQuery(teacher.Id);
            var teacherDTO = await SendAsync(query);
            Assert.That(teacherDTO.LeadingCourses.Any(c => c.Id == course.Id), Is.True);
        }

        [Test]
        public async Task GetThemes_ShouldReturnThemesOfCourseOfTeacher()
        {
            await RunAsTeacherAsync();
            var teacher = await GetAuthenticatedUser<Teacher>();
            var course = CreateCourse(teacher);
            await AddAsync(course);
            var theme1 = CreateTheme(course);
            await AddAsync(theme1);
            var theme2 = CreateTheme(course);
            await AddAsync(theme2);
            GetTeacherQuery query = new GetTeacherQuery(teacher.Id);
            var teacherDTO = await SendAsync(query);
            Assert.That(teacherDTO.LeadingCourses.FirstOrDefault(c => c.Id == course.Id)
                .Themes.Any(th => th.Id == theme1.Id), Is.True);

            Assert.That(teacherDTO.LeadingCourses.FirstOrDefault(c => c.Id == course.Id)
                .Themes.Any(th => th.Id == theme2.Id), Is.True);
        }

        [Test]
        public async Task GetExercises_ShouldReturnExecisesOfThemeOfCourseOfTeacher()
        {
            await RunAsTeacherAsync();
            var teacher = await GetAuthenticatedUser<Teacher>();
            var course = CreateCourse(teacher);
            await AddAsync(course);
            var theme = CreateTheme(course);
            await AddAsync(theme);
            var exercise = CreateExercise(theme);
            await AddAsync(exercise);
            GetTeacherQuery query = new GetTeacherQuery(teacher.Id);
            var teacherDTO = await SendAsync(query);
            Assert.That(teacherDTO.LeadingCourses.FirstOrDefault(c => c.Id == course.Id)
                .Themes.FirstOrDefault(th => th.Id == theme.Id)
                .Exercises.Any(ex => ex.Id == exercise.Id), Is.True);
        }

        #region Test Data

        Course CreateCourse(Teacher teacher)
        {
            return new Course()
            {
                Id = Guid.NewGuid(),
                Name = "C# Threading",
                LearningLanguage = "Tajik",
                TeacherId = teacher.Id
            };
        }

        Theme CreateTheme(Course course)
        {
            return new Theme()
            {
                Id = Guid.NewGuid(),
                Name = "Class Task",
                Content = "Одна задача может запускать другую - вложенную задачу. При этом эти задачи выполняются независимо друг от друга.",
                StartDate = new DateTime(2022, 07, 15),
                EndDate = new DateTime(2022, 07, 22),
                CourseId = course.Id
            };
        }

        Exercise CreateExercise(Theme theme)
        {
            return new Exercise()
            {
                Id = Guid.NewGuid(),
                Name = $"Task.WaitAny(tasks) {theme.Id}",
                ThemeId = theme.Id,
                Description = "Возвращение результатов из задач",
                Test = "",
                Template = $"{theme.Id}"
            };
        }

        #endregion
    }
}
