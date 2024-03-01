using Application.Common.Interfaces;
using Core.ValidationsBehaviours;
using Domain.Entities;
using FluentValidation;
using Microsoft.Extensions.Configuration;

namespace Application.Teachers.Commands.TestCommand
{
    public class UpdateTestCommandValidator : AbstractValidator<UpdateTestCommand>
    {
        public UpdateTestCommandValidator(IApplicationDbContext dbContext, IConfiguration configuration)
        {
            int minRate = int.Parse(configuration[ApplicationConstants.MIN_RATE_EXERCISE]);
            int maxRate = int.Parse(configuration[ApplicationConstants.MAX_RATE_EXERCISE]);
            RuleFor(c => c.Name).NotEmpty().WithMessage(ValidationMessages.GetNotEmptyMessage("{PropertyName}"));
            RuleFor(c => c.Description).NotEmpty().WithMessage(ValidationMessages.GetNotEmptyMessage("{PropertyName}"));
            RuleFor(c => c.Rating).GreaterThanOrEqualTo(minRate).WithMessage(ValidationMessages.GetGreaterThanOrEqualToMessage("{PropertyName}", minRate))
                                            .LessThanOrEqualTo(maxRate).WithMessage(ValidationMessages.GetLessThanOrEqualToMessage("{PropertyName}", maxRate));

            RuleFor(query => query.Id).Must(testID =>
            {
                return dbContext.GetEntities<Test>().Any(t => t.Id == testID);
            }).WithMessage($"Тест не найден.");
        }
    }
}
