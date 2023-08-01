using Application.Common.Interfaces;
using Domain.Entities;
using FluentValidation;

namespace Application.Account.Commands
{
    public class ForgotPasswordCommandValidator : AbstractValidator<ForgotPasswordCommand>
    {
        public ForgotPasswordCommandValidator(IApplicationDbContext dbContext)
        {
            RuleFor(query => query.Email).Must((query, email) =>
            {
                return dbContext.GetEntities<User>().Any(st => st.Email == email);
            }).WithMessage($"Пользователь не найден.");
        }
    }
}
