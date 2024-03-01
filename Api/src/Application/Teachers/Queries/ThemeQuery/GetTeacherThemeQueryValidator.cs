using Application.Common.Interfaces;
using Domain.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Teachers.Queries.ThemeQuery
{
    public class GetTeacherThemeQueryValidator : AbstractValidator<GetTeacherThemeQuery>
    {
        public GetTeacherThemeQueryValidator(IApplicationDbContext dbContext)
        {
            RuleFor(query => query.ThemeId).Must((query, themeId) =>
            {
                    return dbContext.GetEntities<Theme>()
                    .Include(t => t.Course)
                    .Where(t => t.Course.TeacherId == query.TeacherId)
                    .Any(t => t.Id == query.ThemeId);
            }).WithMessage($"Ваш доступ ограничен.");
        }
    }

}
