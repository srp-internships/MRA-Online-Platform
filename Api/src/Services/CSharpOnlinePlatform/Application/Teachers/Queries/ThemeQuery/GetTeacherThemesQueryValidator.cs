using Application.Common.Interfaces;
using Core.ValidationsBehaviours;
using Domain.Entities;
using FluentValidation;

namespace Application.Teachers.Queries.ThemeQuery
{
    public class GetTeacherThemesQueryValidator : AbstractValidator<GetTeacherThemesQuery>
    {
        public GetTeacherThemesQueryValidator(IApplicationDbContext applicationDbContext)
        {
            RuleFor(query => query.CourseId).Must((query, courseGuid) => 
                 applicationDbContext.GetEntities<Course>().Any(s => s.Id == courseGuid && s.TeacherId == query.TeacherId))
            .WithMessage($"Ваш доступ ограничен.");
        }
    }
}
