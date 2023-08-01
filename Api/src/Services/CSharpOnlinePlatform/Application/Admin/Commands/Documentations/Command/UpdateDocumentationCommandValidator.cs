using Core.ValidationsBehaviours;
using FluentValidation;

namespace Application.Admin.Commands.Documentations.Command
{
    public class UpdateDocumentationCommandValidator : AbstractValidator<UpdateDocumentationCommand>
    {
        public UpdateDocumentationCommandValidator()
        {
            RuleFor(s => s.Id).NotEmpty().WithMessage(ValidationMessages.GetNotEmptyMessage("{PropertyName}"));
            RuleFor(s => s.Title).NotEmpty().WithMessage(ValidationMessages.GetNotEmptyMessage("{PropertyName}"));
            RuleFor(s => s.Content).NotEmpty().WithMessage(ValidationMessages.GetNotEmptyMessage("{PropertyName}"));
        }
    }
}
