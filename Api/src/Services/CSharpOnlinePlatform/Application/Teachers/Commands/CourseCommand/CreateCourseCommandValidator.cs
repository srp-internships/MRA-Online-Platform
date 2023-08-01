using Application.Common.Interfaces;
using Core.ValidationsBehaviours;
using Domain.Entities;
using FluentValidation;

namespace Application.Teachers.Commands.CourseCommand
{
    public class CreateCourseCommandValidator : AbstractValidator<CreateCourseCommand>
    {
        public CreateCourseCommandValidator(IApplicationDbContext dbContext)
        {
            RuleFor(c => c.Name).NotEmpty().WithMessage(ValidationMessages.GetNotEmptyMessage("{PropertyName}"));
            RuleFor(c => c.CourseLanguage).NotEmpty().WithMessage(ValidationMessages.GetNotEmptyMessage("{PropertyName}"));

            RuleFor(query => query.TeacherId).Must(teacherGuid =>
            {
                return dbContext.GetEntities<Teacher>().Any(t => t.Id == teacherGuid);
            }).WithMessage($"Ваш доступ ограничен.");
        }
    }
}
