using Application.Common.Interfaces;
using Application.Themes.DTO;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Teachers.Queries.ThemeQuery
{
    public class GetTeacherThemeQuery : IRequest<ThemeDTO>
    {
        public Guid ThemeId { get; }

        public Guid TeacherId { get; }

        public GetTeacherThemeQuery(Guid themeGuid, Guid teacherGuid)
        {
            ThemeId = themeGuid;
            TeacherId = teacherGuid;
        }
    }

    public class GetByGuidTeacherThemeQueryHandler : IRequestHandler<GetTeacherThemeQuery, ThemeDTO>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetByGuidTeacherThemeQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<ThemeDTO> Handle(GetTeacherThemeQuery request, CancellationToken cancellationToken)
        {
            var theme = await _dbContext.GetEntities<Theme>()
                .Include(t => t.Exercises).AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == request.ThemeId, cancellationToken);
            return _mapper.Map<ThemeDTO>(theme);
        }
    }
}
