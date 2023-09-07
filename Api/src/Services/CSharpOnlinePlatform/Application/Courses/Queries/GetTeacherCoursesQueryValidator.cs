using Application.Common.Interfaces;
using Domain.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Teachers.Queries.CourseQuery
{
    public class GetTeacherCoursesQueryValidator : AbstractValidator<GetTeacherCoursesQuery>
    {
        public GetTeacherCoursesQueryValidator(IApplicationDbContext dbContext)
        {
            RuleFor(query => query.TeacherId).Must(teacherGuid =>
            {
                return dbContext.GetEntities<Teacher>().AsNoTracking().Any(t => t.Id == teacherGuid);
            }).WithMessage($"Ваш доступ ограничен.");
        }
    }
}
