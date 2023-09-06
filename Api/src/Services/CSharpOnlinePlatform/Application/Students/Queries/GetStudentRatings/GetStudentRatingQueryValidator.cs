using Application.Common.Interfaces;
using Domain.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Students.Queries
{
    public class GetStudentRatingQueryValidator : AbstractValidator<GetStudentRatingQuery>
    {
        public GetStudentRatingQueryValidator(IApplicationDbContext dbContext)
        {
            RuleFor(s => s.CourseId).Must(courseId =>
            {
                return dbContext.GetEntities<StudentCourse>().AsNoTracking()
                .Any(s => s.CourseId == courseId);
            }).WithMessage("Ваш доступ ограничен.");
        }
    }
}
