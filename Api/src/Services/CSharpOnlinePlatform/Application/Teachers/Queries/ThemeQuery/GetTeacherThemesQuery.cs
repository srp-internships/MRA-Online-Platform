using Application.Common.Interfaces;
using Application.Themes.DTO;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Teachers.Queries.ThemeQuery
{
    public class GetTeacherThemesQuery : IRequest<List<ThemeDTO>>
    {
        public Guid CourseId { get; }

        public Guid TeacherId { get; }

        public GetTeacherThemesQuery(Guid courseId, Guid teacherId)
        {
            CourseId = courseId;
            TeacherId = teacherId;
        }
    }

    public class GetTeacherThemesQueryHandler : IRequestHandler<GetTeacherThemesQuery, List<ThemeDTO>>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetTeacherThemesQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<List<ThemeDTO>> Handle(GetTeacherThemesQuery request, CancellationToken cancellationToken)
        {
            var themes = await _dbContext.GetEntities<Theme>().AsNoTracking()
                .Where(c => c.CourseId == request.CourseId)
                .OrderBy(s => s.StartDate)
                .ToListAsync(cancellationToken: cancellationToken);
            var themeToCourseDto = _mapper.Map<List<ThemeDTO>>(themes);
            return themeToCourseDto;
        }
    }
}
