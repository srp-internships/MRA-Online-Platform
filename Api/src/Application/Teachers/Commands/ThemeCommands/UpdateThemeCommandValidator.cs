using Application.Common.Interfaces;
using Core.ValidationsBehaviours;
using Domain.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Teachers.Commands.ThemeCommands
{
    public class UpdateThemeCommandValidator : AbstractValidator<UpdateThemeCommand>
    {
        public UpdateThemeCommandValidator(IApplicationDbContext dbContext)
        {
            RuleFor(c => c.Name).NotEmpty().WithMessage(ValidationMessages.GetNotEmptyMessage("{PropertyName}"));
            RuleFor(c => c.StartDate).NotEmpty().WithMessage(ValidationMessages.GetNotEmptyMessage("{PropertyName}"));
            RuleFor(c => c.EndDate).GreaterThan(c => c.StartDate).WithMessage(x => ValidationMessages.GetGreaterThanMessage(x.StartDate.ToShortDateString()));
            RuleFor(c => c.Content).NotEmpty().WithMessage(ValidationMessages.GetNotEmptyMessage("{PropertyName}"));
            RuleFor(query => query.TeacherId).Must((query, teacherGuid) =>
            {
                return dbContext.GetEntities<Theme>()
                    .Any(t => t.Id == query.ThemeId);
            }).WithMessage("Invalid theme id");
        }
    }
}
