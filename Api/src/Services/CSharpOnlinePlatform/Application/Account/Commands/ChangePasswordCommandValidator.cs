// using Application.Common.Interfaces;
// using Domain.Entities;
// using FluentValidation;
//
// namespace Application.Account.Commands 
// {
//     public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
//     {
//         public ChangePasswordCommandValidator(IApplicationDbContext dbContext)
//         {
//             RuleFor(query => query.Email).Must((query, email) =>
//               {
//                   return dbContext.GetEntities<User>().Any(st => st.Email == email);
//               }).WithMessage($"Пользователь не найден.");
//         }
//     }
// }
