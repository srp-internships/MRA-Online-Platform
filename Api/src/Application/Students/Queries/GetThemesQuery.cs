using Application.Common.Interfaces;
using Application.Courses.DTO;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Students.Queries
{

    public class GetThemesQuery : IRequest<List<ShortThemeDTO>>
    {
        public Guid CourseGuid { get; }

        public Guid StudentGuid { get; }

        public GetThemesQuery(Guid courseGuid, Guid studentGuid)
        {
            CourseGuid = courseGuid;
            StudentGuid = studentGuid;
        }
    }

    public class GetThemesQueryHandler : IRequestHandler<GetThemesQuery, List<ShortThemeDTO>>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetThemesQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<List<ShortThemeDTO>> Handle(GetThemesQuery request, CancellationToken cancellationToken)
        {
            var themes = await _dbContext.GetEntities<Course>().AsNoTracking()
                .Include(s => s.Themes)
                .ThenInclude(s => s.Tests)
                .Include(s => s.Students)
                .Where(c => c.Id == request.CourseGuid)
                .SelectMany(sc => sc.Themes)
                .OrderBy(s => s.StartDate)
                .ToListAsync(cancellationToken: cancellationToken);
            var themeToCourseDto = _mapper.Map<List<ShortThemeDTO>>(themes);
            return themeToCourseDto;
        }
    }
}