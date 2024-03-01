using Application.Common.Interfaces;
using Domain.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.CodeAnalyzer.Commands
{
    public class AnalyzeTestCommandValidator : AbstractValidator<AnalyzeTestCommand>
    {
        public AnalyzeTestCommandValidator(IApplicationDbContext dbContext)
        {
            RuleFor(s => s.StudentId).Must(studentId =>
            {
                return dbContext.GetEntities<StudentCourse>().Any(s => s.StudentId == studentId);
            }).WithMessage("Студент не найден.");

            RuleFor(s => s.TestId).Must(testId =>
            {
                return dbContext.GetEntities<Test>().Any(s => s.Id == testId);
            }).WithMessage("Тест не найден.");

            RuleFor(s => s.VariantId).Must((query, variantId) =>
            {
                return dbContext.GetEntities<VariantTest>().Any(s => s.Id == variantId && s.TestId == query.TestId);
            }).WithMessage("Вариант не найден.");

            RuleFor(s => s.TestId).Must((query, testId )=>
            {
                return !dbContext.GetEntities<StudentCourseTest>()
                        .Include(s => s.StudentCourse)
                        .Any(s => s.StudentCourse.StudentId == query.StudentId && s.TestId == testId);
            }).WithMessage("Задание уже выполнено.");
        }
    }
}
