// using Application.Common.Interfaces;
// using Domain.Entities;
// using FluentValidation;
// using Microsoft.AspNetCore.Identity;
//
// namespace Application.Account.Commands
// {
//     public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
//     {
//         public LoginUserCommandValidator(UserManager<User> userManager, IApplicationDbContext context)
//         {
//             RuleFor(query => query.Email)
//                 .Must((query, email) =>
//                     {
//                         var user = context.GetEntities<User>().SingleOrDefault(u => u.Email == email);
//                         if (user != null)
//                         {
//                             return userManager.CheckPasswordAsync(user, query.Password).Result;
//                         }
//                         return false;
//                     }
//                  )
//                 .WithMessage("Неправильный логин или пароль.");
//
//             RuleFor(query => query.Email)
//               .Must((query, email) =>
//               {
//                   var user = context.GetEntities<User>().SingleOrDefault(u => u.Email == email);
//                   if (user != null)
//                       return user.EmailConfirmed;
//                   return true;
//               }
//                )
//               .WithMessage("Ваша почта не подтверждена. Сначала подтвердите адрес электронной почты");
//         }
//     }
// }
