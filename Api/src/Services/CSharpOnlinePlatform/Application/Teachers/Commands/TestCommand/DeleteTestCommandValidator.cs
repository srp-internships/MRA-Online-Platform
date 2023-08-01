using Application.Common.Interfaces;
using Domain.Entities;
using FluentValidation;

namespace Application.Teachers.Commands.TestCommand
{
    public class DeleteTestCommandValidator : AbstractValidator<DeleteTestCommand>
    {
        public DeleteTestCommandValidator(IApplicationDbContext dbContext)
        {
            RuleFor(query => query.TestId).Must(testId =>
            {
                return dbContext.GetEntities<Test>().Any(t => t.Id == testId);
            }).WithMessage($"Тест не найден.");
        }
    }
}
