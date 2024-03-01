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
        public async Task GetTeacherThemeQuery_CorrectData_ThemeDTO()
        {
            var teacherId = Guid.NewGuid();
            var course = new Course() { Id = Guid.NewGuid(), LearningLanguage = "Tajik", Name = "Name", TeacherId = teacherId };
            await AddAsync(course);
            var theme = new Theme() { Id = Guid.NewGuid(), Name = "Name", Content = "Content", CourseId = course.Id };
            await AddAsync(theme);
            var command = new GetTeacherThemeQuery(theme.Id, teacherId);

            var themeDTO = await SendAsync(command);

            themeDTO.Id.Should().Be(themeDTO.Id);
        }
    }
}
