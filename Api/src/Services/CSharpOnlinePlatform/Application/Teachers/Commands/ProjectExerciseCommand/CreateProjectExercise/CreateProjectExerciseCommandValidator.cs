using Application.Common.Interfaces;
using Domain.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Teachers.Commands.ProjectExerciseCommand.CreateProjectExercise
{
    public class CreateProjectExerciseCommandValidator : AbstractValidator<CreateProjectExerciseCommand>
    {
        public CreateProjectExerciseCommandValidator(IApplicationDbContext dbContext)
        {
            RuleFor(c => c.Exercise.Name).NotNull().NotEmpty();
            RuleFor(c => c.Exercise.Rating).GreaterThanOrEqualTo(0).LessThanOrEqualTo(10);
            RuleFor(c => c.Exercise.Description).NotNull().NotEmpty();

            RuleFor(query => query.Exercise.ThemeId).Must((query, themeId) =>
            {
                return dbContext.GetEntities<Theme>()
                    .Include(t => t.Course)
                    .Where(t => t.Course.TeacherId == query.TeacherId)
                    .Any(t => t.Id == themeId);
            }).WithMessage($"Ваш доступ ограничен.");
        }
    }
}
