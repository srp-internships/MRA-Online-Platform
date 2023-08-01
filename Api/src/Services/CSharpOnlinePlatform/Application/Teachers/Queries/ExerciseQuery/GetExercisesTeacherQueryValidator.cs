using Application.Common.Interfaces;
using Core.ValidationsBehaviours;
using Domain.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Teachers.Queries.ExerciseQuery
{
    public class GetExercisesTeacherQueryValidator : AbstractValidator<GetExercisesTeacherQuery>
    {
        public GetExercisesTeacherQueryValidator(IApplicationDbContext dbContext)
        {
            RuleFor(exerciseQuery => exerciseQuery.ThemeId).NotEmpty().WithMessage(ValidationMessages.GetNotEmptyMessage("{PropertyName}"));

            RuleFor(query => query.TeacherId).Must((query, teacherId) =>
            {
                return dbContext.GetEntities<Theme>().AsNoTracking()
                    .Include(t => t.Course)
                    .Any(t => t.Course.TeacherId == teacherId && t.Id==query.ThemeId);
            }).WithMessage($"Ваш доступ ограничен.");
        }
    }
}