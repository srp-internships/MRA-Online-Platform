using Application.Common.Interfaces;
using Domain.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Teachers.Commands.ExerciseCommand
{
    public class DeleteExerciseCommandValidator : AbstractValidator<DeleteExerciseCommand>
    {
        public DeleteExerciseCommandValidator(IApplicationDbContext dbContext)
        {
            RuleFor(query => query.ExerciseId).Must((query, exerciseId) =>
            {
                return dbContext.GetEntities<Exercise>()
                    .Include(t => t.Theme).ThenInclude(c => c.Course)
                    .Where(t => t.Theme.Course.TeacherId == query.TeacherId)
                    .Any(t => t.Id == exerciseId);
            }).WithMessage($"Ваш доступ ограничен.");
        }
    }
}
