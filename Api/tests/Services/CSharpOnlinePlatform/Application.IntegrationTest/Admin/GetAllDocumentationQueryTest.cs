using System;
using System.Threading.Tasks;
using Application.Admin.Commands.Documentations;
using Domain.Entities;
using FluentAssertions;
using NUnit.Framework;
using static Application.IntegrationTest.TestHelper;

namespace Application.IntegrationTest.Admin
{
    public class GetAllDocumentationQueryTest
    {
        [Test]
        public async Task GetAllDocumentationQuery_ListOfDocumentation()
        {
            var documentation = new Documentation() { Id = Guid.NewGuid(), Content = "Content", Title = "Title" };
            await AddAsync(documentation);

            var command = new GetAllDocumentationQuery();
            var listOfDocumentations = await SendAsync(command);

            listOfDocumentations.Should().NotBeEmpty();
        }
    }
}
