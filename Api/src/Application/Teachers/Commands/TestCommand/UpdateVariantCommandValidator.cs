using Application.Common.Interfaces;
using Core.ValidationsBehaviours;
using Domain.Entities;
using FluentValidation;
using Microsoft.Extensions.Configuration;

namespace Application.Teachers.Commands.TestCommand
{
    public class UpdateVariantCommandValidator : AbstractValidator<UpdateVariantCommand>
    {
        public UpdateVariantCommandValidator(IApplicationDbContext dbContext, IConfiguration configuration)
        {
            RuleFor(s => s.Variants.Where(s => s.Value == "")).Empty().WithMessage(ValidationMessages.GetNotEmptyMessage("{PropertyName}"));
            RuleFor(s => s.Variants[0].TestId).Must(testId =>
            {
                return dbContext.GetEntities<Test>().Any(s => s.Id == testId);
            }).WithMessage("Тест не найден.");
        }
    }
}
