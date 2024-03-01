using Application.Common.Interfaces;
using Application.Teachers.DTO;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Teachers.Queries.RatingQuery
{
    public class GetStudentsRatingQuery : IRequest<List<GetRatingDTO>>
    {
        public Guid CourseId { get; }
        public Guid TeacherId { get; }

        public GetStudentsRatingQuery(Guid courseId, Guid teacherId)
        {
            CourseId = courseId;
            TeacherId = teacherId;
        }

    }

    public class GetStudentsRatingQueryyHandler : IRequestHandler<GetStudentsRatingQuery, List<GetRatingDTO>>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetStudentsRatingQueryyHandler(IApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<List<GetRatingDTO>> Handle(GetStudentsRatingQuery request, CancellationToken cancellationToken)
        {
            var studentCourses = await _dbContext.GetEntities<StudentCourse>().AsNoTracking()
                        .Include(s => s.Exercises).ThenInclude(s => s.Exercise)
                        .Include(s => s.Tests).ThenInclude(s => s.Test)
                        .Where(s => s.CourseId == request.CourseId && s.Course.TeacherId == request.TeacherId)
                        .OrderByDescending(s => s.Exercises.Where(s => s.Status == Status.Passed).Sum(s => s.Exercise.Rating) +
                                                    s.Tests.Where(s => s.Status == Status.Passed).Sum(s => s.Test.Rating))
                        .ThenBy(s => (s.Exercises.Where(s => s.Status == Status.Passed).Sum(s => s.Exercise.Rating) +
                                      s.Tests.Where(s => s.Status == Status.Passed).Sum(s => s.Test.Rating)) > 0 ? s.Exercises.Count() + s.Tests.Count() : (s.Exercises.Count() + s.Tests.Count()) * -1)
                        .ToListAsync(cancellationToken: cancellationToken);
            return _mapper.Map<List<GetRatingDTO>>(studentCourses);
        }
    }
}
