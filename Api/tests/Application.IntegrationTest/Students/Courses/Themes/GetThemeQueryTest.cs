using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Students.Queries;
using Core.Exceptions;
using Domain.Entities;
using FluentAssertions;
using NUnit.Framework;
using static Application.IntegrationTest.TestHelper;

namespace Application.IntegrationTest.Students.Courses.Themes
{
    public class GetThemeQueryTest
    {
        [Test]
        public async Task GetTheme_ShouldReturnThemeOfCourseFromDataBaseTest()
        {
            await RunAsStudentAsync();

            var courseGuid = Guid.NewGuid();
            var course = CreateTestCourse(courseGuid);

            var theme = CreateTheme(course, DateTime.Now.AddDays(-1));

            await AddAsync(theme);

            GetThemeQuery query = new(theme.Id, GetAuthenticatedUserId());

            var themeDto = await SendAsync(query);
            themeDto.Id.Should().Be(theme.Id);
        }

        [Test]
        public async Task GetThemeWithStartDate_ShouldReturnThemeOfCourseFromDataBaseTest()
        {
            await RunAsStudentAsync();

            var courseGuid = Guid.NewGuid();
            var course = CreateTestCourse(courseGuid);

            var theme = CreateTheme(course, DateTime.Now);

            await AddAsync(theme);

            GetThemeQuery query = new(theme.Id, GetAuthenticatedUserId());

            var themeDto = await SendAsync(query);
            themeDto.Id.Should().Be(theme.Id);
        }

        [Test]
        public async Task GetThemeWithNotstarted_ShouldReturnThemeOfCourseFromDataBaseTest()
        {
            await RunAsStudentAsync();

            var courseGuid = Guid.NewGuid();
            var course = CreateTestCourse(courseGuid);

            var theme = CreateTheme(course, DateTime.Now.AddDays(23));

            await AddAsync(theme);

            GetThemeQuery query = new(theme.Id, GetAuthenticatedUserId());

            ValidationFailureException validationError = Assert.ThrowsAsync<ValidationFailureException>(() => SendAsync(query));
            var firstNameMustNotBeEmptyErrorWasShown = IsErrorExists("ThemeGuid", "Это тема ешё не началас.", validationError);
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
                Name = $"Chapter 1",
                Content = "Test",
                Course = course,
                StartDate = startDate,
                Exercises = CreateExercise()
            };
        }

        List<Exercise> CreateExercise()
        {
            var exercises = new List<Exercise>()
            {
                new Exercise()
                {
                    Id = Guid.NewGuid(),
                    Name = "Integer",
                    Description = "A distanceLis given in centimeters. Find the amount of full meters of this distance (1m=1000cm). Use the operator of integer division. ",
                    Template = "Template",
                    Test = "Test"
                },
                new Exercise()
                {
                    Id = Guid.NewGuid(),
                    Name = "String",
                    Description = "A string is an object of type String whose value is text. Internally, the text is stored as a sequential read-only collection of Char objects.",
                    Template = "Template",
                    Test = "Test"
                },
                new Exercise()
                {
                    Id = Guid.NewGuid(),
                    Name = "double",
                    Description = "There is only one implicit conversion between floating-point numeric types: from float to double.",
                    Template = "Template",
                    Test = "Test"
                }
            };
            return exercises;
        }
        #endregion
    }
}
