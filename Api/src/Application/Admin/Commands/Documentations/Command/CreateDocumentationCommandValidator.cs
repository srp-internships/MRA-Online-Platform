using Application.Admin.Commands.Documentations.Command;
using Application.Common.Interfaces;
using Core.ValidationsBehaviours;
using Domain.Entities;
using FluentValidation;

namespace Application.Admin.Commands.Documentations
{
    public class CreateDocumentationCommandValidator : AbstractValidator<CreateDocumentationCommand>
    {
        public CreateDocumentationCommandValidator(IApplicationDbContext dbContext)
        {
            RuleFor(s => s.Title).NotEmpty().WithMessage(ValidationMessages.GetNotEmptyMessage("{PropertyName}"));
            RuleFor(s => s.Content).NotEmpty().WithMessage(ValidationMessages.GetNotEmptyMessage("{PropertyName}"));

            RuleFor(s => s.Area).
                 Must(area => !dbContext.GetEntities<Documentation>().Any(s => s.Area == area)).WithMessage($"Данный документ существующий.");
        }
    }
}
