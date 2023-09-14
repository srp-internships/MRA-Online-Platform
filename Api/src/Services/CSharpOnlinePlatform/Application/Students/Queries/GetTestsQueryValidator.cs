using Application.Common.Interfaces;
using Domain.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Students.Queries
{
    public class GetTestsQueryValidator : AbstractValidator<GetExercisesQuery>
    {
        public GetTestsQueryValidator(IApplicationDbContext dbContext)
        {
            RuleFor(query => query.ThemeId).Must(themeGuid =>
            {
                return dbContext.GetEntities<Theme>().Any(t => t.Id == themeGuid);
            }).WithMessage($"Тема не найденa.");
        }
    }
}
