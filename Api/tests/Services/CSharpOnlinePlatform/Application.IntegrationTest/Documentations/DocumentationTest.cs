using Application.Documentations;
using Domain.Entities;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using FluentAssertions;
using static Application.IntegrationTest.TestHelper;
using System.Linq;
using Application.Admin.Commands.Documentations.Command;

namespace Application.IntegrationTest.Documentations
{
    public class DocumentationTest
    {
        [Test]
        public async Task UpdateDocumentation_AsyncTest()
        {
            var createDoc = new CreateDocumentationCommand()
            {
                Area = DocumentArea.Student,
                Title = "Student question 2",
                Content = "Student answer 2"
            };
            var createDocDTO = await SendAsync(createDoc);

            var query = new GetDocumentationQuery(DocumentArea.Student);
            var docDTO = await SendAsync(query);

            docDTO.Id.Should().Be(createDocDTO);
            docDTO.Title.Should().Be(createDoc.Title);
            docDTO.Content.Should().Be(createDoc.Content);

            var updateDoc = new UpdateDocumentationCommand()
            {
                Id = createDocDTO,
                Title = "Update title",
                Content = "Update content",
                Area = DocumentArea.Teacher
            };
            await SendAsync(updateDoc);

            query = new GetDocumentationQuery(DocumentArea.Teacher);
            docDTO = await SendAsync(query);

            docDTO.Id.Should().Be(createDocDTO);
            docDTO.Title.Should().Be(updateDoc.Title);
            docDTO.Content.Should().Be(updateDoc.Content);
        }
    }
}
