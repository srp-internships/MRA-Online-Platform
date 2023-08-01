using Application.Common.Interfaces;
using Application.Exercises.DTO;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Teachers.Queries.ExerciseQuery
{
    public class GetExercisesTeacherQuery : IRequest<List<TeacherExerciseDTO>>
    {
        public Guid ThemeId { get; }

        public Guid TeacherId { get; }

        public GetExercisesTeacherQuery(Guid themeId, Guid teacherGuid)
        {
            ThemeId = themeId;
            TeacherId = teacherGuid;
        }
    }

    public class GetExercisesTeacherQueryHandler : IRequestHandler<GetExercisesTeacherQuery, List<TeacherExerciseDTO>>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetExercisesTeacherQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public Task<List<TeacherExerciseDTO>> Handle(GetExercisesTeacherQuery request, CancellationToken cancellationToken)
            => _dbContext.GetEntities<Exercise>()
                        .Where(ex => ex.ThemeId == request.ThemeId)
                        .Select(x => _mapper.Map<TeacherExerciseDTO>(x))
                        .ToListAsync(cancellationToken);
    }
}
