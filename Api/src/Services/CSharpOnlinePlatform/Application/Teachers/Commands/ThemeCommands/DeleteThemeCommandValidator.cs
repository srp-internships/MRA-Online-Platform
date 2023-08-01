using Application.Common.Interfaces;
using Domain.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Teachers.Commands.ThemeCommands
{
    public class DeleteThemeCommandValidator : AbstractValidator<DeleteThemeCommand>
    {
        public DeleteThemeCommandValidator(IApplicationDbContext dbContext)
        {
            RuleFor(query => query.TeacherId).Must((query, teacherGuid) =>
            {
                return dbContext.GetEntities<Teacher>()
                .Include(t => t.LeadingCourses).ThenInclude(c => c.Themes)
                .Where(t => t.Id == teacherGuid)
                .SelectMany(s => s.LeadingCourses)
                .SelectMany(c => c.Themes)
                .Any(t => t.Id == query.ThemeId);
            }).WithMessage($"Ваш доступ ограничен.");
        }
    }
}
