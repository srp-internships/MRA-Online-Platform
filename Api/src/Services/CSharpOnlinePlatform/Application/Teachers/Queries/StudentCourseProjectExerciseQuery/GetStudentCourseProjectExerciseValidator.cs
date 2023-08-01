using Application.Common.Interfaces;
using Domain.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Teachers.Queries.StudentCourseProjectExerciseQuery
{
    public class GetStudentCourseProjectExerciseValidator : AbstractValidator<GetStudentCourseProjectExerciseQuery>
    {
        public GetStudentCourseProjectExerciseValidator(IApplicationDbContext dbContext)
        {
            ProjectShouldExist(dbContext).DependentRules(() => TeacherShouldExist(dbContext))
                .DependentRules(() => ProjectExerciseWithItsTeacher(dbContext));
        }

        IRuleBuilderOptions<GetStudentCourseProjectExerciseQuery, Guid> ProjectShouldExist(IApplicationDbContext dbContext)
        {
            return RuleFor(s => s.ProjectExerciseId).Must((projectExerciseId) =>
            {
                return dbContext.GetEntities<ProjectExercise>().Where(p => p.Id == projectExerciseId).Any();
            }).WithMessage("Проект не найден.");
        }

        IRuleBuilderOptions<GetStudentCourseProjectExerciseQuery, Guid> TeacherShouldExist(IApplicationDbContext dbContext)
        {
            return RuleFor(s => s.TeacherId).Must((teacherId) =>
            {
                var aa = dbContext.GetEntities<Teacher>().Where(p => p.Id == teacherId).FirstOrDefault();
                return dbContext.GetEntities<Teacher>().Where(p => p.Id == teacherId).Any();
            }).WithMessage("Учитель не найден.");
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
