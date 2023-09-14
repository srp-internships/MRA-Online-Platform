using Application.Common.Interfaces;
using Domain.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Students
{
    public class GetStudentQueryValidator : AbstractValidator<GetStudentQuery>
    {
        public GetStudentQueryValidator(IApplicationDbContext applicationDbContext)
        {
            RuleFor(teacherQuery => teacherQuery.StudentGuid).Must(studentGuid =>
            {
                return applicationDbContext.GetEntities<StudentCourse>().AsNoTracking().Any(s => s.StudentId == studentGuid);
            }).WithMessage($"Ваш доступ ограничен.");
        }
    }
}