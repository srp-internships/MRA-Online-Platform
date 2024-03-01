using Application.Common.Interfaces;
using Domain.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Teachers.Queries.ProjectExerciseQuery
{
    public class GetProjectExerciseQueryValidator:AbstractValidator<GetProjectExerciseQuery>
    {
        public GetProjectExerciseQueryValidator(IApplicationDbContext dbContext)
        {
            RuleFor(query => query.ThemeId).Must((query, themeId) =>
            {
                return dbContext.GetEntities<Theme>()
                .Include(t=>t.Course)
                .Where(t=>t.Course.TeacherId == query.TeacherId)
                .Any(t => t.Id == themeId);
            }).WithMessage($"Ваш доступ ограничен.");
        }
    }
}
