using Application.CodeAnalyzer.DTO;
using Application.Common.Interfaces;
using Core.ValidationsBehaviours;
using Domain.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.CodeAnalyzer.Commands
{
    public class AnalyzeCodeCommandValidator : AbstractValidator<AnalyzeCodeCommand>
    {
        IApplicationDbContext _applicationDbContext;
        public AnalyzeCodeCommandValidator(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;

            AnalyzeCodeCommandParameterNotNull().DependentRules(() => AnalyzeCodeCommandVersionNotNull())
                .DependentRules(() => AnalyzeCodeCommandParameterCodeNotEmpty().
                        DependentRules(() => AnalyzeCodeCommandParameterStudentShouldExists()).
                            DependentRules(() => AnalyzeCodeCommandParameterStudentShouldHaveThisExercise()));
        }

        IRuleBuilderOptions<AnalyzeCodeCommand, AnalyzeCodeCommandParameter> AnalyzeCodeCommandParameterNotNull()
        {
            return RuleFor(analyzeCommand => analyzeCommand.Parameters).NotNull().WithMessage(ValidationMessages.GetNotNullMessage("{PropertyName}"));
        }

        IRuleBuilderOptions<AnalyzeCodeCommand, VersionDTO> AnalyzeCodeCommandVersionNotNull()
        {
            return RuleFor(analyzeCommand => analyzeCommand.Version).NotNull().WithMessage(ValidationMessages.GetNotNullMessage("{PropertyName}"));
        }

        IRuleBuilderOptions<AnalyzeCodeCommand, string> AnalyzeCodeCommandParameterCodeNotEmpty()
        {
            return RuleFor(analyzeCommand => analyzeCommand.Parameters.Code).Must(s => !string.IsNullOrWhiteSpace(s)).WithMessage("Необходимо заполнить поле с кодом.");
        }

        IRuleBuilderOptions<AnalyzeCodeCommand, Guid> AnalyzeCodeCommandParameterStudentShouldExists()
        {
            return RuleFor(analyzeCommand => analyzeCommand.StudentGuid).Must(commandParameter => _applicationDbContext.GetEntities<Student>().Any(s => s.Id == commandParameter)).WithMessage("Пользователь не найден.");
        }

        IRuleBuilderOptions<AnalyzeCodeCommand, AnalyzeCodeCommand> AnalyzeCodeCommandParameterStudentShouldHaveThisExercise()
        {
            return RuleFor(analyzeCommand => analyzeCommand).Must(commandParameter =>
            {
                return !_applicationDbContext.GetEntities<StudentCourseExercise>()
                        .Include(s => s.StudentCourse)
                        .Where(s => s.StudentCourse.StudentId == commandParameter.StudentGuid && s.ExerciseId == commandParameter.Parameters.Id)
                        .Any(s => s.Status == Status.Passed);
            }).WithMessage("Задание уже выполнено.");
        }
    }
}
