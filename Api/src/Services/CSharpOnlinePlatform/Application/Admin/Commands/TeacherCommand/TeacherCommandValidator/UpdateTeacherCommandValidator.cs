// using Application.Admin.Commands.TeacherCommand.TeacherCRUD;
// using Application.Common.Interfaces;
// using Core.ValidationsBehaviours;
// using Domain.Entities;
// using FluentValidation;
//
// namespace Application.Admin.Commands.TeacherCommand
// {
//     public class UpdateTeacherCommandValidator :AbstractValidator<UpdateTeacherCommand>
//     {
//         public UpdateTeacherCommandValidator(IApplicationDbContext dbContext)
//         {
//             RuleFor(x => x.FirstName).NotEmpty().WithMessage(ValidationMessages.GetNotEmptyMessage("{PropertyName}"));
//             RuleFor(x => x.LastName).NotEmpty().WithMessage(ValidationMessages.GetNotEmptyMessage("{PropertyName}"));
//             RuleFor(x => x.Email).EmailAddress().WithMessage(ValidationMessages.GetEmailAddressMessage());
//             RuleFor(x => x.DateOfBirth).NotEmpty().WithMessage(ValidationMessages.GetNotEmptyMessage("{PropertyName}"));
//             RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage(ValidationMessages.GetNotEmptyMessage("{PropertyName}"));
//             RuleFor(x => x.Country).NotEmpty().WithMessage(ValidationMessages.GetNotEmptyMessage("{PropertyName}"));
//             RuleFor(x => x.Region).NotEmpty().WithMessage(ValidationMessages.GetNotEmptyMessage("{PropertyName}"));
//             RuleFor(x => x.City).NotEmpty().WithMessage(ValidationMessages.GetNotEmptyMessage("{PropertyName}"));
//             RuleFor(x => x.Address).NotEmpty().WithMessage(ValidationMessages.GetNotEmptyMessage("{PropertyName}"));
//
//             RuleFor(x => x.Id).Must((teacherId) =>
//             {
//                 return dbContext.GetEntities<Teacher>().Any(x => x.Id == teacherId);
//             }).WithMessage($"Ваш доступ ограничен.");
//         }
//     }
// }
