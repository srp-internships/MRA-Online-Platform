using Application.Common.Interfaces;
using Domain.Entities;
using FluentValidation;

namespace Application.Teachers.Commands.ThemeCommands
{
    public class DeleteThemeCommandValidator : AbstractValidator<DeleteThemeCommand>
    {
        public DeleteThemeCommandValidator(IApplicationDbContext dbContext)
        {
            RuleFor(query => query.TeacherId).Must((query, teacherGuid) =>
            {
                return dbContext.GetEntities<Theme>()
                    .Any(t => t.Id == query.ThemeId);
            }).WithMessage($"Invalid theme id");
        }
    }
}