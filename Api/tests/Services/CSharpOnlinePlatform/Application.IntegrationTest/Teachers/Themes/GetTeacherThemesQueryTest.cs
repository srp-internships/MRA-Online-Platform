using Application.Teachers.Queries.ThemeQuery;
using Core.Exceptions;
using Domain.Entities;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;
using static Application.IntegrationTest.TestHelper;

namespace Application.IntegrationTest.Teachers.Themes
{
    public class GetTeacherThemesQueryTest
    {
        [Test]
        public async Task ShouldRequireValidThemeId()
        {
            var command = new GetTeacherThemesQuery(Guid.NewGuid(), Guid.NewGuid());

            await FluentActions.Invoking(() =>
                SendAsync(command)).Should().ThrowAsync<ValidationFailureException>();
        }

        [Test]
        public async Task GetThemes_ShouldReturnThemesFromDataBaseTest()
        {
            await RunAsTeacherAsync();
            var teacher = await GetAuthenticatedUser<Teacher>();

            var course = CreateCourse(teacher);
            await AddAsync(course);

            var theme = CreateTheme(course, new DateTime(2022, 07, 15));
            await AddAsync(theme);

            GetTeacherThemesQuery query = new(course.Id, course.TeacherId);

            var themesDto = await SendAsync(query);
            Assert.That(themesDto.Any(s => s.Id == theme.Id), Is.True);
        }

        [Test]
        public async Task GetOrderedThemesByStartDate_ShouldReturnThemesFromDataBaseTestAsync()
        {
            //arrange
            await RunAsTeacherAsync();
            var teacher = await GetAuthenticatedUser<Teacher>();
            var course = CreateCourse(teacher);
            await AddAsync(course);

            var theme = CreateTheme(course, DateTime.Today.AddDays(2));
            await AddAsync(theme);
            theme = CreateTheme(course, DateTime.Today.AddDays(-2));
            await AddAsync(theme);
            theme = CreateTheme(course, DateTime.Today);
            await AddAsync(theme);
            //act
            GetTeacherThemesQuery query = new(course.Id, course.TeacherId);
            var themesDto = await SendAsync(query);

            //assert
            themesDto.Should().Equal(themesDto.OrderBy(s => s.StartDate));
        }

        #region Test Data
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

        Course CreateCourse(Teacher teacher)
        {
            return new Course()
            {
                Id = Guid.NewGuid(),
                Name = "C# Training",
                LearningLanguage = "Tajik",
                TeacherId = teacher.Id
            };
        }

        #endregion
    }
}
