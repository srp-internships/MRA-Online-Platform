using Application.Common.Interfaces;
using Domain.Entities;
using FluentValidation;

namespace Application.Teachers.Commands.CourseCommand
{
    public class DeleteCourseCommandValidator : AbstractValidator<DeleteCourseCommand>
    {
        public DeleteCourseCommandValidator(IApplicationDbContext dbContext)
        {
            RuleFor(query => query.TeacherId).Must((query, teacherGuid) =>
            {
                return dbContext.GetEntities<Course>().Any(t => t.TeacherId == teacherGuid && t.Id == query.CourseId);
            }).WithMessage($"Ваш доступ ограничен.");
        }
    }
}
