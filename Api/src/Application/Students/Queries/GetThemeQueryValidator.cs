using Application.Common.Interfaces;
using Domain.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Students.Queries
{
    public class GetThemeQueryValidator : AbstractValidator<GetThemeQuery>
    {
        public GetThemeQueryValidator(IApplicationDbContext dbContext)
        {
            RuleFor(query => query.ThemeGuid).Must(themeGuid =>
            {
                return dbContext.GetEntities<Theme>().AsNoTracking().Any(th => th.Id == themeGuid && th.StartDate.Date <= DateTime.Today);
            }).WithMessage($"Это тема ешё не началась.");
        }
    }
}
