// using Application.Common.Interfaces;
// using Application.JWTToken;
// using Domain.Entities;
// using FluentValidation;
// using Microsoft.AspNetCore.Identity;
// using Microsoft.Extensions.Configuration;
//
// namespace Application.Account.Commands
// {
//     public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
//     {
//         public RefreshTokenCommandValidator(UserManager<User> userManager, IApplicationDbContext context, IJWTManagerRepository jwtManager, IConfiguration iconfiguration)
//         {
//             string refreshToken = null;
//             RuleFor(query => query.Token)
//                 .MustAsync(async (token, cancellationToken) =>
//                 {
//                     var username = jwtManager.GetPrincipalFromExpiredToken(token.AccessToken);
//                     var user = await userManager.FindByEmailAsync(username.Identity.Name);
//                     refreshToken = await userManager.GetAuthenticationTokenAsync(user, iconfiguration[ApplicationConstants.JWT_TOKEN_PROVIDER], iconfiguration[ApplicationConstants.JWT_TOKEN_NAME]);
//                     return refreshToken != null;
//                 }).WithMessage($"Неверная попытка!");
//         }
//     }
// }
