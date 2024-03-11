using Application.Common.Interfaces;
using Application.Exercises.DTO;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Teachers.Queries.TestsQuery
{
    public class GetTestsTeacherQuery : IRequest<List<TeacherTestDTO>>
    {
        public GetTestsTeacherQuery(Guid themeId)
        {
            ThemeId = themeId;
        }

        public Guid ThemeId { get; set; }   
    }

    public class GetTestsTeacherQueryHandler : IRequestHandler<GetTestsTeacherQuery, List<TeacherTestDTO>>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetTestsTeacherQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public Task<List<TeacherTestDTO>> Handle(GetTestsTeacherQuery request, CancellationToken cancellationToken)
            => _dbContext.GetEntities<Test>().AsNoTracking()
                        .Include(s => s.Variants)
                        .Where(ex => ex.ThemeId == request.ThemeId)
                        .Select(x => _mapper.Map<TeacherTestDTO>(x))
                        .ToListAsync(cancellationToken);
    }
}
