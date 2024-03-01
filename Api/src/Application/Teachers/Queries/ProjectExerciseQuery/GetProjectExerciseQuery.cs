using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Teachers.Queries.ProjectExerciseQuery
{
    public class GetProjectExerciseQuery : IRequest<List<GetProjectExerciseQueryDTO>>
    {
        public Guid TeacherId { get; }
        public Guid ThemeId { get; }

        public GetProjectExerciseQuery(Guid teacherId, Guid themeId)
        {
            TeacherId = teacherId;
            ThemeId = themeId;
        }
    }

    public class GetProjectExerciseCommandHandler : IRequestHandler<GetProjectExerciseQuery, List<GetProjectExerciseQueryDTO>>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetProjectExerciseCommandHandler(IApplicationDbContext _dbContext, IMapper _mapper)
        {
            this._dbContext = _dbContext;
            this._mapper = _mapper;
        }

        public Task<List<GetProjectExerciseQueryDTO>> Handle(GetProjectExerciseQuery request, CancellationToken cancellationToken)
        {
            return _dbContext.GetEntities<ProjectExercise>().AsNoTracking()
                .Where(t => t.ThemeId == request.ThemeId)
                .Select(t => _mapper.Map<GetProjectExerciseQueryDTO>(t))
                .ToListAsync(cancellationToken);
        }
    }
}
