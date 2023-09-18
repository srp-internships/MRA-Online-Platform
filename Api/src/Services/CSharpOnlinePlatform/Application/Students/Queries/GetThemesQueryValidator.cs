using Application.Common.Interfaces;
using Domain.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Students.Queries
{
    public class GetThemesQueryValidator : AbstractValidator<GetThemesQuery>
    {
        public GetThemesQueryValidator(IApplicationDbContext applicationDbContext)
        {
            RuleFor(query => query.CourseGuid).Must(courseGuid =>
            {
                return applicationDbContext.GetEntities<Course>().AsNoTracking().Any(s => s.Id == courseGuid);
            }).WithMessage($"Ваш доступ ограничен.");
            
        }
    }
}
