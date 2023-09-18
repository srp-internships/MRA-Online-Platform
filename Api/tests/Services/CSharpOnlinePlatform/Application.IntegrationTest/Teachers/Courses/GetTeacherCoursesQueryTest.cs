using Application.Teachers.Queries.CourseQuery;
using Core.Exceptions;
using Domain.Entities;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;
using static Application.IntegrationTest.TestHelper;

namespace Application.IntegrationTest.Teachers.Courses
{
    public class GetTeacherCoursesQueryTest
    {
        [Test]
        public async Task ShouldRequireValidCourseId()
        {
            var command = new GetTeacherCoursesQuery(Guid.NewGuid());

            await FluentActions.Invoking(() =>
                SendAsync(command)).Should().ThrowAsync<ValidationFailureException>();
        }

        [Test]
        public async Task GetCourses_ShouldReturnCourseOfTeacherFromDataBaseTest()
        {
            var teacherId = Guid.NewGuid();
            var course = CreateCourse(teacherId);

            await AddAsync(course);

            GetTeacherCoursesQuery query = new(teacherId);

            var coursesDto = await SendAsync(query);
            Assert.That(coursesDto.Any(s => s.Id == course.Id), Is.True);
        }

        [Test]
        public async Task GetCourses_ShouldReturnCountThemesOfCourseOfTeacherFromDataBaseTest()
        {
            var teacherId = Guid.NewGuid();
            var course = CreateCourse(teacherId);
            await AddAsync(course);
            var theme1 = CreateTheme(course);
            await AddAsync(theme1);
            var theme2 = CreateTheme(course);
            await AddAsync(theme2);

            GetTeacherCoursesQuery query = new(teacherId);

            var coursesDto = await SendAsync(query);
            var courseDto = coursesDto.FirstOrDefault(s => s.Id == course.Id);
            Assert.That(courseDto, Is.Not.Null);
            Assert.That(courseDto.TotalThemes, Is.EqualTo(2));
        }

        #region Test Data

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
        Theme CreateTheme(Course course)
        {
            return new Theme()
            {
                Id = Guid.NewGuid(),
                Name = "Arrays",
                Content = "In C#, an array is a structure representing a fixed length ordered collection of values or objects with the same type.",
                StartDate = new DateTime(2022, 07, 15),
                EndDate = new DateTime(2022, 07, 22),
                CourseId = course.Id
            };
        }
        #endregion
    }
}
