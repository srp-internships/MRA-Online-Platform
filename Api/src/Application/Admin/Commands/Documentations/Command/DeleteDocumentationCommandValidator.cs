using Application.Common.Interfaces;
using Domain.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Admin.Commands.Documentations.Command
{
    public class DeleteDocumentationCommandValidator : AbstractValidator<DeleteDocumentationCommand>
    {
        public DeleteDocumentationCommandValidator(IApplicationDbContext dbContext)
        {
            RuleFor(s => s.Id).
                Must(id => dbContext.GetEntities<Documentation>().Any(s => s.Id == id))
                .WithMessage($"Данный документ еще не зарегистрирован. Выберите существующий документ.");
        }
    }
}
