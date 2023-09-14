using Application.Common.Interfaces;
using Application.Exercises.DTO;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Students.Queries
{
    public class GetTestsQuery : IRequest<List<StudentTestDTO>>
    {
        public GetTestsQuery(Guid themeId, Guid studentId)
        {
            ThemeId = themeId;
            StudentId = studentId;
        }

        public Guid ThemeId { get; }
        public Guid StudentId { get; }
    }

    public class GetTestsQueryHandler : IRequestHandler<GetTestsQuery, List<StudentTestDTO>>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetTestsQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<List<StudentTestDTO>> Handle(GetTestsQuery request, CancellationToken cancellationToken)
        {
            return await _dbContext.GetEntities<Test>().AsNoTracking()
                .Include(s => s.Variants)
                .Include(t => t.Theme).ThenInclude(t => t.Course)
                .Include(s => s.Students.Where(s => s.StudentCourse.StudentId == request.StudentId))
                .Where(s => s.ThemeId == request.ThemeId && s.Theme.StartDate.Date <= DateTime.Today)
                .Select(e => _mapper.Map<StudentTestDTO>(e))
                .ToListAsync(cancellationToken: cancellationToken);
        }
    }
}
