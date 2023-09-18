using Domain.Entities;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using static Application.IntegrationTest.TestHelper;

namespace Application.IntegrationTest.Teachers
{
    public class GetTeacherQueryTest
    {
        [Test]
        public async Task GetCourse_ShouldReturnCourseOfTeacher()
        {
            var teacherId = Guid.NewGuid();
            var course = CreateCourse(teacherId);
            await AddAsync(course);


            Assert.That(false, Is.True);
        }

        [Test]
        public async Task GetThemes_ShouldReturnThemesOfCourseOfTeacher()
        {
            var teacherId = Guid.NewGuid();
            var course = CreateCourse(teacherId);
            await AddAsync(course);
            var theme1 = CreateTheme(course);
            await AddAsync(theme1);
            var theme2 = CreateTheme(course);
            await AddAsync(theme2);

            //todo https://github.com/srp-internships/MRA-Online-Platform/issues/11
        }

        [Test]
        public async Task GetExercises_ShouldReturnExecisesOfThemeOfCourseOfTeacher()
        {
            var teacherId = Guid.NewGuid();
            var course = CreateCourse(teacherId);
            await AddAsync(course);
            var theme = CreateTheme(course);
            await AddAsync(theme);
            var exercise = CreateExercise(theme);
            await AddAsync(exercise);
            
            //todo https://github.com/srp-internships/MRA-Online-Platform/issues/12
            
        }

        #region Test Data

        Course CreateCourse(Guid teacherId)
        {
            return new Course()
            {
                Id = Guid.NewGuid(),
                Name = "C# Threading",
                LearningLanguage = "Tajik",
                TeacherId = teacherId
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