using Application.Students.Queries;
using Domain.Entities;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using static Application.IntegrationTest.TestHelper;

namespace Application.IntegrationTest.Students.Courses
{
    public class GetCoursesQueryTest
    {
        [Test]
        public async Task GetCourses_ShouldReturnCourseOfStudentFromDataBaseTest()
        {
            await RunAsTeacherAsync();
            var teacherId = Guid.NewGuid();

            await RunAsStudentAsync();
            var studentId = Guid.NewGuid();
            var course = CreateCourse(teacherId);
            await AddAsync(course);
            var studentCourse = CreateStudentCourse(course, studentId);
            await AddAsync(studentCourse);

            var them = CreateTheme(course, DateTime.Now);
            await AddAsync(them);
            them = CreateTheme(course, DateTime.Now.AddDays(1));
            await AddAsync(them);
            them = CreateTheme(course, DateTime.Now.AddDays(-2));
            await AddAsync(them);

            GetCoursesQuery query = new(studentId);

            var coursesDto = await SendAsync(query);

            coursesDto.Should().Contain(s => s.Name.Equals(course.Name));

            coursesDto.FirstOrDefault(s => s.Name == course.Name).TotalThemes.Should().Be(3);
            coursesDto.FirstOrDefault(s => s.Name == course.Name).CompletedThemes.Should().Be(1);
        }

        [Test]
        public async Task GetCompletedThemesCount_ShouldBeReturnCourseOfStudentFromDataBaseTest()
        {
            await RunAsStudentAsync();

            // Arrange

            var hasanboyId = Guid.NewGuid();

            var vosidId = Guid.NewGuid();

            Course course = await CreateTestData(hasanboyId, vosidId);

            // Act
            GetCoursesQuery query = new(hasanboyId);
            var hasanboyDTO = await SendAsync(query);
            query = new(vosidId);
            var vosidDTO = await SendAsync(query);

            // Assert
            hasanboyDTO.Should().Contain(s => s.Name.Equals(course.Name));
            hasanboyDTO.FirstOrDefault(s => s.Name == course.Name).TotalThemes.Should().Be(1);
            hasanboyDTO.FirstOrDefault(s => s.Name == course.Name).CompletedThemes.Should().Be(0);

            vosidDTO.Should().Contain(s => s.Name.Equals(course.Name));
            vosidDTO.FirstOrDefault(s => s.Name == course.Name).TotalThemes.Should().Be(1);
            vosidDTO.FirstOrDefault(s => s.Name == course.Name).CompletedThemes.Should().Be(1);
        }

        #region Test Data
        private async Task<Course> CreateTestData(Guid hasanboyId, Guid vosidId)
        {
            await RunAsTeacherAsync();
            var teacherId = Guid.NewGuid();

            var course = CreateCourse(teacherId);
            await AddAsync(course);

            var theme = CreateTheme(course, DateTime.Now.AddDays(-2));
            await AddAsync(theme);

            var variable = await CreateExercise("Variables", 1, theme.Id);

            var studentCourse = CreateStudentCourse(course, hasanboyId);
            await AddAsync(studentCourse);

            await CreateStudentCourseExercise(studentCourse.Id, variable.Id, Status.Failed);

            studentCourse = CreateStudentCourse(course, vosidId);
            await AddAsync(studentCourse);

            await CreateStudentCourseExercise(studentCourse.Id, variable.Id, Status.Passed);
            return course;
        }

        Course CreateCourse(Guid teacherId)
        {
            return new Course()
            {
                Id = Guid.NewGuid(),
                Name = "C# Training",
                LearningLanguage = "Tajik",
                TeacherId = teacherId
            };
        }
        Theme CreateTheme(Course course, DateTime endDate)
        {
            return new Theme()
            {
                Id = Guid.NewGuid(),
                Content = "Content",
                Name = "Name",
                CourseId = course.Id,
                StartDate = endDate.AddDays(-3),
                EndDate = endDate
            };
        }
        StudentCourse CreateStudentCourse(Course course, Guid studentId)
        {
            return new StudentCourse()
            {
                Id = Guid.NewGuid(),
                CourseId = course.Id,
                StudentId = studentId,
            };
        }
        async Task<StudentCourseExercise> CreateStudentCourseExercise(Guid studentCourseId, Guid exerciseId, Status status)
        {
            var studentCourseExercise = new StudentCourseExercise()
            {
                StudentCourseId = studentCourseId,
                ExerciseId = exerciseId,
                Status = status,
                Code = "string"
            };
            await AddAsync(studentCourseExercise);
            return studentCourseExercise;
        }

        async Task<Exercise> CreateExercise(string name, int rate, Guid themId)
        {
            var exercise = new Exercise()
            {
                Id = Guid.NewGuid(),
                ThemeId = themId,
                Rating = rate,
                Name = name,
                Description = "For test",
                Template = "Template",
                Test = "Test"
            };
            await AddAsync(exercise);
            return exercise;
        }
        #endregion
    }
}