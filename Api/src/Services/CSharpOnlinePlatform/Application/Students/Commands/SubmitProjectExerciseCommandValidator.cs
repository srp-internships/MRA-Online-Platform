using Application.Common.Interfaces;
using Core.ValidationsBehaviours;
using Domain.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Application.Students.Commands
{
    public class SubmitProjectExerciseCommandValidator : AbstractValidator<SubmitProjectExerciseCommand>
    {
        IApplicationDbContext _applicationDbContext;

        public SubmitProjectExerciseCommandValidator(IApplicationDbContext _dbContext)
        {
            _applicationDbContext = _dbContext;

            FileNotNull().DependentRules(() => FileSizeNotLarge())
                .DependentRules(() => ProjectShouldExists())
                .DependentRules(() => FileTypeShouldBeZip())
                .DependentRules(() => StudentShouldExist());
        }

        IRuleBuilderOptions<SubmitProjectExerciseCommand, IFormFile> FileNotNull()
        {
            return RuleFor(s => s.File).NotNull().WithMessage(ValidationMessages.GetNotNullMessage("{PropertyName}")); ;
        }

        IRuleBuilderOptions<SubmitProjectExerciseCommand, IFormFile> FileSizeNotLarge()
        {
            return RuleFor(s => s.File).Must(file =>
            {
                return file.Length <= 5000000;
            }).WithMessage("Размер загружаемого проекта не должен превышать 5 мегабайтов.");
        }

        IRuleBuilderOptions<SubmitProjectExerciseCommand, IFormFile> FileTypeShouldBeZip()
        {
            return RuleFor(s => s.File).Must(file =>
            {
                return file.ContentType == "application/x-zip-compressed";
            }).WithMessage("Загружать только архив типа .zip");
        }

        IRuleBuilderOptions<SubmitProjectExerciseCommand, Guid> ProjectShouldExists()
        {
            return RuleFor(pe => pe.ProjectExerciseId).Must(projId =>
            {
                return _applicationDbContext.GetEntities<ProjectExercise>().Any(p => p.Id == projId);
            }).WithMessage("Проект не найден.");
        }

        IRuleBuilderOptions<SubmitProjectExerciseCommand, Guid> StudentShouldExist()
        {
            return RuleFor(s => s.StudentId).Must(studentId =>
            {
                return _applicationDbContext.GetEntities<StudentCourse>().Any(s => s.StudentId == studentId);
            }).WithMessage("Студент не найден.");
        }
    }
}
