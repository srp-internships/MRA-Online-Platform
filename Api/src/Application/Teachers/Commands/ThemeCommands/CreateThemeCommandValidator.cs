using Application.Common.Interfaces;
using Core.ValidationsBehaviours;
using Domain.Entities;
using FluentValidation;

namespace Application.Teachers.Commands.ThemeCommands
{
    public class CreateThemeCommandValidator : AbstractValidator<CreateThemeCommand>
    {
        public CreateThemeCommandValidator(IApplicationDbContext dbContext)
        {
            RuleFor(c => c.Name).NotEmpty().WithMessage(ValidationMessages.GetNotEmptyMessage("{PropertyName}"));
            RuleFor(c => c.StartDate).NotEmpty().WithMessage(ValidationMessages.GetNotEmptyMessage("{PropertyName}"));
            RuleFor(c => c.EndDate).GreaterThan(c => c.StartDate).WithMessage(x => ValidationMessages.GetGreaterThanMessage(x.StartDate.ToShortDateString()))
                                    .NotEmpty().WithMessage(ValidationMessages.GetNotEmptyMessage("{PropertyName}"));
            RuleFor(c => c.Content).NotEmpty().WithMessage(ValidationMessages.GetNotEmptyMessage("{PropertyName}"));

            RuleFor(query => query.TeacherId).Must((query, teacherGuid) =>
            {
                return dbContext.GetEntities<Course>().Any(t => t.Id == query.CourseId && t.TeacherId == teacherGuid);
            }).WithMessage($"Ваш доступ ограничен.");
        }
    }
}
