// using Application.Common.Interfaces;
// using Core.ValidationsBehaviours;
// using Domain.Entities;
// using FluentValidation;
//
// namespace Application.Admin.Commands.TeacherCommand
// {
//     public class CreateTeacherCommandValidator : AbstractValidator<CreateTeacherCommand>
//     {
//         public CreateTeacherCommandValidator(IApplicationDbContext dbContext)
//         {
//             RuleFor(x => x.FirstName).NotEmpty().WithMessage(ValidationMessages.GetNotEmptyMessage("{PropertyName}"));
//             RuleFor(x => x.LastName).NotEmpty().WithMessage(ValidationMessages.GetNotEmptyMessage("{PropertyName}"));
//             RuleFor(x => x.DateOfBirth).NotEmpty().WithMessage(ValidationMessages.GetNotEmptyMessage("{PropertyName}"));
//             RuleFor(x => x.Password).NotEmpty().WithMessage(ValidationMessages.GetNotEmptyMessage("{PropertyName}"));
//             RuleFor(x => x.Country).NotEmpty().WithMessage(ValidationMessages.GetNotEmptyMessage("{PropertyName}"));
//             RuleFor(x => x.Region).NotEmpty().WithMessage(ValidationMessages.GetNotEmptyMessage("{PropertyName}"));
//             RuleFor(x => x.City).NotEmpty().WithMessage(ValidationMessages.GetNotEmptyMessage("{PropertyName}"));
//             RuleFor(x => x.Address).NotEmpty().WithMessage(ValidationMessages.GetNotEmptyMessage("{PropertyName}"));
//             RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage(ValidationMessages.GetNotEmptyMessage("{PropertyName}"));
//             RuleFor(x => x.Email).NotEmpty().WithMessage(ValidationMessages.GetNotEmptyMessage("{PropertyName}"));
//             RuleFor(x => x.Email).EmailAddress().WithMessage(ValidationMessages.GetEmailAddressMessage());
//             RuleFor(studentCommand => studentCommand.Email).
//                Must(email => !dbContext.GetEntities<Teacher>().Any(s => s.Email == email)).WithMessage("Электронная почта уже зарегистрирована. Пожалуйста, укажите другой адрес электронной почты.");
//
//         }
//     }
// }
