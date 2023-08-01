using Application.Common.Interfaces;
using Domain.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Students.Queries
{
    public class GetExerciseQueryValidator : AbstractValidator<GetExercisesQuery>
    {
        public GetExerciseQueryValidator(IApplicationDbContext dbContext)
        {
            RuleFor(query => query.ThemeId).Must(themeGuid =>
            {
                return dbContext.GetEntities<Theme>().AsNoTracking().Any(t => t.Id == themeGuid);
            }).WithMessage($"Ваш доступ ограничен.");
        }
    }
}
