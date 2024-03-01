using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Students.Queries;
using Core.Exceptions;
using Domain.Entities;
using FluentAssertions;
using NUnit.Framework;
using static Application.IntegrationTest.TestHelper;

namespace Application.IntegrationTest.Students.Courses
{
    public class GetThemesQueryTest
    {
        [Test]
        public async Task GetThemes_ShouldReturnThemesOfCourseFromDataBaseTest()
        {
            await RunAsStudentAsync();

            var courseGuid = Guid.NewGuid();
            var course = CreateTestCourse(courseGuid);
            var themes = CreateThemes(course, DateTime.Now.AddDays(-3));

            await AddRangeAsync(themes);

            GetThemesQuery query = new(courseGuid, GetAuthenticatedUserId());

            var themesDto = await SendAsync(query);
            themesDto.Count.Should().Be(2);
        }

        [Test]
        public async Task GetThemesWithStartDate_ShouldReturnThemesOfCourseFromDataBaseTest()
        {
            await RunAsStudentAsync();

            var courseGuid = Guid.NewGuid();
            var course = CreateTestCourse(courseGuid);
            var themes = CreateThemes(course, DateTime.Now.AddDays(-1));

            await AddRangeAsync(themes);

            GetThemesQuery query = new(courseGuid, GetAuthenticatedUserId());

            var themesDto = await SendAsync(query);
            themesDto.SingleOrDefault(s => s.StartDate.Date <= DateTime.Today).IsStarted.Should().BeTrue();
            themesDto.SingleOrDefault(s => s.StartDate.Date > DateTime.Today).IsStarted.Should().BeFalse();
        }

        [Test]
        public async Task GetOrderedThemesByStartDate_ShouldReturnThemesFromDataBaseTestAsync()
        {
            //arrange
            await RunAsStudentAsync();

            var courseGuid = Guid.NewGuid();
            var course = CreateTestCourse(courseGuid);
            await AddAsync(course);
            var theme = CreateTheme(course, DateTime.Today.AddDays(-1));
            await AddAsync(theme);
            theme = CreateTheme(course, DateTime.Today.AddDays(2));
            await AddAsync(theme);
            theme = CreateTheme(course, DateTime.Today);
            await AddAsync(theme);

            //act
            GetThemesQuery query = new(courseGuid, GetAuthenticatedUserId());
            var themesDto = await SendAsync(query);

            //assert
            themesDto.Should().Equal(themesDto.OrderBy(s => s.StartDate));
        }
        
        #region Test Data

        Course CreateTestCourse(Guid guid)
        {
            return new Course()
            {
                Id = guid,
                Name = "C# Training",
                LearningLanguage = "Tajik",
            };
        }

        Theme CreateTheme(Course course, DateTime startDate)
        {
            return new Theme()
            {
                Id = Guid.NewGuid(),
                Name = "Arrays",
                Content = "In C#, an array is a structure representing a fixed length ordered collection of values or objects with the same type.",
                StartDate = startDate,
                EndDate = startDate.AddDays(7),
                CourseId = course.Id
            };
        }

        List<Theme> CreateThemes(Course course, DateTime startDate)
        {
            var themes = new List<Theme>()
            {
               new Theme()
               {
                   Id = Guid.NewGuid(),
                   Name = $"Chapter 1",
                   Content = "Test",
                   Course = course,
                   StartDate = startDate.AddDays(2)
               },
               new Theme()
               {
                   Id = Guid.NewGuid(),
                   Name = $"Chapter 2",
                   Content = "Test",
                   Course = course,
                   StartDate = startDate
               }
            };
            return themes;
        }
        #endregion
    }
}
