using Application.Common.Interfaces;
using Core.ValidationsBehaviours;
using Domain.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Teachers.Queries.RatingQuery
{
    public class GetStudentsRatingQueryValidator : AbstractValidator<GetStudentsRatingQuery>
    {
        public GetStudentsRatingQueryValidator(IApplicationDbContext dbContext)
        {
            RuleFor(s => s.CourseId).NotEmpty().WithMessage(ValidationMessages.GetNotEmptyMessage("{PropertyName}"));
            RuleFor(s => s.TeacherId).Must(teacherId =>
            {
                return dbContext.GetEntities<Course>().AsNoTracking()
                .Any(s => s.TeacherId == teacherId);
            }).WithMessage("Ваш доступ ограничен.");
        }
    }
}
