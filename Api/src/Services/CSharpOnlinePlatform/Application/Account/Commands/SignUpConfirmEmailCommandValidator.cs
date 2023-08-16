// using Application.Common.Interfaces;
// using Core.ValidationsBehaviours;
// using Domain.Entities;
// using FluentValidation;
//
// namespace Application.Account.Commands
// {
//     public class SignUpConfirmEmailCommandValidator : AbstractValidator<SignUpConfirmEmailCommand>
//     {
//         public SignUpConfirmEmailCommandValidator(IApplicationDbContext dbContext)
//         {
//             RuleFor(s => s.Token).NotEmpty().WithMessage(ValidationMessages.GetNotEmptyMessage("{PropertyName}"));
//             RuleFor(s => s.Email).Must((query, email) =>
//             {
//                 return dbContext.GetEntities<User>().Any(s => s.Email == email);
//             }).WithMessage($"Пользователь не найден.");
//         }
//     }
// }
