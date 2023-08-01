using System;
using System.Threading.Tasks;
using Application.Teachers.Queries.TestsQuery;
using Core.Exceptions;
using Domain.Entities;
using FluentAssertions;
using NUnit.Framework;
using static Application.IntegrationTest.TestHelper;

namespace Application.IntegrationTest.Teachers.Tests
{
    public class GetTestsTeacherQueryTest
    {
        [Test]
        public void GetTestsTeacherQuery_NotExistingThemeId_NotFoundException()
        {
            var command = new GetTestsTeacherQuery(Guid.NewGuid());

            ValidationFailureException validationFailureException = Assert.ThrowsAsync<ValidationFailureException>(() => SendAsync(command));
            var notFoundExceptionShown = IsErrorExists("ThemeId", "Тема не найденa.", validationFailureException);

            notFoundExceptionShown.Should().BeTrue();
        }

        [Test]
        public async Task GetTestsTeacherQuery_CorrectData_ListOfTests()
        {
            var theme = new Theme() { Id = Guid.NewGuid(), Content = "Content", Name = "Name" };
            await AddAsync(theme);
            var test = new Test() { Id = Guid.NewGuid(), Description = "", Name = "", Rating = 4, ThemeId = theme.Id };
            await AddAsync(test);
            var command = new GetTestsTeacherQuery(theme.Id);

            var listOfTests = await SendAsync(command);

            listOfTests.Should().NotBeEmpty();
        }
    }
}
