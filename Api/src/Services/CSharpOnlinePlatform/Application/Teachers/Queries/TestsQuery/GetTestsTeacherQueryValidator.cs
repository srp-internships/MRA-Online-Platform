using Application.Common.Interfaces;
using Domain.Entities;
using FluentValidation;

namespace Application.Teachers.Queries.TestsQuery
{
    public class GetTestsTeacherQueryValidator : AbstractValidator<GetTestsTeacherQuery>
    {
        public GetTestsTeacherQueryValidator(IApplicationDbContext dbContext)
        {
            RuleFor(s => s.ThemeId).Must(themId =>
            {
                return dbContext.GetEntities<Theme>().Any(s => s.Id == themId);
            }).WithMessage("Тема не найденa.");
        }
    }
}
