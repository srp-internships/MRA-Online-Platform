using Application.Common.Interfaces;
using Core.ValidationsBehaviours;
using Domain.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Application.Teachers.Commands.ExerciseCommand
{
    public class CreateExerciseCommandValidator : AbstractValidator<CreateExerciseCommand>
    {
        public CreateExerciseCommandValidator(IApplicationDbContext dbContext, IConfiguration configuration)
        {
            int minRate = int.Parse(configuration[ApplicationConstants.MIN_RATE_EXERCISE]);
            int maxRate = int.Parse(configuration[ApplicationConstants.MAX_RATE_EXERCISE]);
            RuleFor(c => c.ExerciseDTO.Name).NotEmpty().WithMessage(ValidationMessages.GetNotEmptyMessage("{PropertyName}"));
            RuleFor(c => c.ExerciseDTO.Template).NotEmpty().WithMessage(ValidationMessages.GetNotEmptyMessage("{PropertyName}"));
            RuleFor(c => c.ExerciseDTO.Test).NotEmpty().WithMessage(ValidationMessages.GetNotEmptyMessage("{PropertyName}"));
            RuleFor(c => c.ExerciseDTO.Rating).GreaterThanOrEqualTo(minRate).WithMessage(ValidationMessages.GetGreaterThanOrEqualToMessage("{PropertyName}", minRate))
                                            .LessThanOrEqualTo(maxRate).WithMessage(ValidationMessages.GetLessThanOrEqualToMessage("{PropertyName}", maxRate));
            RuleFor(c => c.ExerciseDTO.Description).NotEmpty().WithMessage(ValidationMessages.GetNotEmptyMessage("{PropertyName}"));

            RuleFor(query => query.ExerciseDTO.ThemeId).Must((query, themeId) =>
            {
                return dbContext.GetEntities<Theme>()
                    .Include(t => t.Course)
                    .Where(t => t.Course.TeacherId == query.TeacherId)
                    .Any(t => t.Id == themeId);
            }).WithMessage($"Ваш доступ ограничен.");
        }
    }
}
