using Application.Common.Interfaces;
using Domain.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Teachers.Queries.StudentCourseProjectExerciseQuery
{
    public class GetStudentCourseProjectExerciseValidator : AbstractValidator<GetStudentCourseProjectExerciseQuery>
    {
        public GetStudentCourseProjectExerciseValidator(IApplicationDbContext context)
        {
            ProjectShouldExist(context);
            ProjectExerciseWithItsTeacher(context);
        }
        IRuleBuilderOptions<GetStudentCourseProjectExerciseQuery, Guid> ProjectShouldExist(IApplicationDbContext dbContext)
        {
            return RuleFor(s => s.ProjectExerciseId).Must((projectExerciseId) =>
            {
                return dbContext.GetEntities<ProjectExercise>().Any(p => p.Id == projectExerciseId);
            }).WithMessage("Проект не найден.");
        }
        
        IRuleBuilderOptions<GetStudentCourseProjectExerciseQuery, Guid> ProjectExerciseWithItsTeacher(IApplicationDbContext dbContext)
        {
            return RuleFor(s => s.TeacherId).Must((query,teacherId) =>
            {
                return dbContext.GetEntities<ProjectExercise>()
                .Include(p => p.Theme)
                .ThenInclude(t => t.Course)
                .Where(p => p.Theme.Course.TeacherId == teacherId)
                .Any(p => p.Id == query.ProjectExerciseId);
            }).WithMessage("Отказано в доступе.");
        }
    }
}
