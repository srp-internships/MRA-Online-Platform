using Application.Common.Interfaces;
using Application.Exercises.DTO;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Students.Queries
{
    public class GetStudentProjectExerciseQuery : IRequest<List<StudentProjectExerciseDTO>>
    {
        public Guid ThemeId { get; }

        public Guid StudentId { get; }

        public GetStudentProjectExerciseQuery(Guid themeGuid, Guid studentGuid)
        {
            ThemeId = themeGuid;
            StudentId = studentGuid;
        }
    }

    public class GetStudentProjectExerciseQueryHandler : IRequestHandler<GetStudentProjectExerciseQuery, List<StudentProjectExerciseDTO>>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetStudentProjectExerciseQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<List<StudentProjectExerciseDTO>> Handle(GetStudentProjectExerciseQuery request, CancellationToken cancellationToken)
        {
            return await _dbContext.GetEntities<ProjectExercise>().AsNoTracking()
                .Include(t => t.Theme).ThenInclude(t => t.Course)
                .Include(s => s.Students.Where(s => s.StudentCourse.StudentId == request.StudentId))
                .Where(s => s.ThemeId == request.ThemeId && s.Theme.StartDate.Date <= DateTime.Today)
                .Select(e => _mapper.Map<StudentProjectExerciseDTO>(e))
                .ToListAsync(cancellationToken: cancellationToken);
        }
    }
}
