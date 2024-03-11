using System;
using System.Threading.Tasks;
using Application.Admin.Commands.Documentations.Command;
using Core.Exceptions;
using Domain.Entities;
using FluentAssertions;
using NUnit.Framework;
using static Application.IntegrationTest.TestHelper;

namespace Application.IntegrationTest.Admin
{
    public class DeleteDocumentationCommandTest
    {
        [Test]
        public void DeleteDocumentationCommand_NotExistingDocumentationId_NotFoundException()
        {
            var command = new DeleteDocumentationCommand(Guid.NewGuid());

            ValidationFailureException validationFailureException = Assert.ThrowsAsync<ValidationFailureException>(() => SendAsync(command));
            var notFoundExceptionShown = IsErrorExists("Id", "Данный документ еще не зарегистрирован. Выберите существующий документ.", validationFailureException);

            notFoundExceptionShown.Should().BeTrue();
        }

        [Test]
        public async Task DeleteDocumentationCommand_CorrectData_Guid()
        {
            var documentation = new Documentation() { Id = Guid.NewGuid(), Content = "Content", Title = "Title" };
            await AddAsync(documentation);
            var command = new DeleteDocumentationCommand(documentation.Id);

            var documentationGuid = await SendAsync(command);

            documentationGuid.Should().Be(documentation.Id);
        }
    }
}
