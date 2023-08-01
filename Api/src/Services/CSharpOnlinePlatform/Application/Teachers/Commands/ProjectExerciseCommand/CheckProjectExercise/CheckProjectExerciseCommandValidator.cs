using Application.Common.Interfaces;
using Core.ValidationsBehaviours;
using Domain.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Teachers.Commands.ProjectExerciseCommand.CheckProjectExercise
{
    public class CheckProjectExerciseCommandValidator : AbstractValidator<CheckProjectExerciseCommand>
    {
        public CheckProjectExerciseCommandValidator(IApplicationDbContext dbContext)
        {
            ProjectExerciseShouldExist(dbContext).DependentRules(() => CommentShouldBeNotEmpty())
                .DependentRules(() => StatusShouldNotBeNull()).DependentRules(() => ProjectExerciseWithItsTeacher(dbContext));
        }

        IRuleBuilderOptions<CheckProjectExerciseCommand, string> CommentShouldBeNotEmpty()
        {
            return RuleFor(c => c.Comment).NotEmpty().WithMessage(ValidationMessages.GetNotEmptyMessage("{PropertyName}"));
        }

        IRuleBuilderOptions<CheckProjectExerciseCommand, Status> StatusShouldNotBeNull()
        {
            return RuleFor(c => c.Status).NotNull().WithMessage(ValidationMessages.GetNotNullMessage("{PropertyName}"));
        }

        IRuleBuilderOptions<CheckProjectExerciseCommand, Guid> ProjectExerciseShouldExist(IApplicationDbContext dbContext)
        {
            return RuleFor(c => c.ProjectExerciseId).Must(projExId =>
            {
                return dbContext.GetEntities<ProjectExercise>().Any(t => t.Id == projExId);
            }).WithMessage("Проект не найден.");
        }

        IRuleBuilderOptions<CheckProjectExerciseCommand, Guid> ProjectExerciseWithItsTeacher(IApplicationDbContext dbContext)
        {
            return RuleFor(query => query.TeacherId).Must((query, teacherId) =>
            {
                return dbContext.GetEntities<StudentCourseProjectExercise>()
                    .Include(s => s.ProjectExercise).ThenInclude(s => s.Theme).ThenInclude(s => s.Course)
                    .Where(t => t.ProjectExerciseId == query.ProjectExerciseId)
                    .Any(s => s.ProjectExercise.Theme.Course.TeacherId == teacherId);
            }).WithMessage("Ваш доступ ограничен.");
        }
    }
}
