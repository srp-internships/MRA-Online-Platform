using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace Application.Teachers.Queries.StudentCourseProjectExerciseQuery
{
    public class GetStudentCourseProjectExerciseQuery : IRequest<List<GetStudentCourseProjectExerciseDTO>>
    {
        public Guid ProjectExerciseId { get; set; }
        public Guid TeacherId { get; set; }

        public GetStudentCourseProjectExerciseQuery(Guid projectExerciseId, Guid teacherId)
        {
            ProjectExerciseId = projectExerciseId;
            TeacherId = teacherId;
        }
    }

    public class GetStudentCourseProjectExerciseHandler : IRequestHandler<GetStudentCourseProjectExerciseQuery, List<GetStudentCourseProjectExerciseDTO>>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetStudentCourseProjectExerciseHandler(IApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<List<GetStudentCourseProjectExerciseDTO>> Handle(GetStudentCourseProjectExerciseQuery request, CancellationToken cancellationToken)
        {
            return await _dbContext.GetEntities<StudentCourseProjectExercise>().AsNoTracking()
                .Include(s => s.StudentCourse)
                .Where(t => t.ProjectExerciseId == request.ProjectExerciseId && t.Status == Status.WaitForTeacher)
                .Select(s => _mapper.Map<GetStudentCourseProjectExerciseDTO>(s))
                .ToListAsync(cancellationToken);
        }
    }
}
