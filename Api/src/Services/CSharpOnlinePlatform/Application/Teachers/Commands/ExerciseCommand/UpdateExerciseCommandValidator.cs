using Application.Common.Interfaces;
using Core.ValidationsBehaviours;
using Domain.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Application.Teachers.Commands.ExerciseCommand
{
    public class UpdateExerciseCommandValidator : AbstractValidator<UpdateExerciseCommand>
    {
        public UpdateExerciseCommandValidator(IApplicationDbContext dbContext, IConfiguration configuration)
        {
            int minRate = int.Parse(configuration[ApplicationConstants.MIN_RATE_EXERCISE]);
            int maxRate = int.Parse(configuration[ApplicationConstants.MAX_RATE_EXERCISE]);
            RuleFor(c => c.ExerciseDTO.Name).NotEmpty().WithMessage(ValidationMessages.GetNotEmptyMessage("{PropertyName}"));
            RuleFor(c => c.ExerciseDTO.Template).NotEmpty().WithMessage(ValidationMessages.GetNotEmptyMessage("{PropertyName}"));
            RuleFor(c => c.ExerciseDTO.Test).NotEmpty().WithMessage(ValidationMessages.GetNotEmptyMessage("{PropertyName}"));
            RuleFor(c => c.ExerciseDTO.Rating).GreaterThanOrEqualTo(minRate).WithMessage(ValidationMessages.GetGreaterThanOrEqualToMessage("{PropertyName}", minRate))
                                            .LessThanOrEqualTo(maxRate).WithMessage(ValidationMessages.GetLessThanOrEqualToMessage("{PropertyName}", maxRate));
            RuleFor(c => c.ExerciseDTO.Description).NotEmpty().WithMessage(ValidationMessages.GetNotEmptyMessage("{PropertyName}"));

            RuleFor(query => query.ExerciseDTO.Id).Must((query, exerciseId) =>
            {
                return dbContext.GetEntities<Exercise>()
                    .Include(t => t.Theme).ThenInclude(c => c.Course)
                    .Where(t => t.Theme.Course.TeacherId == query.TeacherId)
                    .Any(t => t.Id == exerciseId);
            }).WithMessage($"Ваш доступ ограничен.");
        }
    }
}
