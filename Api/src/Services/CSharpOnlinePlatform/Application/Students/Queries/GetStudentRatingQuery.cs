using Application.Common.Interfaces;
using Application.Courses.DTO;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Students.Queries
{
    public class GetStudentRatingQuery : IRequest<RatingDTO>
    {
        public Guid CourseId { get; }
        public Guid StudentId { get; }

        public GetStudentRatingQuery(Guid courseId, Guid studentId)
        {
            CourseId = courseId;
            StudentId = studentId;
        }
    }

    public class GetStudentRatingHandler : IRequestHandler<GetStudentRatingQuery, RatingDTO>
    {
        private readonly IApplicationDbContext _dbContext;

        public GetStudentRatingHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<RatingDTO> Handle(GetStudentRatingQuery request, CancellationToken cancellationToken)
        {

            var student = await _dbContext.GetEntities<StudentCourse>().AsNoTracking()
                        .Include(s => s.Course.Themes).ThenInclude(s => s.Exercises)
                        .Include(s => s.Course.Themes).ThenInclude(s => s.Tests)
                        .Include(s => s.Exercises).ThenInclude(s => s.Exercise)
                        .Include(s => s.Tests).ThenInclude(s => s.Test)
                        .Where(s => s.CourseId == request.CourseId && s.StudentId == request.StudentId)
                        .FirstAsync(cancellationToken: cancellationToken);

            var studentPos = await _dbContext.GetEntities<StudentCourse>().AsNoTracking()
                        .Where(s => s.CourseId == request.CourseId)
                        .OrderByDescending(s => s.Exercises.Where(s => s.Status == Status.Passed).Sum(s => s.Exercise.Rating) +
                                                    s.Tests.Where(s => s.Status == Status.Passed).Sum(s => s.Test.Rating))
                        .ThenBy(s => s.Exercises.Count() + s.Tests.Count())
                        .ToListAsync(cancellationToken: cancellationToken);

            return new RatingDTO()
            {
                TotalRate = student.Course.Themes.Sum(s => (s.Tests != null ? s.Tests.Sum(s => s.Rating) : 0) + (s.Exercises != null ? s.Exercises.Sum(s => s.Rating) : 0)),
                CompletedRate = GetCompletedRateExercise(student) + GetCompletedRateTest(student),
                Position = studentPos.IndexOf(studentPos.First(s => s.StudentId == request.StudentId && s.CourseId == request.CourseId)) + 1
            };
        }

        private int GetCompletedRateExercise(StudentCourse student)
        {
            if (student.Exercises != null)
                return student.Exercises.Where(s => s.Status == Status.Passed).Sum(s => s.Exercise.Rating);
            return 0;
        }

        private int GetCompletedRateTest(StudentCourse student)
        {
            if (student.Tests != null)
                return student.Tests.Where(s => s.Status == Status.Passed).Sum(s => s.Test.Rating);
            return 0;
        }
    }
}
