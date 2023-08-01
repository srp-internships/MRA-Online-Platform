using Application.Teachers.Queries.ThemeQuery;
using Core.Exceptions;
using Domain.Entities;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using static Application.IntegrationTest.TestHelper;

namespace Application.IntegrationTest.Teachers.Themes
{
    public class GetTeacherThemeQueryTest
    {
        [Test]
        public void GetTeacherThemeQuery_NotExistingThemeId_ForbiddenException()
        {
            var command = new GetTeacherThemeQuery(Guid.NewGuid(), Guid.NewGuid());

            ValidationFailureException validationFailureException = Assert.ThrowsAsync<ValidationFailureException>(() => SendAsync(command));

            var forbiddenExceptionShown = IsErrorExists("ThemeId", "Ваш доступ ограничен.", validationFailureException);
            forbiddenExceptionShown.Should().BeTrue();
        }

        [Test]
        public async Task GetTeacherThemeQuery_CorrectData_ThemeDTO()
        {
            await RunAsTeacherAsync();
            var teacher = await GetAuthenticatedUser<Teacher>();
            var course = new Course() { Id = Guid.NewGuid(), LearningLanguage = "Tajik", Name = "Name", TeacherId = teacher.Id };
            await AddAsync(course);
            var theme = new Theme() { Id = Guid.NewGuid(), Name = "Name", Content = "Content", CourseId = course.Id };
            await AddAsync(theme);
            var command = new GetTeacherThemeQuery(theme.Id, teacher.Id);

            var themeDTO = await SendAsync(command);

            themeDTO.Id.Should().Be(themeDTO.Id);
        }
    }
}
