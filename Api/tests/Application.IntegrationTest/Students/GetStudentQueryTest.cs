using Domain.Entities;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;
using Application.Students.Queries;
using static Application.IntegrationTest.TestHelper;

namespace Application.IntegrationTest.Students
{
    public class GetStudentQueryTest
    {
        [Test]
        public async Task GetCourse_ShouldReturnCourseOfStudent()
        {
            await RunAsStudentAsync();
            var studentId = Guid.NewGuid();
            var course = CreateCourse();
            await AddAsync(course);
            var studentCourse = CreateStudentCourse(course, studentId);
            await AddAsync(studentCourse);
            GetCoursesQuery query = new GetCoursesQuery(studentId);
            var coursesDto = await SendAsync(query);
            Assert.That(coursesDto.Any(c => c.Id == course.Id), Is.True);
        }

        [Test]
        public async Task GetThemes_ShouldReturnThemesOfCourseOfStudent()
        {
            var studentId = Guid.NewGuid();
            var course = CreateCourse();
            await AddAsync(course);
            var studentCourse = CreateStudentCourse(course, studentId);
            await AddAsync(studentCourse);
            var theme1 = CreateTheme(course);
            await AddAsync(theme1);
            var theme2 = CreateTheme(course);
            await AddAsync(theme2);

            var query = new GetThemesQuery(course.Id, studentId);
            var themes = await SendAsync(query);
            Assert.That(themes.Any(s => s.Id == theme1.Id), Is.True);
            Assert.That(themes.Any(s => s.Id == theme2.Id), Is.True);
        }

        [Test]
        public async Task GetExercises_ShouldReturnExercisesOfThemeOfCourseOfStudent()
        {
            var studentId = Guid.NewGuid();
            var course = CreateCourse();
            await AddAsync(course);
            var studentCourse = CreateStudentCourse(course, studentId);
            await AddAsync(studentCourse);
            var theme = CreateTheme(course);
            await AddAsync(theme);
            var exercise = CreateExercise(theme);
            await AddAsync(exercise);


            var query = new GetExercisesQuery(theme.Id, studentId);
            
            var exercises = await SendAsync(query);
            
            Assert.That(exercises.Any(e=>e.Id==exercise.Id));
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

        StudentCourse CreateStudentCourse(Course course, Guid studentId)
        {
            return new StudentCourse()
            {
                Id = Guid.NewGuid(),
                CourseId = course.Id,
                StudentId = studentId
            };
        }

        Theme CreateTheme(Course course)
        {
            return new Theme()
            {
                Id = Guid.NewGuid(),
                Name = "Class Task",
                Content =
                    "Одна задача может запускать другую - вложенную задачу. При этом эти задачи выполняются независимо друг от друга.",
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