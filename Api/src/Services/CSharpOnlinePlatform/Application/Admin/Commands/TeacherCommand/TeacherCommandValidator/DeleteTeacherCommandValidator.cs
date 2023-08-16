// using Application.Admin.Commands.TeacherCommand.TeacherCRUD;
// using Application.Common.Interfaces;
// using Domain.Entities;
// using FluentValidation;
//
// namespace Application.Admin.Commands.TeacherCommand
// {
//     public class DeleteTeacherCommandValidator : AbstractValidator<DeleteTeacherCommand>
//     {
//         public DeleteTeacherCommandValidator(IApplicationDbContext dbContext)
//         {
//             RuleFor(x => x.TeacherId).Must((teacherId) =>
//             {
//                 return dbContext.GetEntities<Teacher>().Any(x => x.Id == teacherId);
//             }).WithMessage($"Ваш доступ ограничен.");
//         }
//     }
// }
