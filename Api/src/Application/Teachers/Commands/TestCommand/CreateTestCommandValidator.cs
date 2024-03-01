using Application.Common.Interfaces;
using Core.ValidationsBehaviours;
using Domain.Entities;
using FluentValidation;
using Microsoft.Extensions.Configuration;

namespace Application.Teachers.Commands.TestCommand
{
    public class CreateTestCommandValidator : AbstractValidator<CreateTestCommand>
    {
        public CreateTestCommandValidator(IApplicationDbContext dbContext, IConfiguration configuration)
        {
            int minRate = int.Parse(configuration[ApplicationConstants.MIN_RATE_EXERCISE]);
            int maxRate = int.Parse(configuration[ApplicationConstants.MAX_RATE_EXERCISE]);
            RuleFor(s => s.Name).NotEmpty().WithMessage(ValidationMessages.GetNotEmptyMessage("{PropertyName}"));
            RuleFor(s => s.Description).NotEmpty().WithMessage(ValidationMessages.GetNotEmptyMessage("{PropertyName}"));
            RuleFor(s => s.Rating).GreaterThanOrEqualTo(minRate).WithMessage(ValidationMessages.GetGreaterThanOrEqualToMessage("{PropertyName}", minRate))
                                            .LessThanOrEqualTo(maxRate).WithMessage(ValidationMessages.GetLessThanOrEqualToMessage("{PropertyName}", maxRate));

            RuleFor(s => s.ThemeId).Must(themId =>
            {
                return dbContext.GetEntities<Theme>().Any(s => s.Id == themId);
            }).WithMessage("Тема не найденa.");
        }
    }
}
