using Application.Common.Interfaces;
using Core.ValidationsBehaviours;
using Domain.Entities;
using FluentValidation;

namespace Application.Teachers.Commands.CourseCommand
{
    public class UpdateCourseCommandValidator : AbstractValidator<UpdateCourseCommand>
    {
        public UpdateCourseCommandValidator(IApplicationDbContext dbContext)
        {
            RuleFor(c => c.CourseName).NotEmpty().WithMessage(ValidationMessages.GetNotEmptyMessage("{PropertyName}"));
            RuleFor(c => c.CourseLanguage).NotEmpty().WithMessage(ValidationMessages.GetNotEmptyMessage("{PropertyName}"));

            RuleFor(query => query.TeacherId).Must((query, teacherGuid) =>
            {
                return dbContext.GetEntities<Course>().Any(t => t.TeacherId == teacherGuid && t.Id == query.CourseId);
            }).WithMessage($"Ваш доступ ограничен.");
        }
    }
}
