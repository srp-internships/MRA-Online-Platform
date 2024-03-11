using Application.Common.Interfaces;
using Application.Teachers.Commands.TestCommand;
using Domain.Entities;
using FluentValidation;

namespace Application.Teachers.Commands.ProjectExerciseCommand.DeleteProjectExercise
{
    public class DeleteProjectExerciseCommandValidator : AbstractValidator<DeleteProjectExerciseCommand>
    {
        public DeleteProjectExerciseCommandValidator(IApplicationDbContext dbContext)
        {
            RuleFor(query => query.ProjectExerciseID).Must(proExerciseId =>
            {
                return dbContext.GetEntities<ProjectExercise>().Any(t => t.Id == proExerciseId);
            }).WithMessage($"Проект не найден.");
        }
    }
}
