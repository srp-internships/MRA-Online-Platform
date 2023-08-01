using Application.Common.Interfaces;
using Domain.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Teachers
{
    public class GetTeacherQueryValidator : AbstractValidator<GetTeacherQuery>
    {
        public GetTeacherQueryValidator(IApplicationDbContext applicationDbContext)
        {
            RuleFor(teacherQuery => teacherQuery.TeacherId).Must(teacherId =>
            {
                return applicationDbContext.GetEntities<Teacher>().AsNoTracking().Any(s => s.Id == teacherId);
            }).WithMessage($"Ваш доступ ограничен.");
        }
    }
}
