using Application.Students;
using Domain.Entities;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;
using static Application.IntegrationTest.TestHelper;

namespace Application.IntegrationTest.Students
{
    public class GetStudentQueryTest
    {
        [Test]
        public async Task GetCourse_ShouldReturnCourseOfStudent()
        {
            await RunAsStudentAsync();
            var student = await GetAuthenticatedUser<Student>();
            var course = CreateCourse();
            await AddAsync(course);
            var studentCourse = CreateStudentCourse(course, student);
            await AddAsync(studentCourse);
            GetStudentQuery query = new GetStudentQuery(student.Id);
            var studentDTO = await SendAsync(query);
            Assert.That(studentDTO.Courses.Any(c => c.CourseId == course.Id), Is.True);
        }

        [Test]
        public async Task GetThemes_ShouldReturnThemesOfCourseOfStudent()
        {
            await RunAsStudentAsync();
            var student = await GetAuthenticatedUser<Student>();
            var course = CreateCourse();
            await AddAsync(course);
            var studentCourse = CreateStudentCourse(course, student);
            await AddAsync(studentCourse);
            var theme1 = CreateTheme(course);
            await AddAsync(theme1);
            var theme2 = CreateTheme(course);
            await AddAsync(theme2);
            GetStudentQuery query = new GetStudentQuery(student.Id);
            var studentDTO = await SendAsync(query);
            Assert.That(studentDTO.Courses.FirstOrDefault(c => c.CourseId == course.Id).Course
                .Themes.Any(th => th.Id == theme1.Id), Is.True);

            Assert.That(studentDTO.Courses.FirstOrDefault(c => c.CourseId == course.Id).Course
                .Themes.Any(th => th.Id == theme2.Id), Is.True);
        }

        [Test]
        public async Task GetExercises_ShouldReturnExecisesOfThemeOfCourseOfStudent()
        {
            await RunAsStudentAsync();
            var student = await GetAuthenticatedUser<Student>();
            var course = CreateCourse();
            await AddAsync(course);
            var studentCourse = CreateStudentCourse(course, student);
            await AddAsync(studentCourse);
            var theme = CreateTheme(course);
            await AddAsync(theme);
            var exercise = CreateExercise(theme);
            await AddAsync(exercise);
            GetStudentQuery query = new GetStudentQuery(student.Id);
            var studentDTO = await SendAsync(query);
            Assert.That(studentDTO.Courses.FirstOrDefault(c => c.CourseId == course.Id).Course
                .Themes.FirstOrDefault(th => th.Id == theme.Id)
                .Exercises.Any(ex => ex.Id == exercise.Id), Is.True);
        }

        #region Test Data

        Course CreateCourse()
        {
            return new Course()
            {
                Id = Guid.NewGuid(),
                Name = "C# Threading",
                LearningLanguage = "Tajik"
            };
        }

        StudentCourse CreateStudentCourse(Course course, Student student)
        {
            return new StudentCourse()
            {
                Id = Guid.NewGuid(),
                CourseId = course.Id,
                StudentId = student.Id
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
