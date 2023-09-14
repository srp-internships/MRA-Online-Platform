using Application.Common.Interfaces;
using Application.Exercises.DTO;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Students.Queries
{
    public class GetExercisesQuery : IRequest<List<StudentExerciseDTO>>
    {
        public Guid ThemeId { get; }

        public Guid StudentId { get; }

        public GetExercisesQuery(Guid themeGuid, Guid studentGuid)
        {
            ThemeId = themeGuid;
            StudentId = studentGuid;
        }
    }

    public class GetExerciseQueryHandler : IRequestHandler<GetExercisesQuery, List<StudentExerciseDTO>>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetExerciseQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<List<StudentExerciseDTO>> Handle(GetExercisesQuery request, CancellationToken cancellationToken)
        {
            return await _dbContext.GetEntities<Exercise>().AsNoTracking()
                .Include(t => t.Theme).ThenInclude(t => t.Course)
                .Include(s => s.Students.Where(s => s.StudentCourse.StudentId == request.StudentId))
                .Where(s => s.ThemeId == request.ThemeId && s.Theme.StartDate.Date <= DateTime.Today)
                .Select(e => _mapper.Map<StudentExerciseDTO>(e))
                .ToListAsync(cancellationToken: cancellationToken);
        }
    }
}