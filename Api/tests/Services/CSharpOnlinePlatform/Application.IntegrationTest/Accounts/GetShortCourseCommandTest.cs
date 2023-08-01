using Application.Account.Commands;
using FluentAssertions;
using NUnit.Framework;
using System.Threading.Tasks;
using static Application.IntegrationTest.TestHelper;

namespace Application.IntegrationTest.Accounts
{
    public class GetShortCourseCommandTest
    {
        [Test]
        public async Task GetShortCourseCommand_ListOfShortCourses()
        {
            var command = new GetShortCourseCommand();

            var listOfShortCourses = await SendAsync(command);

            listOfShortCourses.Should().NotBeEmpty();
        }
    }
}
